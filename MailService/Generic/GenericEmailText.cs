using MailService.Email;

namespace MailService.Sender
{
    internal class GenericEmailText : GenericEmail
    {
        private readonly string subject;
        private readonly string recipient;
        private readonly string msg;
        internal  MailOptions Options { get; set; }
        public GenericEmailText(string subject, string recipient, string msg)
        {
            this.subject = subject;
            this.recipient = recipient;
            this.msg = msg;
        }
        public override IEmail Object() => new EmailText(subject, recipient, msg, Options);
    }
}
