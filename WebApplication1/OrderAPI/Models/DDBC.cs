using Microsoft.EntityFrameworkCore;
using OrderAPI.confing;
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
            optionsBuilder.UseSqlServer(@"Data Source=LONG\SQLEXPRESS;Initial Catalog=Orderdata;Integrated Security=SSPI; Persist Security Info=True;User ID =long; Password =123456");//Persist Security Info=True;User ID =long; Password =123456
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.ApplyConfiguration(new OrderConfing());
            modelBuilder.ApplyConfiguration(new OrderDetailConfing());
        }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderDetail> orderDetails { get; set; }
    }
}
