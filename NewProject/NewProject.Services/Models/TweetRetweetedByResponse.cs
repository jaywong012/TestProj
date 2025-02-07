using Newtonsoft.Json;

namespace NewProject.Services.Models;

public class TweetRetweetedByResponse
{
    [JsonProperty("data")]
    public List<TwitterUser> Data { get; set; }

    [JsonProperty("meta")]
    public Meta Meta { get; set; }

    [JsonProperty("errors")]
    public List<ErrorResponse> Errors { get; set; }
}

public class TwitterUser
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("username")]
    public string Username { get; set; }

    // Add other user fields if needed, e.g. name, created_at, etc.
}

public class Meta
{
    [JsonProperty("result_count")]
    public int ResultCount { get; set; }

    [JsonProperty("next_token")]
    public string NextToken { get; set; }

    // Possibly other fields
}

public class ErrorResponse
{
    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("code")]
    public int Code { get; set; }

    // Or if using the new V2 error format:
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("detail")]
    public string Detail { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
}