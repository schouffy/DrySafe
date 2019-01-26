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
          DrySafe.Logger.Info($"Rain is planned ! Sending notification.");
          NotificationHelper.SendNotification(rainStatus.GetDescription());
        }
        else
        {
          DrySafe.Logger.Info($"No rain planned.");
        }
      }
      catch (Exception e)
      {
        Logger.Error($"Error: {e.Message}");
        try
        {
          NotificationHelper.SendNotification($"DrySafe - Erreur: {e.Message}");
        }
        catch (Exception e2)
        {
          MailHelper.SendMail("DrySafe: Error", "One or more errors occured during DrySafe execution: "
            + Environment.NewLine
            + e.Message
            + Environment.NewLine
            + e.StackTrace
            + Environment.NewLine
            + Environment.NewLine
            + e2.Message
            + Environment.NewLine
            + e2.StackTrace
            + Environment.NewLine
            + Environment.NewLine
            ).Wait();
        }
      }
    }
  }
}
