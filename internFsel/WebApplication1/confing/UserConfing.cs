using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models;

namespace WebApplication1.confing
{
    public class UserConfing : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Username).IsRequired();
            builder.Property(x =>x.Password).IsRequired();
            builder.Property(x => x.Fullname).IsRequired();
        }
    }
}
