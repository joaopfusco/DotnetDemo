using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using DotnetDemo.Domain.Models;
using DotnetDemo.Repository.Mappings;

namespace DotnetDemo.Repository.Data
{
    public partial class AppDbContext : DbContext
    {
        private readonly string _connectionString;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            _connectionString = GetConnectionString(configuration);
        }

        private string GetConnectionString(IConfiguration configuration)
        {
            var envConnection = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION");
            var appsettingsConnection = configuration.GetConnectionString("DefaultConnection");

            if (!string.IsNullOrWhiteSpace(envConnection))
                return envConnection;

            if (!string.IsNullOrWhiteSpace(appsettingsConnection))
                return appsettingsConnection;

            throw new Exception("Não há ConnectionString.");
        }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMapping());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }

        public override int SaveChanges()
        {
            BeforeSaveChanges();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            BeforeSaveChanges();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void BeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            var entries = ChangeTracker.Entries<BaseModel>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            var now = DateTime.Now;
            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = now;
                }
                else
                {
                    entry.Property(nameof(BaseModel.CreatedAt)).IsModified = false;
                }
                entity.UpdatedAt = now;
            }
        }
    }
}
