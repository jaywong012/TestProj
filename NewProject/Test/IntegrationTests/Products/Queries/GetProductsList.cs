using System.Text.Json;
using Application.Common;
using Application.Features.Products.Queries;
using Domain.Entities;
using Test.Configurations.IntegrationTest;

namespace Test.IntegrationTests.Products.Queries;

public class GetProductsList
{
    private InitConfigModel _configurations;
    private GetProductListQuery _query;

    [SetUp]
    public async Task Setup()
    {
        _configurations = InitConfigs.SetupInMemoryDatabase();
        _query = new GetProductListQuery
        {
            SearchKey = "CoCa"
        };
        _configurations.Client = await InitConfigs.GenerateToken(_configurations.Client);
    }

    [TearDown]
    public void Teardown()
    {
        _configurations.Dispose();
    }

    [Test]
    public void GetAllProducts_EmptyList_ReturnEmpty()
    {
        DbContextHelper.ClearEntities<Product>(_configurations.Context);
        var response = _configurations.Client.GetAsync(EndPointConstants.PRODUCT).Result;
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var content = JsonSerializer.Deserialize<List<Product>>(responseBody);
        Assert.That(content, Is.Empty);
    }

    [Test]
    public void GetAllProducts_HaveSearchKey_Return1Items()
    {
        SeedDatabase.SeedProducts(_configurations.Context);
        var response = _configurations.Client.GetAsync($"{EndPointConstants.PRODUCT}?searchKey={_query.SearchKey}").Result;
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var content = JsonSerializer.Deserialize<List<GetProductQueryResponse>>(responseBody);
        Assert.That(content, Is.Not.Null);
        if (content != null)
        {
            Assert.That(content, Has.Count.EqualTo(1));
        }
    }

    [Test]
    public void GetAllProducts_Has3Items_Return3Items()
    {
        SeedDatabase.SeedProducts(_configurations.Context);
        var response = _configurations.Client.GetAsync(EndPointConstants.PRODUCT).Result;
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var content = JsonSerializer.Deserialize<List<GetProductQueryResponse>>(responseBody);
        Assert.That(content, Is.Not.Null);
        if (content != null)
        {
            Assert.That(content, Has.Count.EqualTo(3));
        }
    }
}
