using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NewProject.Services.Interface;
using NewProject.Services.Models;
using Newtonsoft.Json;
using Domain.Base;

namespace NewProject.Services.Services;

public class FacebookApiService : IFacebookApiService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _key;
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _accessToken;
    public FacebookApiService(IHttpContextAccessor httpContextAccessor, 
        IOptions<JwtSettings> jwtSettings, 
        IUnitOfWork unitOfWork,
        IOptions<SocialAccessTokensInfo> socialAccessTokensInfo)
    {
        _httpContextAccessor = httpContextAccessor;
        _key = jwtSettings.Value.Key;
        _unitOfWork = unitOfWork;
        _accessToken = "EAA3Pe7Ri2PUBOzhFA0ZACmaE5l05OAsZCQR308YB4QOJhxUJ8zeOQWOL8bG0ZCE18aVZBVn0pXALek2DiAyzyJ2LWOUppKkiZAwe7tf3iyynj38ZCS7lZBTIc4xZB4qi4yNwnZAqi3l3cKNMH6DO5ZBJZCXs8y56SP4W0yTWSp8T8UgxHvQXYIY7ZAwaBArQLUchmfiJbZClMMYZAug19xTx6ZC7ZCavARqyZAoYqYERCifbP5FZCNNqZCqDxHKoEfJAZBLJCf3pSQJvqBYZD";
    }

    public async Task<string> RetrieveUrlByTitle(string title)
    {
        using var client = new HttpClient();

        var requestUrl = $"https://graph.facebook.com/v22.0/me/posts?fields=message,permalink_url&access_token={_accessToken}";
        var pageCount = 0;
        const int maxPages = 2;

        while (!string.IsNullOrEmpty(requestUrl) && pageCount < maxPages)
        {
            var response = await client.GetAsync(requestUrl);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var fbResponse = JsonConvert.DeserializeObject<FacebookResponse>(jsonResponse);
            var filteredPosts = fbResponse.Data.Where(post => !string.IsNullOrEmpty(post.Message)).ToList();
            if (filteredPosts.Count == 0) return string.Empty;

            foreach (var post in filteredPosts)
            {
                // Here we assume that the "message" field holds the post title or relevant content.
                if (!string.IsNullOrEmpty(post.Message) && post.Message.Contains(title, StringComparison.OrdinalIgnoreCase))
                {
                    // If found, return the permalink URL.
                    return post.PermalinkUrl;
                }
            }
            requestUrl = fbResponse?.Paging?.Next;
            pageCount++;
        }


        return string.Empty;
    }


    public async Task<bool> CheckFacebookUserRetweet(CheckRetweetRequest request)
    {
        return false;
    }
}