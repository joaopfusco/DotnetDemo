using DotnetDemo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetDemo.Repository.Mappings
{
    public class RefreshTokenMapping : BaseMapping<RefreshToken>
    {
        public override void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.Property(x => x.Token).IsRequired();
            builder.HasIndex(x => x.Token).IsUnique();

            builder
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            base.Configure(builder);
        }
    }
}
