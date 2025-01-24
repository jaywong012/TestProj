using Application.Common;
using Application.Features.Accounts.Commands;
using Test.Configurations.IntegrationTest;

namespace Test.IntegrationTests.Accounts.Commands;

public class RegisterAccount
{
    private InitConfigModel _configurationsModel;
    private RegisterAccountCommandRequest _request;

    [SetUp]
    public void SetUp()
    {
        _configurationsModel = InitConfigs.SetupInMemoryDatabase();
        _request = new RegisterAccountCommandRequest
        {
            Password = "Test123",
            Role = Constants.ADMIN,
            UserName = "Test"
        };
    }

    [TearDown]
    public void TearDown()
    {
        _configurationsModel.Dispose();
    }


    [Test]
    public async Task Test()
    {
        var jsonContent = Utilities.SerializeToJsonContent(_request);
        var result = await _configurationsModel.Client.PostAsync(EndPointConstants.ACCOUNT, jsonContent);

        Assert.That(result.IsSuccessStatusCode);
    }
}