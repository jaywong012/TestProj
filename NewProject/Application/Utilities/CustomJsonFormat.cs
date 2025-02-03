using System.Text;
using System.Text.Json;
using Application.Common;

namespace Application.Utilities;

public static class CustomJsonFormat
{
    public static StringContent SerializeToJsonContent(object requestObject)
    {
        return new StringContent(
            JsonSerializer.Serialize(requestObject),
            Encoding.UTF8,
            CommonConstants.APPLICATION_JSON
        );
    }
}