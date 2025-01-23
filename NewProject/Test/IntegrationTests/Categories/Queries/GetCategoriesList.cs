using System.Text.Json;
using Application.Features.Categories.Queries;
using Domain.Entities;
using Test.Configurations.IntegrationTest;

namespace Test.IntegrationTests.Categories.Queries;

public class GetCategoriesList
{
    private InitConfigModel _configurations;

    [SetUp]
    public void Setup()
    {
        _configurations = InitConfigs.SetupInMemoryDatabase();
        DbContextHelper.ClearEntities<Category>(_configurations.Context);
    }

    [TearDown]
    public void Teardown()
    {
        _configurations.Dispose();
    }

    [Test]
    public void GetAllCategories_EmptyList_ReturnEmpty()
    {
        var response = _configurations.Client.GetAsync("api/category").Result;
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var content = JsonSerializer.Deserialize<List<GetCategoryQueryResponse>>(responseBody);
        Assert.That(content, Is.Empty);
    }

    [Test]
    public void GetAllCategories_Has2Items_Return2Items()
    {
        SeedDatabase.SeedCategories(_configurations.Context);
        var response = _configurations.Client.GetAsync("api/category").Result;
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var content = JsonSerializer.Deserialize<List<GetCategoryQueryResponse>>(responseBody);
        Assert.That(content, Is.Not.Null);
        if (content != null) Assert.That(content, Has.Count.EqualTo(2));
    }
}