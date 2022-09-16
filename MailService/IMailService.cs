using MimeKit;
using System.Threading.Tasks;

namespace MailService
{
    public interface IMailService
    {
        Task SendEmailTextAsync(string subject, string recipient, string message);
        Task SendEmailTemplateAsync(string subject, string recipient, string link);

    }
}
