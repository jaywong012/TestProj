using System.Net;
using System.Text.Json;
using Application.Features.Products.Queries;
using Test.IntegrationTests.Configurations;

namespace Test.IntegrationTests.Products;

public class GetCategoriesById
{
    private InitializeIntegrationTestConfigurationsModel _configurations;
    private readonly Guid _productId = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43C");

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
    public void GetProductById_EmptyList_ReturnNotFound()
    {
        var response = _configurations.Client.GetAsync($"api/product/{_productId}").Result;
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public void GetProductById_FoundItem_ReturnCorrectItem()
    {
        SeedDatabase.SeedProducts(_configurations.Context);

        var expectedProduct = _configurations.Context.Products?.FirstOrDefault(p => p.Id == _productId);
        Assert.That(expectedProduct, Is.Not.Null);

        var response = _configurations.Client.GetAsync($"api/product/{_productId}").Result;
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var product = JsonSerializer.Deserialize<GetProductQueryResponse>(responseBody);
        Assert.Multiple(() =>
        {
            Assert.That(product, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            if (product == null || expectedProduct == null) return;
            Assert.That(product.Id, Is.EqualTo(expectedProduct.Id));
            Assert.That(product.Name, Is.EqualTo(expectedProduct.Name));
            Assert.That(product.Price, Is.EqualTo(expectedProduct.Price));
        });
    }
}