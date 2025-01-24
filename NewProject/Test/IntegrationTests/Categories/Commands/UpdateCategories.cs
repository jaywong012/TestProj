using System.Net;
using System.Text.Json;
using Application.Common;
using Application.Features.Categories.Queries;
using Application.Utilities;
using Test.Configurations.IntegrationTest;

namespace Test.IntegrationTests.Categories.Commands;

public class UpdateCategories
{
    private InitConfigModel _configurations;
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
        _configurations.Dispose();
    }

    [Test]
    public async Task UpdateCategory_CategoryDoesNotExist_CategoryNotFound()
    {
        var category = await _configurations.Client.GetAsync($"{EndPointConstants.CATEGORY}/{_categoryId}");
        Assert.That(category.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task UpdateProduct_UpdateAllFields_ProductHasBeenUpdated()
    {
        SeedDatabase.SeedCategories(_configurations.Context);

        const string updatedName = "Park";

        var response = await _configurations.Client.GetAsync($"{EndPointConstants.CATEGORY}/{_categoryId}");
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var category = JsonSerializer.Deserialize<GetCategoryQueryResponse>(responseBody);
        if (category != null)
        {
            category.Name = updatedName;
        }

        if (category != null)
        {
            var jsonContent = CustomJsonFormat.SerializeToJsonContent(category);


            var putResponse = await _configurations.Client.PutAsync($"{EndPointConstants.CATEGORY}/{_categoryId}", jsonContent);
            putResponse.EnsureSuccessStatusCode();
        }

        var updatedResponse = await _configurations.Client.GetAsync($"{EndPointConstants.CATEGORY}/{_categoryId}");
        var updatedResponseBody = updatedResponse.Content.ReadAsStringAsync().Result;
        var updatedCategory = JsonSerializer.Deserialize<GetCategoryQueryResponse>(updatedResponseBody);
        Assert.That(updatedCategory, Is.Not.Null);
        Assert.That(updatedCategory?.Name, Is.EqualTo(updatedName));
    }
}