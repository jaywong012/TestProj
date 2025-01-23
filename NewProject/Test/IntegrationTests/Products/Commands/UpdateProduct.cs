using System.Net;
using System.Text;
using System.Text.Json;
using Application.Common;
using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using Domain.Entities;
using Test.Configurations.IntegrationTest;

namespace Test.IntegrationTests.Products.Commands;

public class UpdateProduct
{
    private InitConfigModel _configurations;
    private UpdateProductCommandRequest _request;

    [SetUp]
    public void SetUp()
    {
        _configurations = InitConfigs.SetupInMemoryDatabase();
        DbContextHelper.ClearEntities<Product>(_configurations.Context);
        _request = new UpdateProductCommandRequest(
            "Strong Bow",
            100,
            Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F431"),
            Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43B"));
    }

    [TearDown]
    public void TearDown()
    {
        _configurations.Dispose();
    }

    [Test]
    public async Task UpdateProduct_ProductIsNotExist_ThrowProductNotFound()
    {
        var productId = _request.Id;
        var jsonContent = new StringContent(JsonSerializer.Serialize(_request), Encoding.UTF8, Constants.APPLICATION_JSON);

        var putResponse = await _configurations.Client.PutAsync($"api/product/{productId}", jsonContent);
        Assert.That(putResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task UpdateProduct_EmptyCategory_ProductHasBeenUpdated()
    {
        _request.CategoryId = Guid.Empty;

        var jsonContent = new StringContent(JsonSerializer.Serialize(_request), Encoding.UTF8, Constants.APPLICATION_JSON);

        var putResponse = await _configurations.Client.PutAsync($"api/product/{_request.Id}", jsonContent);
        Assert.That(putResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task UpdateProduct_UpdateAllFields_ProductHasBeenUpdated()
    {
        SeedDatabase.SeedProducts(_configurations.Context);

        var updatedName = _request.Name;
        var updatedPrice = _request.Price;
        var updatedCategoryId = _request.CategoryId;
        var productId = _request.Id;

        var response = await _configurations.Client.GetAsync($"api/product/{productId}");
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var product = JsonSerializer.Deserialize<GetProductQueryResponse>(responseBody);
        if (product != null)
        {
            product.Name = updatedName;
            product.Price = (int)updatedPrice;
            product.CategoryId = updatedCategoryId;
        }

        var jsonContent = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, Constants.APPLICATION_JSON);


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