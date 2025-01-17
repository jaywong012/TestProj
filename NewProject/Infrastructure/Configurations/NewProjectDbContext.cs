using Domain.Base;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations;

public sealed class NewProjectDbContext : DbContext
{
    public NewProjectDbContext(DbContextOptions<NewProjectDbContext> options) : base(options)
    {
        var relationalDatabase = options.Extensions
            .OfType<Microsoft.EntityFrameworkCore.Infrastructure.RelationalOptionsExtension>()
            .FirstOrDefault();

        if (relationalDatabase != null)
        {
            Database.SetCommandTimeout(12000);
        }
    }

    public DbSet<Product>? Products { get; set; }

    public DbSet<Category>? Categories { get; set; }

    public DbSet<Account>? Accounts { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<BaseModel>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.LastSavedTime = DateTime.UtcNow;
                    entry.Entity.LastCreatedTime = DateTime.UtcNow;
                    entry.CurrentValues["IsDeleted"] = false;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastSavedTime = DateTime.UtcNow;
                    break;
                case EntityState.Deleted:
                    entry.Entity.LastSavedTime = DateTime.UtcNow;
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                default:
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}