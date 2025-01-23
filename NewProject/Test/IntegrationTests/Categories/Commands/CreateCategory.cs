using System.Text;
using System.Text.Json;
using Application.Common;
using Application.Features.Categories.Commands;
using Domain.Entities;
using Test.Configurations.IntegrationTest;

namespace Test.IntegrationTests.Categories.Commands;

public class CreateCategory
{
    private InitConfigModel _configuration;
    private CreateCategoryCommandRequest _commandRequest;

    [SetUp]
    public void SetUp()
    {
        _configuration = InitConfigs.SetupInMemoryDatabase();
        _commandRequest = new CreateCategoryCommandRequest("Name");
    }

    [TearDown]
    public void TearDown()
    {
        _configuration.Dispose();
    }

    [Test]
    public async Task CreateCategory_HaveExactParam_CreateCategorySuccessful()
    {
        Category category = new()
        {
            Name = _commandRequest.Name,
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, Constants.APPLICATION_JSON);

        var response = await _configuration.Client.PostAsync("api/category", jsonContent);

        Assert.That(response.IsSuccessStatusCode);
    }
}