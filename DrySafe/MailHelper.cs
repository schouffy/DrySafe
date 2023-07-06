using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace DrySafe
{
    class MailHelper
    {
        public static async Task SendMail(String subject, String message)
        {
            String recipient = Environment.GetEnvironmentVariable("ErrorEmailRecipient");

            MailjetClient client = new MailjetClient(Environment.GetEnvironmentVariable("MailJetApiKey"), Environment.GetEnvironmentVariable("MailJetApiSecret"))
            {
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }.Property(Send.Messages, new JArray {
                new JObject {
                 {"From", new JObject {
                  {"Email", Environment.GetEnvironmentVariable("ErrorMailSenderAddress")},
                  {"Name", "DrySafe"}
                  }},
                 {"To", new JArray {
                  new JObject {
                  {"Email", recipient}
                   }
                  }},
                 {"Subject", subject},
                 {"TextPart", message}
                 }
                });
            MailjetResponse response = await client.PostAsync(request);
        }
    }
}
