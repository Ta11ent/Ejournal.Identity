using MailService.Email;

namespace MailService.Sender
{
    internal class GenericEmailTemplate : GenericEmail
    {
        private readonly string subject;
        private readonly string recipient;
        private readonly string link;
        internal MailOptions Options { get; set; }
        public GenericEmailTemplate(string subject, string recipient, string link)
        {
            this.subject = subject;
            this.recipient = recipient;
            this.link = link;
        }
        public override IEmail Object() => new EmailTemplate(subject, recipient, link, Options);
    }
}
