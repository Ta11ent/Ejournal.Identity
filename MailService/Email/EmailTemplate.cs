using MimeKit;
using System.IO;

namespace MailService.Email
{
    internal class EmailTemplate : IEmail
    {
        private readonly string subject;
        private readonly string recipient;
        private readonly string link;
        private readonly MailOptions options;
        public EmailTemplate(string subject, string recipient, string link, MailOptions options)
        {
            this.subject = subject;
            this.recipient = recipient;
            this.link = link;
            this.options = options;
        }

        public MimeMessage Email()
        {
            StreamReader str = new StreamReader(options.TemplatePath);
            string MailText = str.ReadToEnd();
            str.Close();

            MailText = MailText.Replace("[ConfirmationLink]", link).Replace("[Subject]", subject);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(options.Sender);
            email.To.Add(MailboxAddress.Parse(recipient));
            email.Subject = subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();

            return email;
        }
    }
}
