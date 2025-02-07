using Domain.Base;
using Domain.Common.Enums;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Utilities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;

namespace Application.Features.SocialAccessInfos.Commands;

public class CreateSocialAccessInfoCommandRequest : IRequest<bool>
{
    public string? UserId { get; set; }

    public required string UserName { get; set; }

    public required string AccessToken { get; set; }

    public string? AccessSecret { get; set; }

    public required string Type { get; set; }
}

public class CreateSocialAccessInfoCommandHandler : IRequestHandler<CreateSocialAccessInfoCommandRequest, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _key;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CreateSocialAccessInfoCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IOptions<JwtSettings> jwtSettings)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _key = jwtSettings.Value.Key;
    }

    public async Task<bool> Handle(CreateSocialAccessInfoCommandRequest request, CancellationToken cancellationToken)
    {
        var authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

        var userDetail = ClaimAuthorizationToken.ClaimTokens(authHeader, _key);
        var extractedUserId = "";
        if (!string.IsNullOrEmpty(request.UserId))
        {
            //extractedUserId = request.UserId.Split("/")[-1];
            var parts = request.UserId.Split('/');
            extractedUserId = parts[^1];
        }
        SocialAccessInfo socialInfo = new()
        {
            UserId = extractedUserId,
            UserName = request.UserName,
            AccountId = Guid.Parse(userDetail.UserId),
            Token = request.AccessToken,
            AccessSecret = request.AccessSecret,
            Type = request.Type.ToEnum<PostTypeEnum>()
        };

        var existToken = _unitOfWork
            .SocialAccessInfoRepository
            .GetAll()
            .FirstOrDefault(ac =>
                ac.AccountId == socialInfo.AccountId 
                && ac.Type == socialInfo.Type);

        if (existToken != null)
        {
            existToken.Token = socialInfo.Token;
            existToken.AccessSecret = socialInfo.AccessSecret;
            existToken.UserName = socialInfo.UserName;
            await _unitOfWork.SocialAccessInfoRepository.Update(existToken);
        }
        else
        {
            await _unitOfWork.SocialAccessInfoRepository.Add(socialInfo);
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}