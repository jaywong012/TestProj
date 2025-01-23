using System.Net;
using Application.Features.Products.Commands;
using Domain.Entities;
using Test.Configurations.IntegrationTest;

namespace Test.IntegrationTests.Products.Commands;

public class GenerateCsv
{
    private InitConfigModel _initConfigModel;
    private GenerateCsvCommandRequest _request;

    [SetUp]
    public void SetUp()
    {
        _initConfigModel = InitConfigs.SetupInMemoryDatabase();
        _request = new GenerateCsvCommandRequest();
        DbContextHelper.ClearEntities<Product>(_initConfigModel.Context);
    }

    [TearDown]
    public void TearDown()
    {
        _initConfigModel.Dispose();
    }

    [Test]
    public async Task GenerateCsv_EmptySearchKey_GenerateReport()
    {
        SeedDatabase.SeedProducts(_initConfigModel.Context);
        var response = await _initConfigModel.Client.GetAsync("api/product/generate-csv");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GenerateCsv_HaveSearchKey_GenerateReportWithFilter()
    {
        SeedDatabase.SeedProducts(_initConfigModel.Context);
        _request.SearchKey = "Coca";

        var response = await _initConfigModel.Client.GetAsync($"api/product/generate-csv?{_request.SearchKey}");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}