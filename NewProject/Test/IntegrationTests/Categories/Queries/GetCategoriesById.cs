using System.Net;
using System.Text.Json;
using Application.Common;
using Application.Features.Categories.Queries;
using Domain.Entities;
using Test.Configurations.IntegrationTest;

namespace Test.IntegrationTests.Categories.Queries;

public class GetCategoriesById
{
    private InitConfigModel? _configurations;
    private readonly Guid _categoryId = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43A");

    [SetUp]
    public async Task SetUp()
    {
        _configurations = InitConfigs.SetupInMemoryDatabase();
        _configurations.Client = await InitConfigs.GenerateToken(_configurations.Client);
    }

    [TearDown]
    public void TearDown()
    {
        _configurations?.Dispose();
    }

    [Test]
    public void GetCategoryById_EmptyList_ReturnNotFound()
    {
        if (_configurations == null) return;
        DbContextHelper.ClearEntities<Category>(_configurations.Context);
        var response = _configurations.Client.GetAsync($"{EndPointConstants.CATEGORY}/{_categoryId}").Result;
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public void GetCategoryById_FoundCategory_ReturnCorrectCategory()
    {
        if (_configurations?.Context.Categories == null) return;
        SeedDatabase.SeedCategories(_configurations.Context);

        var expectedCategory = _configurations.Context.Categories.FirstOrDefault(p => p.Id == _categoryId);
        Assert.That(expectedCategory, Is.Not.Null);

        var response = _configurations.Client.GetAsync($"{EndPointConstants.CATEGORY}/{_categoryId}").Result;
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var category = JsonSerializer.Deserialize<GetCategoryQueryResponse>(responseBody);
        Assert.Multiple(() =>
        {
            Assert.That(category, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(category?.Id, Is.EqualTo(expectedCategory?.Id));
            Assert.That(category?.Name, Is.EqualTo(expectedCategory?.Name));
        });
    }
}