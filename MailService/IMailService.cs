using System.Threading.Tasks;

namespace MailService
{
    public interface IMailService
    {
        Task SendEmailAsync(string subject, string recipient, string message);
        Task SendEmailTemplateAsync(string subject, string recipient, string link);
    }
}
