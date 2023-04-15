using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketHive_MadCats.Shared.Models;

namespace BusinessLogic
{
    public static class ConvertApiCaller
    {
        private static readonly string apiAccessKey = "QPobaiyjwse6715PwdaQDSabrT9MWrqP";
        private static HttpClient httpClient = new();

        static ConvertApiCaller()
        {
            httpClient.DefaultRequestHeaders.Add("apikey", apiAccessKey);
        }

        /// <summary>
        /// Sends a request to an API that converts currencies at https://api.apilayer.com/exchangerates_data/convert
        /// and returns a model that contains all the API response body data, if request was successfull. Returns null
        /// otherwise
        /// </summary>
        /// <param name="to">Three letter abbreviation of currency to convert to</param>
        /// <param name="amount">Amount of SEK to convert into "to" currency</param>
        /// <returns></returns>
        public static async Task<ConvertApiResponseModel?> ConvertSekToCurrencyAsync(string to, int amount)
        {
            string baseUrl = "https://api.apilayer.com/exchangerates_data/convert";
            string url = $"?to={to}&from=SEK&amount={amount}";
            var response = await httpClient.GetAsync(baseUrl + url);
            if(response.StatusCode != System.Net.HttpStatusCode.OK) { return null; }
            var json = await response.Content.ReadAsStringAsync();
            ConvertApiResponseModel? convertApiResponseModel = JsonConvert.DeserializeObject<ConvertApiResponseModel>(json);
            return convertApiResponseModel;
        }

        public static async Task<double> ConvertSekToCurrencyAsyncIntOnly(string to, double amount)
        {
            string baseUrl = "https://api.apilayer.com/exchangerates_data/convert";
            string url = $"?to={to}&from=SEK&amount={amount}";
            var response = await httpClient.GetAsync(baseUrl + url);
            var json = await response.Content.ReadAsStringAsync();
            ConvertApiResponseModel? convertApiResponseModel = JsonConvert.DeserializeObject<ConvertApiResponseModel>(json);
            return convertApiResponseModel.Result;
        }

        public static ConvertApiResponseModel? ConvertSekToCurrency(string to, int amount)
        {
            string baseUrl = "https://api.apilayer.com/exchangerates_data/convert";
            string url = $"?to={to}&from=SEK&amount={amount}";
            var response = httpClient.GetAsync(baseUrl + url).GetAwaiter().GetResult();
            if (response.StatusCode != System.Net.HttpStatusCode.OK) { return null; }
            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            ConvertApiResponseModel? convertApiResponseModel = JsonConvert.DeserializeObject<ConvertApiResponseModel>(json);
            return convertApiResponseModel;
        }
    }
}
