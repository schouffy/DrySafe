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
    public class Forecast
    {
        public DateTime Time;
        public String Weather;
    }

    public class WeatherStatus
    {
        public Boolean WillItRain => RainLevels == null ? false : RainLevels.Any(level => !level.Weather.Contains("Temps sec"));
        public Forecast[] RainLevels { get; set; }

        public String GetDescription()
        {
            return String.Join(", ", RainLevels
                .Where(f => !f.Weather.Contains("Temps sec"))
                .Select(f => $"{f.Time.ToString("HH:mm")}: {f.Weather}"));
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
                        RainLevels = ((JArray)data.forecast).Select(fc => new Forecast
                        {
                            Time = TimeZoneInfo.ConvertTimeFromUtc(
                                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds((int)fc.SelectToken("dt")),
                                TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time")),
                            Weather = (string)fc.SelectToken("desc")
                        }).ToArray()
                    };
                }
            }
        }

    }
}
