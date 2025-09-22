using DotnetDemo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetDemo.Repository.Mappings
{
    public class UserPasswordMapping : BaseMapping<UserPassword>
    {
        public override void Configure(EntityTypeBuilder<UserPassword> builder)
        {
            builder.Property(up => up.Password).IsRequired();

            builder
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
