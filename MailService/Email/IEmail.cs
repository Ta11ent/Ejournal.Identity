using MimeKit;

namespace MailService
{
    internal interface IEmail
    {
        MimeMessage Email();
    }
}
