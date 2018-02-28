using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DrySafe
{
  public static class DrySafe
  {
    public static TraceWriter Logger;

    [FunctionName("DrySafe")]
    public static void Run([TimerTrigger("0 0 9,18 * * 1-5")]TimerInfo myTimer, TraceWriter log)
    {
      Logger = log;

      Logger.Info($"C# Timer trigger function executed at: {DateTime.Now}");

      try
      {
        var rainStatus = WeatherHelper.GetRainInNextHour();
        if (rainStatus.WillItRain)
        {
          NotificationHelper.SendNotification(rainStatus.GetDescription());
        }
      }
      catch (Exception e)
      {
        Logger.Error($"Error: {e.Message}");
        NotificationHelper.SendNotification($"DrySafe - Erreur: {e.Message}");
      }
    }
  }
}
