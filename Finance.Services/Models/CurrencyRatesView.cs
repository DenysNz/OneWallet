using System.Text.Json.Serialization;

namespace Finance.Services.Models
{
    public class CurrencyRatesView
    {
        //To do: thith class has to be fixed in the future
        public Quotes quotes { get; set; }
        public string source { get; set; }
        public bool success { get; set; }
        public int timestamp { get; set; }

        public class Quotes
        {
            public decimal USDEUR { get; set; }
        }
    }
}
