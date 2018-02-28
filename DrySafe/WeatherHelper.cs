using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;

namespace DrySafe
{
  public class WeatherStatus
  {
    public Boolean WillItRain => RainLevels == null ? false : RainLevels.Any(level => !level.Contains("Pas de précipitations"));
    public String[] RainLevels { get; set; }

    public String GetDescription()
    {
      return String.Join(", ", RainLevels);
    }
  }

  public class WeatherHelper
  {
    public static WeatherStatus GetRainInNextHour()
    {
      DrySafe.Logger.Info($"Querying weather API...");

      using (WebClient wc = new WebClient())
      {
        wc.Encoding = System.Text.Encoding.UTF8;
        var json = wc.DownloadString("http://www.meteofrance.com/mf3-rpc-portlet/rest/pluie/751090");
        dynamic data = Newtonsoft.Json.Linq.JObject.Parse(json);

        return new WeatherStatus()
        {
          RainLevels = ((JArray)data.niveauPluieText).Select(jv => (String)jv).ToArray()
        };
      }
    }

  }
}
