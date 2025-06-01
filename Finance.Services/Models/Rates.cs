using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Finance.Services.Models
{
    public class Rates
    {
        [JsonPropertyName("EUR")]
        public decimal EUR { get; set; }
        //[JsonPropertyName("UAH")]
        //public decimal UAH { get; set; }
    }
}
