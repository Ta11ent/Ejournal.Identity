using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.IO;
using System.Threading.Tasks;

namespace MailService
{
    public class MailService : IMailService
    {
        private readonly MailOptions _mailSettings;
        public MailService(IOptions<MailOptions> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public virtual async Task SendEmailAsync(string subject, string recipient, string message)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_mailSettings.Sender));
            email.To.Add(MailboxAddress.Parse(recipient));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = message
            };

            await SendEmail(email);
        }
        public virtual async Task SendEmailTemplateAsync(string subject, string recipient, string link)
        {
            StreamReader str = new StreamReader(_mailSettings.TemplatePath);
            string MailText = str.ReadToEnd();
            str.Close();

            MailText = MailText.Replace("[ConfirmationLink]", link).Replace("[Subject]", subject);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Sender);
            email.To.Add(MailboxAddress.Parse(recipient));
            email.Subject = subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();

            await SendEmail(email);
        }

        private async Task SendEmail(MimeMessage email)
        {
            using (var smpt = new SmtpClient())
            {
                await smpt.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                await smpt.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
                await smpt.SendAsync(email);
                await smpt.DisconnectAsync(true);
            }
        }
    }
}
