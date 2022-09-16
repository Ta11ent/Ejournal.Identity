using MimeKit;
using MimeKit.Text;
namespace MailService.Email
{
    internal class EmailText : IEmail
    {
        private readonly string subject;
        private readonly string recipient;
        private readonly string msg;
        private readonly MailOptions options;

        public EmailText(string subject, string recipient, string msg, MailOptions options)
        {
            this.subject = subject;
            this.recipient = recipient;
            this.msg = msg;
            this.options = options;
        }
        public MimeMessage Email()
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(options.Sender));
            email.To.Add(MailboxAddress.Parse(recipient));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = msg
            };

            return email;
        }
    }
}
