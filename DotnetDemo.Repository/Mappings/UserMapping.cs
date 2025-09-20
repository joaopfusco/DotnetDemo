using DotnetDemo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetDemo.Repository.Mappings
{
    public class UserMapping : BaseMapping<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Username).IsRequired();
            builder.Property(x => x.Email).IsRequired();

            builder.HasIndex(x => new { x.Username }).IsUnique();
            builder.HasIndex(x => new { x.Email }).IsUnique();

            base.Configure(builder);
        }
    }
}
