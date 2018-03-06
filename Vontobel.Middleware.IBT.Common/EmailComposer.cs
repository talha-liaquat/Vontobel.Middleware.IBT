using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Vontobel.Middleware.IBT.Common
{
    public class EmailComposer
    {
        public static void SendEmail(string subject, string body, IList<String> recepients)
        {
            var mailMessage = new MailMessage();
            var smtpClient = new SmtpClient();

            mailMessage.From = new MailAddress(SystemConfig.Default["FromAddress"]);
            foreach (var recepient in recepients)
            {
                mailMessage.To.Add(recepient);
            }
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = body;
            smtpClient.Host = SystemConfig.Default["SmtpAddress"];
            smtpClient.Port = int.Parse(SystemConfig.Default["SmtpPort"]);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = new System.Net.NetworkCredential(SystemConfig.Default["FromAddress"], SystemConfig.Default["FromAddressPassword"]);
            smtpClient.Send(mailMessage);
        }
    }
}
