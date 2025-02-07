using Application.Features.Products.Commands;
using Domain.Common.Constants;
using Infrastructure.Utilities;
using Test.Configurations.IntegrationTest;

namespace Test.IntegrationTests.Products.Commands;

public class CreateProduct
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
            Price = 20,
            CategoryId = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43A")
        };
        _configurations.Client = await InitConfigs.GenerateToken(_configurations.Client);
    }

    [TearDown]
    public void TearDown()
    {
        _configurations.Dispose();
    }

    [Test]
    public async Task CreateProduct_InsertAllFields_ProductHasCreated()
    {
        var jsonContent = CustomJsonFormat.SerializeToJsonContent(_request);

        var postResponse = await _configurations.Client.PostAsync(EndPointConstants.PRODUCT, jsonContent);
        postResponse.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task CreateProduct_InsertWithoutCategory_ProductHasBeenInserted()
    {
        _request.CategoryId = Guid.Empty;

        var jsonContent = CustomJsonFormat.SerializeToJsonContent(_request);

        var postResponse = await _configurations.Client.PostAsync(EndPointConstants.PRODUCT, jsonContent);
        postResponse.EnsureSuccessStatusCode();
    }
}