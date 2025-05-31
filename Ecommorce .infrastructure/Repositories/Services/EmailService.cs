using Ecommorce.Core.DTO;
using Ecommorce.Core.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommorce_.infrastructure.Repositories.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmail(EmailDTO emailDTO)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("My Ecom", _config["EmailSetting:From"]));
            message.To.Add(new MailboxAddress(emailDTO.To, emailDTO.To));
            message.Subject = emailDTO.Subject;
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailDTO.Content
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                await smtp.ConnectAsync(_config["EmailSetting:Smtp"], int.Parse(_config["EmailSetting:Port"]), true);
                await smtp.AuthenticateAsync(_config["EmailSetting:UserName"], _config["EmailSetting:Password"]);
                await smtp.SendAsync(message);  // <-- Send the email here
            }
            catch (Exception ex)
            {
                // Consider logging the error here
                throw new InvalidOperationException("Failed to send email.", ex);
            }
            finally
            {
                if (smtp.IsConnected)
                    await smtp.DisconnectAsync(true);
            }
        }

    }
}
