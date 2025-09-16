using Domain.Base;
using Domain.Interfaces;
using Infrastructure.Utilities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Application.Features.SocialAccessInfos.Queries;

public class RemoveUserSocialAccessInfoRequest : IRequest<IEnumerable<UserSocialAccessResponse>>;

public class GetUserSocialAccessInfoRequestHandler : IRequestHandler<RemoveUserSocialAccessInfoRequest, IEnumerable<UserSocialAccessResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _key;

    public GetUserSocialAccessInfoRequestHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IOptions<JwtSettings> jwtSettings)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _key = jwtSettings.Value.Key;
    }

    public async Task<IEnumerable<UserSocialAccessResponse>> Handle(RemoveUserSocialAccessInfoRequest request, CancellationToken cancellationToken)
    {
        var authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
        var userDetail = ClaimAuthorizationToken.ClaimTokens(authHeader, _key);

        var userSocialAccessInfos = _unitOfWork
            .SocialAccessInfoRepository
            .GetAll()
            .Where(x => x.AccountId == Guid.Parse(userDetail.UserId))
            .Select(x => new UserSocialAccessResponse
            {
                Id = x.Id,
                UserName = x.UserName,
                Type = x.Type.GetDescription()
            });

        return await Task.FromResult(userSocialAccessInfos.ToList());
    }
}

public class UserSocialAccessResponse
{
    public Guid Id { get; set; }

    public required string UserName { get; set; }

    public required string Type { get; set; }
}