using Application.Features.Products.Queries;
using System.Text.Json;
using Application.Common;
using Test.Configurations.IntegrationTest;

namespace Test.IntegrationTests.Products.Queries;

public class GetProductListByPaging
{
    private InitConfigModel _configurations;
    private GetProductListByPagingQuery _request;

    [SetUp]
    public async Task Setup()
    {
        _configurations = InitConfigs.SetupInMemoryDatabase();
        _request = new GetProductListByPagingQuery
        {
            PageIndex = 0,
            PageSize = 2,
            SearchKey = "Co"
        };
        SeedDatabase.SeedProducts(_configurations.Context);
        _configurations.Client = await InitConfigs.GenerateToken(_configurations.Client);
    }

    [TearDown]
    public void Teardown()
    {
        _configurations.Dispose();
    }

    [Test]
    public void GetProductListByPaging_NotEmptyItems_ReturnExactPageAndItems()
    {
        const string expectedKeyword = "CoCa";

        var response = _configurations.Client.GetAsync($"{EndPointConstants.PRODUCT_PAGED}?searchKey={_request.SearchKey}&pageIndex={_request.PageIndex}&pageSize={_request.PageSize}").Result;
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var content = JsonSerializer.Deserialize<PagedProductListResponse>(responseBody);

        Assert.That(content, Is.Not.Null);

        if (content == null) return;

        Assert.Multiple(() =>
        {
            Assert.That(content.TotalPages, Is.EqualTo(1));
            Assert.That(content.Products.Count(), Is.EqualTo(1));
            Assert.That(content.Products.FirstOrDefault()?.Name, Is.EqualTo(expectedKeyword));
        });
    }
}