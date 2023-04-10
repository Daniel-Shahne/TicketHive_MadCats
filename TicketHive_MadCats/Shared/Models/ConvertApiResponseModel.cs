using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketHive_MadCats.Shared.Models
{
    // ConvertApiResponse myDeserializedClass = JsonConvert.DeserializeObject<ConvertApiResponse>(myJsonResponse);

    // Make calls to https://api.apilayer.com/exchangerates_data/convert with params
    // apikey (in header)
    // from: 3 letter currency abbr
    // to: -//-
    // amount: amount to convert
    public class Info
    {
        [JsonProperty("timestamp")]
        public int Timestamp { get; set; }

        [JsonProperty("rate")]
        public double Rate { get; set; }
    }

    public class Query
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }
    }

    public class ConvertApiResponseModel
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("query")]
        public Query Query { get; set; }

        [JsonProperty("info")]
        public Info Info { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("result")]
        public double Result { get; set; }
    }
}
