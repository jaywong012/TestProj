using Application.Common;
using Domain.Entities;
using Infrastructure.Configurations;

namespace Test.Configurations.IntegrationTest;

public static class SeedDatabase
{
    public static void SeedProducts(NewProjectDbContext context)
    {
        if (context.Products == null) return;
        DbContextHelper.ClearEntities<Product>(context);
        context.Products.AddRange(new List<Product>
        {
            new() {Id = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43A"), Name = "CoCa", Price = 20, LastSavedTime = new DateTime()},
            new() {Id = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43B"), Name = "Pepsi", Price = 50, LastSavedTime = new DateTime()},
            new() {Id = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43C"), Name = "Latte", Price = 120, LastSavedTime = new DateTime()}
        });
        context.SaveChanges();
    }
    public static void SeedCategories(NewProjectDbContext context)
    {
        if (context.Categories == null) return;
        DbContextHelper.ClearEntities<Category>(context);
        context.Categories.AddRange(new List<Category>
        {
            new() {Id = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43A"), Name = "Water"},
            new() {Id = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43B"), Name = "Food"}
        });
        context.SaveChanges();
    }
    public static void SeedAccounts(NewProjectDbContext context)
    {
        if (context.Accounts == null) return;
        DbContextHelper.ClearEntities<Account>(context);
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("123");
        context.Accounts.AddRange(new List<Account>
        {
            new() {Id = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43A"), UserName = "jac", Hash = hashedPassword, Role = Constants.ADMIN}
        });
        context.SaveChanges();
    }
}