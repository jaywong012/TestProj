using System.Text;
using System.Text.Json;
using Application.Features.Products.Queries;
using Test.IntegrationTests.Configurations;

namespace Test.IntegrationTests.Products;

public class UpdateProduct
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
    public async Task UpdateProduct_UpdateAllFields_ProductHasBeenUpdated()
    {
        SeedDatabase.SeedProducts(_configurations.Context);

        const string updatedName = "Strong Bow";
        const decimal updatedPrice = 100;
        var updatedCategoryId = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F431");

        var productId = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43A");
        var response = await _configurations.Client.GetAsync($"api/product/{productId}");
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var product = JsonSerializer.Deserialize<GetProductQueryResponse>(responseBody);
        if (product != null)
        {
            product.Name = updatedName;
            product.Price = (int)updatedPrice;
            product.CategoryId = updatedCategoryId;
        }

        var jsonContent = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");


        var putResponse = await _configurations.Client.PutAsync($"api/product/{productId}", jsonContent);
        putResponse.EnsureSuccessStatusCode();

        var updatedResponse = await _configurations.Client.GetAsync($"api/product/{productId}");
        var updatedResponseBody = updatedResponse.Content.ReadAsStringAsync().Result;
        var updatedProduct = JsonSerializer.Deserialize<GetProductQueryResponse>(updatedResponseBody);
        Assert.That(updatedProduct, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(updatedProduct?.Name, Is.EqualTo(updatedName));
            Assert.That(updatedProduct?.Price, Is.EqualTo(updatedPrice));
            Assert.That(updatedProduct?.CategoryId, Is.EqualTo(updatedCategoryId));
        });

    }
}