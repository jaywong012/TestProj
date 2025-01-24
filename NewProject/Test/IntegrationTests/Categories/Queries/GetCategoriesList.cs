using System.Text.Json;
using Application.Common;
using Application.Features.Categories.Queries;
using Domain.Entities;
using Test.Configurations.IntegrationTest;

namespace Test.IntegrationTests.Categories.Queries;

public class GetCategoriesList
{
    private InitConfigModel _configurations;

    [SetUp]
    public async Task Setup()
    {
        _configurations = InitConfigs.SetupInMemoryDatabase();
        _configurations.Client = await InitConfigs.GenerateToken(_configurations.Client);
    }

    [TearDown]
    public void Teardown()
    {
        _configurations.Dispose();
    }

    [Test]
    public void GetAllCategories_EmptyList_ReturnEmpty()
    {
        DbContextHelper.ClearEntities<Category>(_configurations.Context);
        var response = _configurations.Client.GetAsync(EndPointConstants.CATEGORY).Result;
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var content = JsonSerializer.Deserialize<List<GetCategoryQueryResponse>>(responseBody);
        Assert.That(content, Is.Empty);
    }

    [Test]
    public void GetAllCategories_Has2Items_Return2Items()
    {
        SeedDatabase.SeedCategories(_configurations.Context);
        var response = _configurations.Client.GetAsync(EndPointConstants.CATEGORY).Result;
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var content = JsonSerializer.Deserialize<List<GetCategoryQueryResponse>>(responseBody);
        Assert.That(content, Is.Not.Null);
        if (content != null) Assert.That(content, Has.Count.EqualTo(2));
    }
}