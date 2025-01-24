using System.Net;
using System.Text.Json;
using Application.Common;
using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using Application.Utilities;
using Domain.Entities;
using Test.Configurations.IntegrationTest;

namespace Test.IntegrationTests.Products.Commands;

public class UpdateProduct
{
    private InitConfigModel _configurations;
    private UpdateProductCommandRequest _request;

    [SetUp]
    public async Task SetUp()
    {
        _configurations = InitConfigs.SetupInMemoryDatabase();
        _request = new UpdateProductCommandRequest(
            "Strong Bow",
            100,
            Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F431"),
            Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43B"));
        _configurations.Client = await InitConfigs.GenerateToken(_configurations.Client);
    }

    [TearDown]
    public void TearDown()
    {
        _configurations.Dispose();
    }

    [Test]
    public async Task UpdateProduct_ProductIsNotExist_ThrowProductNotFound()
    {
        DbContextHelper.ClearEntities<Product>(_configurations.Context);
        var productId = _request.Id;
        var jsonContent = CustomJsonFormat.SerializeToJsonContent(_request);

        var putResponse = await _configurations.Client.PutAsync($"{EndPointConstants.PRODUCT}/{productId}", jsonContent);
        Assert.That(putResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task UpdateProduct_EmptyCategory_ProductHasBeenUpdated()
    {
        DbContextHelper.ClearEntities<Product>(_configurations.Context);
        _request.CategoryId = Guid.Empty;

        var jsonContent = CustomJsonFormat.SerializeToJsonContent(_request);

        var putResponse = await _configurations.Client.PutAsync($"{EndPointConstants.PRODUCT}/{_request.Id}", jsonContent);
        Assert.That(putResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task UpdateProduct_UpdateAllFields_ProductHasBeenUpdated()
    {
        SeedDatabase.SeedProducts(_configurations.Context);

        var response = await _configurations.Client.GetAsync($"{EndPointConstants.PRODUCT}/{_request.Id}");
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var product = JsonSerializer.Deserialize<GetProductQueryResponse>(responseBody);
        if (product != null)
        {
            product.Name = _request.Name;
            product.Price = (int)_request.Price;
            product.CategoryId = _request.CategoryId;
        }

        if (product != null)
        {
            var jsonContent = CustomJsonFormat.SerializeToJsonContent(product);

            var putResponse = await _configurations.Client.PutAsync($"{EndPointConstants.PRODUCT}/{_request.Id}", jsonContent);
            putResponse.EnsureSuccessStatusCode();
        }

        var updatedResponse = await _configurations.Client.GetAsync($"{EndPointConstants.PRODUCT}/{_request.Id}");
        var updatedResponseBody = updatedResponse.Content.ReadAsStringAsync().Result;
        var updatedProduct = JsonSerializer.Deserialize<GetProductQueryResponse>(updatedResponseBody);
        Assert.That(updatedProduct, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(updatedProduct?.Name, Is.EqualTo(_request.Name));
            Assert.That(updatedProduct?.Price, Is.EqualTo(_request.Price));
            Assert.That(updatedProduct?.CategoryId, Is.EqualTo(_request.CategoryId));
        });

    }
}