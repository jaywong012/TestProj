using System.Text;
using System.Text.Json;
using Application.Common;

namespace Test.Configurations.IntegrationTest;

public static class Utilities
{
    public static StringContent SerializeToJsonContent(object requestObject)
    {
        return new StringContent(
            JsonSerializer.Serialize(requestObject),
            Encoding.UTF8,
            Constants.APPLICATION_JSON
        );
    }
}