using StocksAppWithConfiguration.ServiceContracts;
using System.Net.Http;
using System.Text.Json;

namespace StocksAppWithConfiguration.Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public FinnhubService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public Dictionary<string, object> GetStockPriceQuote(string stockSymbol)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["RequestToken"]}"),
                Method = HttpMethod.Get
             };

            HttpResponseMessage httpResponseMessage = httpClient.Send(httpRequestMessage);

            string responseBody = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();

            Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
                if (responseDictionary == null) 
                {
                    throw new InvalidOperationException("No response from finnhub.io");
                }

                if (responseDictionary.ContainsKey("error"))
                {
                    throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));
                }
                return responseDictionary;
         }
        

        public Dictionary<string, object> GetCompanyProfile(string stockSymbol)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["RequestToken"]}"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = httpClient.Send(httpRequestMessage);

                string responseBody = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();

                Dictionary<string, object>? responseDictionary =
                JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

                if (responseDictionary == null)
                    throw new InvalidOperationException("No response from finnhub.io");

                if (responseDictionary.ContainsKey("error"))
                    throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));

                return responseDictionary;
            }
        }
    }
}
