using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MollyjoggerBackend.Areas.AdminPanel.Utils
{
    public static class Helper
    {
        public static async Task SendMessage(string messageSubject, string messageBody, string mailTo)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("mollyjogger77@gmail.com", "adminadmin123");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            MailMessage message = new MailMessage("mollyjogger77@gmail.com", mailTo);
            message.Subject = messageSubject;
            message.Body = messageBody;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            try
            {
                await client.SendMailAsync(message);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
