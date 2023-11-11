using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Payment_Group5.Services
{
    public class CurrencyConverterService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "7bb59db3037f455d24c70e8c"; // Replace with your actual API key
        private readonly string _apiBaseUrl = "https://v6.exchangerate-api.com/v6/";

        public CurrencyConverterService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> ConvertCurrency(decimal amount, string fromCurrency, string toCurrency)
        {
            try
            {
                // Get the exchange rate from fromCurrency to toCurrency
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}{_apiKey}/pair/{fromCurrency}/{toCurrency}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(responseBody);

                if (json["result"].Value<string>() != "success")
                {
                    throw new Exception("Failed to fetch currency data.");
                }

                decimal conversionRate = json["conversion_rate"].Value<decimal>();

                decimal convertedAmount = amount * conversionRate;
                return convertedAmount;
            }
            catch (HttpRequestException e)
            {
                // Handle network issues
                throw new Exception("Network error occurred.", e);
            }
            catch (Exception e)
            {
                // Handle other exceptions
                throw new Exception("An error occurred while converting currency.", e);
            }
        }
    }
}