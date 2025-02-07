using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewProject.Services.Models
{
    public class FacebookResponse
    {
        [JsonProperty("data")]
        public List<FacebookPost> Data { get; set; }

        [JsonProperty("paging")]
        public Paging Paging { get; set; }
    }

    public class FacebookPost
    {
        // Note: The message field may not be present in every post, so it can be nullable.
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("permalink_url")]
        public string PermalinkUrl { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class Paging
    {
        [JsonProperty("previous")]
        public string Previous { get; set; }

        [JsonProperty("next")]
        public string Next { get; set; }
    }
}
