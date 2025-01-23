using System.Net;
using Application.Features.Categories.Commands;
using Domain.Entities;
using Test.Configurations.IntegrationTest;

namespace Test.IntegrationTests.Categories.Commands;

public class DeleteCategoryById
{
    private InitConfigModel? _configurations;
    private DeleteCategoryCommandRequest _request;

    [SetUp]
    public void SetUp()
    {
        _configurations = InitConfigs.SetupInMemoryDatabase();
        DbContextHelper.ClearEntities<Category>(_configurations.Context);
        _request = new DeleteCategoryCommandRequest
        {
            Id = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43A")
        };
    }

    [TearDown]
    public void TearDown()
    {
        _configurations?.Dispose();
    }

    [Test]
    public async Task DeleteCategory_CategoryDoesNotExist_CategoryNotFound()
    {
        if (_configurations == null) return;
        var categoryId = _request.Id;
        var category = await _configurations.Client.GetAsync($"api/category/{categoryId}");
        Assert.That(category.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task DeleteCategory_HasDeleteCategory_CategoryGotDeleted()
    {
        if (_configurations == null) return;
        var categoryId = _request.Id;
        SeedDatabase.SeedCategories(_configurations.Context);
        var category = await _configurations.Client.GetAsync($"api/category/{categoryId}");
        Assert.That(category, Is.Not.Null);

        await _configurations.Client.DeleteAsync($"api/category/{categoryId}");
        var deletedCategory = await _configurations.Client.GetAsync($"api/category/{categoryId}");
        Assert.That(deletedCategory.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}