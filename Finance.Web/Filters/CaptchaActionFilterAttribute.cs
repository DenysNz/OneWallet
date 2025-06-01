using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using Finance.Services.Models.Support;
//using System.Web.Mvc;

namespace Finance.Web.Filters
{
    public class CaptchaActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var config = context.HttpContext.RequestServices.GetService<IConfiguration>();
            var reCaptchaSecret = config.GetSection("GoogleReCaptcha:SecretKey").Value;

            context.ActionArguments.TryGetValue("request", out object? request);
            var view = (SupportRequestViewModel) request;
            var token = view.Token;

            using (var client = new HttpClient { BaseAddress = new Uri("https://www.google.com") })
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new FormUrlEncodedContent(new[]
               {
                    new KeyValuePair<string, string>("secret", reCaptchaSecret),
                    new KeyValuePair<string, string>("response", token),
                    //new KeyValuePair<string, string>("remoteip", request.UserHostAddress)
                });
                var response = client.PostAsync("/recaptcha/api/siteverify", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    dynamic jsonData = JObject.Parse(jsonResponse);

                    if (!jsonData.success)
                    {
                        throw new HttpResponseException(HttpStatusCode.Unauthorized);
                    }
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }
            }
            base.OnActionExecuting(context);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Do something after the action executes.
        }
    }
}

