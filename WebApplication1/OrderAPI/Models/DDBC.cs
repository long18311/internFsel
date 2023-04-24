using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace OrderAPI.Models
{
    public class DDBC : DbContext
    {
        public DDBC() { }
        public DDBC(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=LONG\SQLEXPRESS;Initial Catalog=Orderdata; Integrated Security = True ");//Persist Security Info=True;User ID =sa; Password =12345678
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            ;
        }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderDetail> orderDetails { get; set; }
    }
}
