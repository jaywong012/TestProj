using Domain.Entities;
using System.Net;
using Test.Configurations.IntegrationTest;

namespace Test.IntegrationTests.Products.Commands;

public class DeleteProductById
{
    private InitConfigModel _configurations;

    [SetUp]
    public void SetUp()
    {
        _configurations = InitConfigs.SetupInMemoryDatabase();
        DbContextHelper.ClearEntities<Product>(_configurations.Context);
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