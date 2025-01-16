using Domain.Entities;
using Infrastructure.Configurations;

namespace Test.IntegrationTests.Configurations;

public static class SeedDatabase
{
    public static void SeedProducts(NewProjectDbContext context)
    {
        if (context.Products == null) return;
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
        context.Categories.AddRange(new List<Category>
        {
            new() {Id = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43A"), Name = "Water"},
            new() {Id = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43B"), Name = "Food"}
        });
        context.SaveChanges();
    }
}