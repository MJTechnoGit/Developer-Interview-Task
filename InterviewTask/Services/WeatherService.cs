using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using InterviewTask.Models;
using Newtonsoft.Json;
using System.Threading;
using Newtonsoft.Json.Serialization;
using NLog;

namespace InterviewTask.Services
{
    

    public class WeatherService : IWeatherService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger _logger;

        string apiBaseUrl = ConfigurationManager.AppSettings["APIBaseURL"];
        string apiKey = ConfigurationManager.AppSettings["WeatherAPIKey"];
        string query = "data/2.5/weather?id=";

        DefaultContractResolver contractResolver;
        public WeatherService(ILogger logger) 
        {
            httpClient = new HttpClient()
            {
                BaseAddress = new Uri(apiBaseUrl)
            };

            contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            _logger = logger;
        }

        public async Task<string> GetWeatherDetail(string areaID)
        {
            string uri = String.Format("{0}{1}&appid={2}",query, areaID, apiKey);

            try
            {       
                var httpResponse = httpClient.GetAsync(uri, CancellationToken.None);
                httpResponse.Wait();

                httpResponse.Result.EnsureSuccessStatusCode();

                if (httpResponse.Result.Content is object && httpResponse.Result.Content.Headers.ContentType.MediaType == "application/json")
                {

                    var contentStream = await httpResponse.Result.Content.ReadAsStringAsync();
                    dynamic apiResponse = JsonConvert.DeserializeObject(contentStream, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore });
                    var weatherInfo = JsonConvert.SerializeObject(apiResponse.weather);                    

                    return weatherInfo;
                }

            }
            catch (System.Text.Json.JsonException ex)
            {
                _logger.Error("An error has occurred: " + ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error("An error has occurred: " + ex.Message);
            }

            return null;            
        }

    }
}