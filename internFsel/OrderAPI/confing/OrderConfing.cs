using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderAPI.Models;

namespace OrderAPI.confing
{
    public class OrderConfing : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.OrderDate).IsRequired();
            builder.Property(x => x.TotalPrice).IsRequired();
            builder.HasMany<OrderDetail>(x => x.OrderDetails).WithOne(s => s.Order).HasForeignKey(x => x.OrderId);
        }
    }
}
