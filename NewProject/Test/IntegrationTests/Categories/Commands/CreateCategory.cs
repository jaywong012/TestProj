using Application.Common;
using Application.Features.Categories.Commands;
using Application.Utilities;
using Domain.Entities;
using Test.Configurations.IntegrationTest;

namespace Test.IntegrationTests.Categories.Commands;

public class CreateCategory
{
    private InitConfigModel _configurations;
    private CreateCategoryCommandRequest _commandRequest;

    [SetUp]
    public async Task SetUp()
    {
        _configurations = InitConfigs.SetupInMemoryDatabase();
        _commandRequest = new CreateCategoryCommandRequest("Name");
        _configurations.Client = await InitConfigs.GenerateToken(_configurations.Client);
    }

    [TearDown]
    public void TearDown()
    {
        _configurations.Dispose();
    }

    [Test]
    public async Task CreateCategory_HaveExactParam_CreateCategorySuccessful()
    {
        Category category = new()
        {
            Name = _commandRequest.Name
        };

        var jsonContent = CustomJsonFormat.SerializeToJsonContent(category);

        var response = await _configurations.Client.PostAsync(EndPointConstants.CATEGORY, jsonContent);

        Assert.That(response.IsSuccessStatusCode);
    }
}