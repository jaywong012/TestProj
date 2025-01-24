using System.Net;
using Application.Common;
using Application.Features.Products.Commands;
using Test.Configurations.IntegrationTest;

namespace Test.IntegrationTests.Products.Commands;

public class GenerateCsv
{
    private InitConfigModel _configurations;
    private GenerateCsvCommandRequest _request;

    [SetUp]
    public async Task SetUp()
    {
        _configurations = InitConfigs.SetupInMemoryDatabase();
        SeedDatabase.SeedProducts(_configurations.Context);
        _request = new GenerateCsvCommandRequest();
        _configurations.Client = await InitConfigs.GenerateToken(_configurations.Client);
    }

    [TearDown]
    public void TearDown()
    {
        _configurations.Dispose();
    }

    [Test]
    public async Task GenerateCsv_EmptySearchKey_GenerateReport()
    {
        var response = await _configurations.Client.GetAsync($"{EndPointConstants.PRODUCT_GENERATE_CSV}");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GenerateCsv_HaveSearchKey_GenerateReportWithFilter()
    {
        _request.SearchKey = "Coca";

        var response = await _configurations.Client.GetAsync($"{EndPointConstants.PRODUCT_GENERATE_CSV}?{_request.SearchKey}");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}