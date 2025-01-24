using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Common;

namespace Application.Utilities
{
    public static class CustomJsonFormat
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
}
