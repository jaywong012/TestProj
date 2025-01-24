using Application.Common;
using Application.Features.Products.Commands;
using Test.Configurations.IntegrationTest;

namespace Test.IntegrationTests.Products.Commands;

public class AddProduct
{
    private InitConfigModel _configurations;
    private CreateProductCommandRequest _request;

    [SetUp]
    public async Task SetUp()
    {
        _configurations = InitConfigs.SetupInMemoryDatabase();
        _request = new CreateProductCommandRequest
        {
            Name = "CoCa",
            Price = 20
        };
        _configurations.Client = await InitConfigs.GenerateToken(_configurations.Client);
    }

    [TearDown]
    public void TearDown()
    {
        _configurations.Dispose();
    }

    [Test]
    public async Task UpdateProduct_UpdateAllFields_ProductHasBeenUpdated()
    {
        var jsonContent = Utilities.SerializeToJsonContent(_request);

        var postResponse = await _configurations.Client.PostAsync(EndPointConstants.PRODUCT, jsonContent);
        postResponse.EnsureSuccessStatusCode();
    }
}