using DotnetDemo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetDemo.Repository.Mappings
{
    public class UserPasswordMapping : BaseMapping<UserPassword>
    {
        public override void Configure(EntityTypeBuilder<UserPassword> builder)
        {
            builder.Property(x => x.Id)
                .UseIdentityColumn()
                .HasIdentityOptions(startValue: 2);

            builder.Property(up => up.Password).IsRequired();

            builder.HasData(new UserPassword
            {
                Id = 1,
                UserId = 1,
                Password = "AQAAAAIAAYagAAAAEMRXoBWD0dV14KPMfTS7/OHrK/3OqmT2yvV0nvDFenlQOw7X3wLD3DdBkkMP3SGVFw==",
            });

            base.Configure(builder);
        }
    }
}
