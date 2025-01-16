using System.Net;
using Test.IntegrationTests.Configurations;

namespace Test.IntegrationTests.Categories;

public class DeleteCategoryById
{
    private InitializeIntegrationTestConfigurationsModel? _configurations;

    [SetUp]
    public void SetUp()
    {
        _configurations = InitializeIntegrationTestConfigurations.SetupInMemoryDatabase();
        InitializeIntegrationTestConfigurations.ClearCategory(_configurations.Context);
    }

    [TearDown]
    public void TearDown()
    {
        _configurations?.Dispose();
    }

    [Test]
    public async Task DeleteCategory_HasDeleteCategory_CategoryGotDeleted()
    {
        if (_configurations == null) return;
        var categoryId = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43A");
        SeedDatabase.SeedCategories(_configurations.Context);
        var category = await _configurations.Client.GetAsync($"api/category/{categoryId}");
        Assert.That(category, Is.Not.Null);

        await _configurations.Client.DeleteAsync($"api/category/{categoryId}");
        var deletedCategory = await _configurations.Client.GetAsync($"api/category/{categoryId}");
        Assert.That(deletedCategory.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}