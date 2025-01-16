using System.Text.Json;
using Application.Features.Products.Queries;
using Domain.Entities;
using Test.IntegrationTests.Configurations;

namespace Test.IntegrationTests.Products;

public class GetProductsList
{
    private InitializeIntegrationTestConfigurationsModel _configurations;

    [SetUp]
    public void Setup()
    {
        _configurations = InitializeIntegrationTestConfigurations.SetupInMemoryDatabase();
        InitializeIntegrationTestConfigurations.ClearProduct(_configurations.Context);
    }

    [TearDown]
    public void Teardown()
    {
        _configurations.Dispose();
    }

    [Test]
    public void GetAllProducts_EmptyList_ReturnEmpty()
    {
        var response = _configurations.Client.GetAsync("api/product").Result;
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var content = JsonSerializer.Deserialize<List<Product>>(responseBody);
        Assert.That(content, Is.Empty);
    }

    [Test]
    public void GetAllProducts_Has3Items_Return3Items()
    {
        SeedDatabase.SeedProducts(_configurations.Context);
        var response = _configurations.Client.GetAsync("api/product").Result;
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var content = JsonSerializer.Deserialize<List<Product>>(responseBody);
        Assert.That(content, Is.Not.Null);
        if (content != null)
        {
            Assert.That(content, Has.Count.EqualTo(3));
        }
    }

    [Test]
    public void GetProductListByPaging_NotEmptyItems_ReturnExactPageAndItems()
    {
        SeedDatabase.SeedProducts(_configurations.Context);
        GetProductListByPagingQuery pageRequest = new()
        {
            PageIndex = 1,
            PageSize = 2
        };
        var response = _configurations.Client.GetAsync($"api/Product/paged?pageIndex={pageRequest.PageIndex}&pageSize={pageRequest.PageSize}").Result;
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var content = JsonSerializer.Deserialize<PagedProductListResponse>(responseBody);
        Assert.That(content, Is.Not.Null);
        if (content == null) return;
        Assert.Multiple(() =>
        {
            Assert.That(content.Products, Is.Not.Empty);
            Assert.That(content.TotalPages, Is.EqualTo(2));
            Assert.That(content.Products.Count(), Is.EqualTo(1));
        });
    }
}
