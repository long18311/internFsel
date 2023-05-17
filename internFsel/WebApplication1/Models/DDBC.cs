using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace WebApplication1.Models
{
    public class DDBC : DbContext
    {
        public DDBC() { }
        public DDBC(DbContextOptions options) : base(options) 
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=LONG\SQLEXPRESS;Initial Catalog=InternEl; Integrated Security = True; Connection Timeout=30");//Persist Security Info=True;User ID =sa; Password =12345678
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<User> users { get; set; }
        public DbSet<Customer> customers { get; set; }


    }
}
