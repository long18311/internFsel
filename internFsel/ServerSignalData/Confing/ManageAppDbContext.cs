using Microsoft.EntityFrameworkCore;
using ServerSignalData.Models;
using System.Reflection;

namespace ServerSignalData.Confing
{
    public class ManageAppDbContext : DbContext

    {
        public ManageAppDbContext() { }
        public ManageAppDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=LONG\SQLEXPRESS;Database=SignalData;Trusted_Connection=True; Connection Timeout=2");//Persist Security Info=True;User ID =sa; Password =12345678
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<User> users { get; set; }
        public DbSet<Message> messages { get; set; }
    }
}
