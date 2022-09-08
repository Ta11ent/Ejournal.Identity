using Microsoft.Extensions.DependencyInjection;
using MailService;

namespace Ejournal.Identity.Extensions
{
    public static class MailServiceExtensions
    {
        public static void AddMailService(this IServiceCollection services)
        {
            services.AddTransient<IMailService, MailService.MailService>();
        }
    }
}
