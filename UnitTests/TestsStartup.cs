using FreshdeskContacts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshdeskContacts.Database;
using Microsoft.EntityFrameworkCore;
using FreshdeskContacts.Repositories;
using FreshdeskContacts.Services;

namespace UnitTests
{
    public class TestsStartup
    {
        private readonly ServiceProvider _serviceProvider;

        public TestsStartup()
        {
            var services = new ServiceCollection();

            // Register your services here
            services.AddDbContext<MyDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: "TestDb"));
            services.AddScoped<IGithubUserRepository, GithubUserRepository>();
            services.AddScoped<IGithubUserService, GithubUserService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        public void Dispose()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetService<MyDbContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        public T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}
