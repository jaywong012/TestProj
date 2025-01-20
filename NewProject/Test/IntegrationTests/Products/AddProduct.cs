using System.Text;
using System.Text.Json;
using Test.IntegrationTests.Configurations;
using Application.Features.Products.Commands;

namespace Test.IntegrationTests.Products;

public class AddProduct
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
        CreateProductCommandRequest product = new()
        {
            Name = "CoCa",
            Price = 20
        };
        var jsonContent = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");


        var postResponse = await _configurations.Client.PostAsync("api/product", jsonContent);
        postResponse.EnsureSuccessStatusCode();
    }
}