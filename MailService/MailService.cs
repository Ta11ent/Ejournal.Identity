using MailKit.Net.Smtp;
using MailKit.Security;
using MailService.Sender;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace MailService
{
    public class MailService : IMailService
    {
        private readonly MailOptions mailSettings;
        public MailService(IOptions<MailOptions> mailSettings)
        {
            this.mailSettings = mailSettings.Value;
        }

        public async Task SendEmailTextAsync(string subject, string recipient, string message)
        {
            GenericEmail objectEmail = new GenericEmailText(subject, recipient, message)
            {
                Options = mailSettings
            };
            await SendEmail(objectEmail.Object().Email());
        }

        public async Task SendEmailTemplateAsync(string subject, string recipient, string link)
        {
            GenericEmail objectEmail = new GenericEmailTemplate(subject, recipient, link)
            {
                Options = mailSettings
            };
            await SendEmail(objectEmail.Object().Email());
        }

        private async Task SendEmail(MimeMessage email)
        {
            using (var smpt = new SmtpClient())
            {
                await smpt.ConnectAsync(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                await smpt.AuthenticateAsync(mailSettings.UserName, mailSettings.Password);
                await smpt.SendAsync(email);
                await smpt.DisconnectAsync(true);
            }
        }

    }
}
