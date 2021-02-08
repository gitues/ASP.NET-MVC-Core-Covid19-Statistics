using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ASPNETCoreMVCcovid19App.Services
{

    public interface IStatisticsService
    {
        Task<string> GetStatistics(string filter);
    }

    public class StatisticsService : IStatisticsService
    {
        private HttpClient _httpClient;
        private IConfiguration _config;

        public StatisticsService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient == null ? new HttpClient() : httpClient;
            _config = config;
        }

        public async Task<string> GetStatistics(string filter)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_config.GetValue<String>("Url.Base.Api.Covid19") + filter),
                Headers =
                {
                    { "x-rapidapi-key", _config.GetValue<String>("cnf.x.rapidapi.key") },
                    { "x-rapidapi-host", _config.GetValue<String>("cnf.x.rapidapi.host") },
                },
            };
            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();                
            }
        }
    }


}