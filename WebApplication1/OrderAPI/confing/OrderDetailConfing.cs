using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderAPI.Models;

namespace OrderAPI.confing
{
    public class OrderDetailConfing : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ProductName).IsRequired();
            builder.Property(x => x.UnitPrice).IsRequired();
            builder.Property(x => x.Quantity).IsRequired();
            builder.HasOne<Order>(x => x.Order).WithMany(g => g.OrderDetails).HasForeignKey(x => x.OrderId);
        }
    }
}
