using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using RingoMediaReminder.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RingoMediaReminder.Services
{
    public class MailingServices : IMailingServices
    {
        private readonly MailSettings _mailSettings;

        public MailingServices(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(IList<string> mailTos, string subject, string body)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Email),
                Subject = subject
            };

            foreach (var mailTo in mailTos)
            {
                email.To.Add(MailboxAddress.Parse(mailTo));
            }

            var builder = new BodyBuilder
            {
                HtmlBody = body
            };
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));

            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(_mailSettings.Host, (int)_mailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSettings.Email, _mailSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., logging)
                throw new InvalidOperationException("Failed to send email", ex);
            }
            finally
            {
                // Ensure the SMTP client is properly disposed
                await smtp.DisconnectAsync(true);
                smtp.Dispose();
            }
        }
    }
}
