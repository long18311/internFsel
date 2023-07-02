using Microsoft.EntityFrameworkCore;
using RabitQpProductAPI.Models;

namespace RabitQpProductAPI.Data
{
    public class DbContextClass : DbContext
    {
        protected readonly IConfiguration Configuration;
        public DbContextClass(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Server=LONG\SQLEXPRESS;Database=RabitMQDemo;Trusted_Connection=True; Connection Timeout=2");
        }
        public DbSet<Product> products { get; set; }
    }
}
