using System.Net;
using Application.Common;
using Application.Features.Logins.Commands;
using Test.Configurations.IntegrationTest;

namespace Test.IntegrationTests.Logins;

public class GenerateJwtToken
{
    private InitConfigModel _configurations;
    private GenerateJwtTokenCommandRequest _request;

    [SetUp]
    public void SetUp()
    {
        _configurations = InitConfigs.SetupInMemoryDatabase();
        _request = new GenerateJwtTokenCommandRequest
        {
            UserName = "jac",
            Password = "1234"
        };
    }

    [Test]
    public async Task GenerateJwtToken_IncorrectPassword_ReturnErrorMessage()
    {
        SeedDatabase.SeedAccounts(_configurations.Context);

        var jsonContent = Utilities.SerializeToJsonContent(_request);
        var loginResponse = await _configurations.Client.PostAsync(EndPointConstants.LOGIN, jsonContent);
        Assert.That(loginResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
}