using DotnetDemo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetDemo.Repository.Mappings
{
    public class BaseMapping<TModel> : IEntityTypeConfiguration<TModel> where TModel : BaseModel
    {
        public virtual void Configure(EntityTypeBuilder<TModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .UseIdentityColumn();

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(x => x.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
