using Domain.Entities;

namespace Test.Configurations.UnitTest;

public static class MockData
{
    public static List<Product> MockProducts()
    {
        return
        [
            new Product
            {
                Id = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43A"), Name = "CoCa", Price = 20, IsDeleted = false
            },
            new Product
            {
                Id = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43B"), Name = "Pepsi", Price = 50, IsDeleted = false
            },
            new Product
            {
                Id = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43C"), Name = "Latte", Price = 120, IsDeleted = false
            }
        ];
    }
    public static List<Category> MockCategories()
    {
        return
        [
            new Category { Id = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43A"), Name = "Food", IsDeleted = false },
            new Category { Id = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43B"), Name = "Drink", IsDeleted = false },
            new Category { Id = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43C"), Name = "Car", IsDeleted = false }
        ];
    }
}
