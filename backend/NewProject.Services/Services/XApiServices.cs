using Domain.Base;
using Domain.Common.Enums;
using Domain.ErrorHandlingManagement;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NewProject.Services.Interface;
using NewProject.Services.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using Domain.Entities;
using Infrastructure.Utilities;

namespace NewProject.Services.Services;

public class XApiServices : IXApiServices
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _key;
    private readonly IUnitOfWork _unitOfWork;
    private readonly XTokens _xTokens;
    public XApiServices(IHttpContextAccessor httpContextAccessor, IOptions<JwtSettings> jwtSettings, IUnitOfWork unitOfWork, IOptions<SocialAccessTokensInfo> socialAccessTokensInfo)
    {
        _httpContextAccessor = httpContextAccessor;
        _key = jwtSettings.Value.Key;
        _unitOfWork = unitOfWork;
        _xTokens = socialAccessTokensInfo.Value.XTokens;
    }
    public async Task<bool> CheckUserRetweet(CheckRetweetRequest request)
    {
        bool tweetedStatus;
        var authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

        var userDetail = ClaimAuthorizationToken.ClaimTokens(authHeader, _key);

        var post = _unitOfWork
            .PostRepository
            .GetAll()
            .FirstOrDefault(p => p.Url == request.Url);

        if (!string.IsNullOrEmpty(post?.RetweetedUsers))
        {
            tweetedStatus = await CheckSaveUserTweetStatus(userDetail.UserId, post.Id);
            if (tweetedStatus) return true;
        }

        var bearerToken = _xTokens.AccessToken;

        var lastSegment = request.Url.TrimEnd('/').Split('/').Last();

        lastSegment = lastSegment.Split('?')[0];
        var tweetId = lastSegment;

        var retweetedByResponse = await GetRetweetedByAsync(tweetId, bearerToken);

        var retweetedIds = retweetedByResponse?.Data?.Select(user => user.Id).ToList();
        var retweetedUsersString = string.Join(",", retweetedIds ?? Enumerable.Empty<string>());
        post.RetweetedUsers = retweetedUsersString;
        await _unitOfWork.PostRepository.Update(post);
        if (string.IsNullOrEmpty(post?.RetweetedUsers)) throw new VerifyActionFailedException("Verify failed. Please try again in 15 minutes.");

        tweetedStatus = await CheckSaveUserTweetStatus(userDetail.UserId, post.Id);

        if (!tweetedStatus) throw new VerifyActionFailedException("You haven't shared the post, please try again after 15 minutes");
        return tweetedStatus;

        //// Check if more pages are available
        //if (!string.IsNullOrEmpty(retweetedByResponse.Meta?.NextToken))
        //{
        //    Console.WriteLine("There might be more retweeters. You can use the NextToken to paginate.");
        //}

    }

    private async Task<bool> CheckSaveUserTweetStatus(string userId, Guid postId)
    {
        var parseUserId = Guid.Parse(userId);
        var socialAccessInfo = _unitOfWork
            .SocialAccessInfoRepository
            .GetAll()
            .FirstOrDefault(at => at.AccountId == parseUserId && at.Type == PostTypeEnum.X);

        var post = _unitOfWork
            .PostRepository
            .GetAll()
            .FirstOrDefault(p => p.Id == postId);
        if (!post.RetweetedUsers.Contains(socialAccessInfo.UserId))
        {
            return false;
        }
        AccountPostShare accountPostShare = new()
        {
            AccountId = parseUserId,
            PostId = postId
        };
        await _unitOfWork.AccountPostShareRepository.Add(accountPostShare);
        return true;
    }

    private static async Task<TweetRetweetedByResponse> GetRetweetedByAsync(
        string tweetId,
        string bearerToken
    )
    {
        var url = $"https://api.twitter.com/2/tweets/{tweetId}/retweeted_by?user.fields=username";

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization
            = new AuthenticationHeaderValue("Bearer", bearerToken);

        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.TooManyRequests:
                    throw new TooManyRequestException($"Too many request. Please try again in 15 minutes.");
                default:
                {

                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception(
                        $"Request failed. Status: {response.StatusCode}. Body: {errorContent}");
                }
            }
        }

        var responseBody = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<TweetRetweetedByResponse>(responseBody);
        return result;
    }
}