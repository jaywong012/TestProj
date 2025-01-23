using System.Net;
using System.Text;
using System.Text.Json;
using Application.Common;
using Application.Features.Categories.Queries;
using Domain.Entities;
using Test.Configurations.IntegrationTest;

namespace Test.IntegrationTests.Categories.Commands;

public class UpdateCategories
{
    private InitConfigModel _configurations;

    [SetUp]
    public void SetUp()
    {
        _configurations = InitConfigs.SetupInMemoryDatabase();
        DbContextHelper.ClearEntities<Category>(_configurations.Context);
    }

    [TearDown]
    public void TearDown()
    {
        _configurations.Dispose();
    }

    [Test]
    public async Task UpdateCategory_CategoryDoesNotExist_CategoryNotFound()
    {
        var categoryId = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43A");
        var category = await _configurations.Client.GetAsync($"api/category/{categoryId}");
        Assert.That(category.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task UpdateProduct_UpdateAllFields_ProductHasBeenUpdated()
    {
        SeedDatabase.SeedCategories(_configurations.Context);

        const string updatedName = "Park";

        var categoryId = Guid.Parse("A005FC52-5AE6-4400-4752-08DD2FB6F43A");
        var response = await _configurations.Client.GetAsync($"api/category/{categoryId}");
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var category = JsonSerializer.Deserialize<GetCategoryQueryResponse>(responseBody);
        if (category != null)
        {
            category.Name = updatedName;
        }

        var jsonContent = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, Constants.APPLICATION_JSON);


        var putResponse = await _configurations.Client.PutAsync($"api/category/{categoryId}", jsonContent);
        putResponse.EnsureSuccessStatusCode();

        var updatedResponse = await _configurations.Client.GetAsync($"api/category/{categoryId}");
        var updatedResponseBody = updatedResponse.Content.ReadAsStringAsync().Result;
        var updatedCategory = JsonSerializer.Deserialize<GetCategoryQueryResponse>(updatedResponseBody);
        Assert.That(updatedCategory, Is.Not.Null);
        Assert.That(updatedCategory?.Name, Is.EqualTo(updatedName));
    }
}