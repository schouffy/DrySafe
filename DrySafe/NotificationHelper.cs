using PushbulletSharp;
using PushbulletSharp.Models.Requests;
using PushbulletSharp.Models.Responses;
using System;
using System.Linq;

namespace DrySafe
{
  public class NotificationHelper
  {
    private static String _apiKey = Environment.GetEnvironmentVariable("PushBulletApiKey");

    public static void SendNotification(String notification)
    {
      DrySafe.Logger.Info($"Send notification : {notification}");

      PushbulletClient client = new PushbulletClient(_apiKey);

      //If you don't know your device_iden, you can always query your devices
      var devices = client.CurrentUsersDevices();

      foreach (var device in devices.Devices.Where(d => d.Fingerprint != null))
      {
        if (device != null)
        {
          PushNoteRequest request = new PushNoteRequest
          {
            DeviceIden = device.Iden,
            Title = "Alerte pluie",
            Body = notification
          };

          PushResponse response = client.PushNote(request);
        }
      }
    }
  }
}
