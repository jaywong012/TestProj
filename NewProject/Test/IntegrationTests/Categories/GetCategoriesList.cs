using System.Text.Json;
using Application.Features.Categories.Queries;
using Test.IntegrationTests.Configurations;

namespace Test.IntegrationTests.Categories;

public class GetCategoriesList
{
    private InitializeIntegrationTestConfigurationsModel _configurations;

    [SetUp]
    public void Setup()
    {
        _configurations = InitializeIntegrationTestConfigurations.SetupInMemoryDatabase();
        InitializeIntegrationTestConfigurations.ClearCategory(_configurations.Context);
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