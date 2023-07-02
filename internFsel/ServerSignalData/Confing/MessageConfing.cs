using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerSignalData.Models;

namespace OrderAPI.Confing
{
    public class MessageConfing : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.time).IsRequired();
            builder.HasOne<User>(x => x.UserSender).WithMany().HasForeignKey(x => x.UserSenderid);
            
        }
    }
}
