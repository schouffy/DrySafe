using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DrySafe
{
    public class WeatherStatus
    {
        public Boolean WillItRain => RainLevels == null ? false : RainLevels.Any(level => !level.Contains("Temps sec"));
        public String[] RainLevels { get; set; }

        public String GetDescription()
        {
            return String.Join(", ", RainLevels);
        }
    }

    public class WeatherHelper
    {
        static string _url = Environment.GetEnvironmentVariable("MeteoFranceUrl");

        public static async Task<WeatherStatus> GetRainInNextHour()
        {
            DrySafe.Logger.Log(LogLevel.Information, $"Querying weather API...");

            using (var httpClient = new HttpClient())
            {
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, _url))
                {
                    var response = await httpClient.SendAsync(requestMessage);

                    dynamic data = JObject.Parse(await response.Content.ReadAsStringAsync());
                    return new WeatherStatus()
                    {
                        RainLevels = ((JArray)data.forecast).Select(fc => (string)fc.SelectToken("desc")).ToArray()
                    };
                }
            }
        }

    }
}
