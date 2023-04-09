using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketHive_MadCats.Shared.Models;

namespace BusinessLogic
{
    public class ConvertApiCaller
    {
        private readonly string apiAccessKey = "QPobaiyjwse6715PwdaQDSabrT9MWrqP";
        private HttpClient httpClient = new();

        public ConvertApiCaller()
        {
            httpClient.DefaultRequestHeaders.Add("apikey", apiAccessKey);
            httpClient.BaseAddress = new Uri("https://api.apilayer.com/exchangerates_data/convert");
        }

        public async Task<ConvertApiResponseModel?> ConvertCurrency(string from, string to, int amount)
        {
            string url = $"?to={to}&from={from}&amount={amount}";
            var response = await httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            ConvertApiResponseModel? convertApiResponseModel = JsonConvert.DeserializeObject<ConvertApiResponseModel>(json);
            return convertApiResponseModel;
        }
    }
}
