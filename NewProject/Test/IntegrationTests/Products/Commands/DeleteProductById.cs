using Domain.Entities;
using System.Net;
using Application.Common;
using Test.Configurations.IntegrationTest;

namespace Test.IntegrationTests.Products.Commands;

public class DeleteProductById
{
    private InitConfigModel _configurations;
    private readonly Guid _productId = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43C");

    [SetUp]
    public async Task SetUp()
    {
        _configurations = InitConfigs.SetupInMemoryDatabase();
        DbContextHelper.ClearEntities<Product>(_configurations.Context);
        _configurations.Client = await InitConfigs.GenerateToken(_configurations.Client);
    }

    [TearDown]
    public void TearDown()
    {
        _configurations.Dispose();
    }

    [Test]
    public async Task DeleteRecord_HasDeletedItem_ItemGotDeleted()
    {
        SeedDatabase.SeedProducts(_configurations.Context);
        var product = await _configurations.Client.GetAsync($"{EndPointConstants.PRODUCT}/{_productId}");
        Assert.That(product.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        await _configurations.Client.DeleteAsync($"{EndPointConstants.PRODUCT}/{_productId}");
        var deletedProduct = await _configurations.Client.GetAsync($"{EndPointConstants.PRODUCT}/{_productId}");
        Assert.That(deletedProduct.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}