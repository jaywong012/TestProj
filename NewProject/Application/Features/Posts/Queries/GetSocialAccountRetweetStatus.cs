using Domain.Common.Enums;
using Infrastructure.Utilities;
using MediatR;
using NewProject.Services.Interface;
using NewProject.Services.Models;

namespace Application.Features.Posts.Queries;

public class GetSocialAccountRetweetStatus : IRequest<bool>
{
    public string Url { get; set; }
    
    public string Type { get; set; }
}

public class GetSocialAccountRetweetStatusHandler : IRequestHandler<GetSocialAccountRetweetStatus, bool>
{
    private readonly IXApiServices _xApiServices;
    private readonly IFacebookApiService _facebookApiServices;
    public GetSocialAccountRetweetStatusHandler(IXApiServices xApiServices, IFacebookApiService facebookApiService)
    {
        _xApiServices = xApiServices;
        _facebookApiServices = facebookApiService;
    }
    public async Task<bool> Handle(GetSocialAccountRetweetStatus request, CancellationToken cancellationToken)
    {
        var enumType = Enum.Parse<PostTypeEnum>(request.Type);
        var result = false;
        switch (enumType)
        {
            case PostTypeEnum.X:
                result = await _xApiServices.CheckUserRetweet(new CheckRetweetRequest
                {
                    Url = request.Url
                });
                break;
            case PostTypeEnum.Facebook:
                result = await _facebookApiServices.CheckFacebookUserRetweet(new CheckRetweetRequest
                {
                    Url = request.Url
                });
                break;
        }
        return result;
    }
}