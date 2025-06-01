using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid;
using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using Finance.Data.Migrations;
using Finance.Data.Models;
using Finance.Data.Repositories.Interfaces;
using Finance.Services.Models;
using Finance.Services.Services.Interfaces;

namespace Finance.Services.Services
{
    public class CurrencyService : ICurrencyService
    {
        private ICurrencyRepository _repositoryCurrency;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public CurrencyService(ICurrencyRepository repositoryCurrency, IMemoryCache memoryCache, IConfiguration configuration, ILogger<PushSubscriptionService> logger)
        {
            _repositoryCurrency = repositoryCurrency;
            _memoryCache = memoryCache;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<List<CurrencyModel>> CurrencyLookup(bool justPopular)
        {
            return await _repositoryCurrency
                .GetCurrencies(justPopular)
                .Select(x => new CurrencyModel(x.CurrencyId, x.CurrencyName, x.IsPopular))
                .ToListAsync();
        }

        public async Task<Dictionary<string, decimal>> GetRate(string toCurrency)
        {
            if (!_memoryCache.TryGetValue(toCurrency, out Dictionary<string,decimal> rates))
            {
                var allCurrencies = await _repositoryCurrency.GetAllCurrencyNamesAsync();
                rates = await GetCurrencyRates(allCurrencies, toCurrency);

                if (rates != null)
                {
                    _memoryCache.Set(toCurrency, rates, TimeSpan.FromDays(1));
                    _memoryCache.Set(toCurrency + "Backup", rates, TimeSpan.FromDays(30));

                    return rates;
                }
                else if (rates == null && _memoryCache.TryGetValue(toCurrency + "Backup", out Dictionary<string, decimal> ratesBackup))
                {
                    return ratesBackup;
                }
                else
                {
                    throw new ApplicationException("Service is down and we don't have backup value");
                }
            }
            return rates;
        }

        public async Task<Dictionary<string, decimal>> GetCurrencyRates(List<string> fromCurrency, string toCurrency)
        {
            using (var client = new HttpClient())
            {
                var fromCurrencies = System.Net.WebUtility.UrlEncode(string.Join(",", fromCurrency));
                client.BaseAddress = new Uri("https://api.apilayer.com");
                client.DefaultRequestHeaders.Add("apikey", _configuration.GetSection("Apilayer:ApiKey").Value);

                var url = client.BaseAddress + "exchangerates_data/latest?symbols=" + fromCurrencies + "&base=" + toCurrency;

                var rates = new Dictionary<string, decimal>();
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var jsonObject = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<dynamic>(jsonObject);

                    foreach (var currency in fromCurrency)
                    {
                        rates.Add(currency, (decimal)data.rates[currency]);
                    }

                    return rates;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Apilayer has not returned currency rates");

                    return null;
                }
            }
        }

        public async Task<List<LookupItem>> GetUserCurrencies(int userId)
        {
            return await  _repositoryCurrency
                .GetUserCurrencies(userId)
                .Select(x => new LookupItem(x.CurrencyId, x.CurrencyName))
                .ToListAsync();
        }

        public async Task SaveUserCurrencies(List<LookupItem> currencies, int userId)
        {
               var currencyList = currencies
                .Select(x => new Currency { CurrencyId = x.Id, CurrencyName = x.Title})
                .ToList();

            await _repositoryCurrency.SaveUserCurrencies(currencyList, userId);
        }
    }
}
