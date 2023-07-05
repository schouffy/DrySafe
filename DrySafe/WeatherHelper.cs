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
		const string _url = "https://rpcache-aa.meteofrance.com/internet2018client/2.0/nowcast/rain?lat=44.808035&lon=-0.590296";

		public static async Task<WeatherStatus> GetRainInNextHour()
		{
			DrySafe.Logger.Log(LogLevel.Information, $"Querying weather API...");

			using (var httpClient = new HttpClient())
			{
				using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, _url))
				{
					requestMessage.Headers.Authorization =
						new AuthenticationHeaderValue("Bearer", "eyJjbGFzcyI6ImludGVybmV0IiwiYWxnIjoiSFMyNTYiLCJ0eXAiOiJKV1QifQ.eyJqdGkiOiJmMTBmMDYxMjI4ZWU1NDg4MDI3MjhiYWZjYzc4MzE4ZCIsImlhdCI6MTY4ODU3NTE3Nn0.e5grsXfyf1w-KCPGw67cPkAovyfG9nvoF9p0d4wc8po");

					var response = await httpClient.SendAsync(requestMessage);

					dynamic data = JObject.Parse(await response.Content.ReadAsStringAsync());
					return new WeatherStatus()
					{
						RainLevels = ((JArray)data.properties.forecast).Select(fc => (string)fc.SelectToken("rain_intensity_description")).ToArray()
					};
				}
			}
		}

	}
}
