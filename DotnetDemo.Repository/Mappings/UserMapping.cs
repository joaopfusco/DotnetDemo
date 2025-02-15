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

            User user = new()
            {
                Username = "root",
                Password = "root"
            };

            PasswordHasher<User> _passwordHasher = new();
            string password = _passwordHasher.HashPassword(user, user.Password);

            builder.HasData(new User
            {
                Id = 1,
                Username = user.Username,
                Password = password,
            });

            base.Configure(builder);
        }
    }
}
