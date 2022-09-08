namespace MailService
{
    public class MailOptions
    {
        public string Sender { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TemplatePath { get; set; } 
            = @"..\MailService\Templates\email.html";
    }
}
