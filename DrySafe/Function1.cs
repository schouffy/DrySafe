using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DrySafe
{
    public class DrySafe
    {
        public static ILogger Logger;

        [FunctionName("DrySafe")]
        public async Task Run([TimerTrigger("0 30 8,17 * * 1-5"
#if DEBUG
			, RunOnStartup = true
#endif   
			)]TimerInfo myTimer, ILogger log)
        {
            Logger = log;

            Logger.Log(LogLevel.Information, $"C# Timer trigger function executed at: {DateTime.Now}");

            try
            {
                var rainStatus = await WeatherHelper.GetRainInNextHour();
                if (rainStatus.WillItRain)
                {
                    Logger.Log(LogLevel.Information, $"Rain is planned ! Sending notification.");
                    NotificationHelper.SendNotification(rainStatus.GetDescription());
                }
                else
                {
                    Logger.Log(LogLevel.Information, $"No rain planned.");
                }
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.Error, $"Error: {e.Message}");
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
