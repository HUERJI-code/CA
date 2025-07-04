using CA.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CA.Services
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
            // provides database connection-string
            "server=localhost;user=root;password=;database=AndroidCA;",
            new MySqlServerVersion(new Version(8, 0, 39))
            );
            optionsBuilder.UseLazyLoadingProxies();
        }
        // our database tables
        public DbSet<User> User { get; set; }
        public DbSet<Score> Score { get; set; }
    }
}
