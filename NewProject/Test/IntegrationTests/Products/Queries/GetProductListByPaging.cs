using Application.Features.Logins.Commands;
using Application.Features.Products.Queries;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Application.Common;
using Domain.Entities;
using Test.Configurations.IntegrationTest;

namespace Test.IntegrationTests.Products.Queries;

public class GetProductListByPaging
{
    private InitConfigModel _configurations;
    private GetProductListByPagingQuery _request;

    [SetUp]
    public void Setup()
    {
        _configurations = InitConfigs.SetupInMemoryDatabase();
        _request = new GetProductListByPagingQuery()
        {
            PageIndex = 0,
            PageSize = 2,
            SearchKey = "Co"
        };
        DbContextHelper.ClearEntities<Product>(_configurations.Context);
        DbContextHelper.ClearEntities<Account>(_configurations.Context);
    }

    [TearDown]
    public void Teardown()
    {
        _configurations.Dispose();
    }

    [Test]
    public async Task GetProductListByPaging_NotEmptyItems_ReturnExactPageAndItems()
    {
        SeedDatabase.SeedProducts(_configurations.Context);
        SeedDatabase.SeedAccounts(_configurations.Context);

        var loginRequest = new GenerateJwtTokenCommandRequest
        {
            UserName = "jac",
            Password = "123"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, Constants.APPLICATION_JSON);
        var loginResponse = await _configurations.Client.PostAsync("api/Login", jsonContent);
        loginResponse.EnsureSuccessStatusCode();
        var jwtToken = loginResponse.Content.ReadAsStringAsync().Result;

        _configurations.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        var response = _configurations.Client.GetAsync($"api/Product/paged?pageIndex={_request.PageIndex}&pageSize={_request.PageSize}&searchKey={_request.SearchKey}").Result;
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var content = JsonSerializer.Deserialize<PagedProductListResponse>(responseBody);
        Assert.That(content, Is.Not.Null);
        if (content == null) return;
        Assert.Multiple(() =>
        {
            Assert.That(content.TotalPages, Is.EqualTo(1));
            Assert.That(content.Products.Count(), Is.EqualTo(1));
            Assert.That(content.Products.FirstOrDefault()?.Name, Is.EqualTo("CoCa"));
        });
    }
}