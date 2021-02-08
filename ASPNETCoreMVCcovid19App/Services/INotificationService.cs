using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ASPNETCoreMVCcovid19App.Services
{

    public interface INotificationService
    {
        Task Notifiy();
    }

    public class NotificationEmailService : INotificationService
    {
        private readonly string _to;
        private readonly string _subject;
        private readonly string _body;
        private readonly IConfiguration _config;

        public NotificationEmailService(string to, string subject, string body, IConfiguration config)
        {
            _to = to;
            _subject = subject;
            _body = body;
            _config = config;
        }

        public async Task Notifiy()
        {
            // send e-mail, tested with gmail, you can configure your own mail server
            //remember set the config credentials and server information in appsettings.json
            var fromAddress = new MailAddress(_config.GetValue<String>("MailSettings:fromAcountSendMail"), _config.GetValue<String>("MailSettings:fromDisplayName"));
            var toAddress = new MailAddress(_to);
            string fromPassword = _config.GetValue<String>("MailSettings:fromPassword").ToString();

            var smtp = new SmtpClient
            {
                Host = _config.GetValue<String>("MailSettings:Host").ToString(),
                Port = Int32.Parse(_config.GetValue<String>("MailSettings:Port").ToString()),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = _subject,
                Body = _body
            })
            {
               smtp.Send(message);
            }
        }
    }

    public class NotificationService
    {
        public async Task Send(List<INotificationService> notifications)
        {
            foreach (var notification in notifications)
            {
                await notification.Notifiy();
            }
        }
    }


}
