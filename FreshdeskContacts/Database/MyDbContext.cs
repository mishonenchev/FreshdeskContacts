using System.Configuration;
using FreshdeskContacts.Models;
using Microsoft.EntityFrameworkCore;

namespace FreshdeskContacts.Database
{
    public class MyDbContext : DbContext
    {
        public DbSet<GithubUser> Users { get; set; }

        public MyDbContext()
        {
        }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }
    }
}
