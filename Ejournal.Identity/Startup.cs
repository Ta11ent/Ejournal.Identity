using Ejournal.Identity.Data;
using Ejournal.Identity.Extensions;
using Ejournal.Identity.Models;
using MailService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace Ejournal.Identity
{
    public class Startup
    {
        public IConfiguration AppConfiguration { get; }
        public Startup(IConfiguration configuration) =>
            AppConfiguration = configuration;
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = AppConfiguration.GetValue<string>("DbConnection");
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddIdentity<AppUser, AppRole>(config =>
            {
                config.Password.RequiredLength = 8;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddAspNetIdentity<AppUser>()
                .AddInMemoryApiResources(Configuration.ApiResources)
                .AddInMemoryIdentityResources(Configuration.IdentityResources)
                .AddInMemoryApiScopes(Configuration.ApiScopes)
                .AddInMemoryClients(Configuration.Clients)
                .AddDeveloperSigningCredential();


            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "eJournal.Identity.Cookie";
                config.LoginPath = "/Auth/Login";
                config.LogoutPath = "/Auth/Logout";
            });

            services.AddControllersWithViews();

            services.Configure<MailOptions>(AppConfiguration.GetSection("MailSettings"));
            services.AddMailService();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "Styles")),
                RequestPath = "/styles"
            });

            app.UseRouting();
            app.UseIdentityServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
