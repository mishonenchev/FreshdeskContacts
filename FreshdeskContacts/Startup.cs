using System.Configuration;
using FreshdeskContacts.Database;
using FreshdeskContacts.Repositories;
using FreshdeskContacts.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FreshdeskContacts.Clients;
using AutoMapper;

namespace FreshdeskContacts
{
    public class Startup
    {
        public ServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseSqlServer(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            });
            services.AddScoped<IGithubUserRepository, GithubUserRepository>();
            services.AddScoped<IGithubUserService, GithubUserService>();
            services.AddAutoMapper(typeof(Startup));

            services.AddTransient<GithubClient>();
            services.AddTransient<FreshdeskClient>(provider =>
            {
                var mapper = provider.GetService<IMapper>();
                Console.Write("Input domain for freshdesk API:  ");
                var domain = Console.ReadLine();
                return new FreshdeskClient(mapper, domain);
            });

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
