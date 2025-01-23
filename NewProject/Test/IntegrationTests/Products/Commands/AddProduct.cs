using System.Text;
using System.Text.Json;
using Application.Common;
using Application.Features.Products.Commands;
using Domain.Entities;
using Test.Configurations.IntegrationTest;

namespace Test.IntegrationTests.Products.Commands;

public class AddProduct
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
    public async Task UpdateProduct_UpdateAllFields_ProductHasBeenUpdated()
    {
        CreateProductCommandRequest product = new()
        {
            Name = "CoCa",
            Price = 20
        };
        var jsonContent = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, Constants.APPLICATION_JSON);


        var postResponse = await _configurations.Client.PostAsync("api/product", jsonContent);
        postResponse.EnsureSuccessStatusCode();
    }
}