using System.Net;
using Test.IntegrationTests.Configurations;

namespace Test.IntegrationTests.Products;

public class DeleteProductById
{
    private InitializeIntegrationTestConfigurationsModel _configurations;

    [SetUp]
    public void SetUp()
    {
        _configurations = InitializeIntegrationTestConfigurations.SetupInMemoryDatabase();
        InitializeIntegrationTestConfigurations.ClearProduct(_configurations.Context);
    }

    [TearDown]
    public void TearDown()
    {
        _configurations.Dispose();
    }

    [Test]
    public async Task DeleteRecord_HasDeletedItem_ItemGotDeleted()
    {
        var productId = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43C");
        SeedDatabase.SeedProducts(_configurations.Context);
        var product = await _configurations.Client.GetAsync($"api/Product/{productId}");
        Assert.That(product.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        await _configurations.Client.DeleteAsync($"api/Product/{productId}");
        var deletedProduct = await _configurations.Client.GetAsync($"api/Product/{productId}");
        Assert.That(deletedProduct.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}