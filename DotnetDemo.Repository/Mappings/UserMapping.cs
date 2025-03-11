using DotnetDemo.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Cryptography;
using System.Text;

namespace DotnetDemo.Repository.Mappings
{
    public class UserMapping : BaseMapping<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Id)
                .UseIdentityColumn()
                .HasIdentityOptions(startValue: 2);

            builder.HasIndex(x => new { x.Username }).IsUnique();

            builder.HasData(new User
            {
                Id = 1,
                Username = "root",
                Password = "AQAAAAIAAYagAAAAEP8PG6clj/SvgE7ELzmTICj861gpD8wEbPAjmyep0KrHAqGyy9rqn+UrVlFlci1DAQ==",
            });

            base.Configure(builder);
        }
    }
}
