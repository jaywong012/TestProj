using Application.Features.SocialAccessInfos.Queries;
using Domain.Base;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Features.SocialAccessInfos.Commands;

public class RemoveSocialAccessInfoCommandRequest : IRequest
{
    public Guid Id { get; set; }
}

public class GetUserSocialAccessInfoRequestHandler : IRequestHandler<RemoveSocialAccessInfoCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _key;
    private readonly IMediator _mediator;

    public GetUserSocialAccessInfoRequestHandler(IUnitOfWork unitOfWork, IOptions<JwtSettings> jwtSettings, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _key = jwtSettings.Value.Key;
        _mediator = mediator;
    }

    public async Task Handle(RemoveSocialAccessInfoCommandRequest request, CancellationToken cancellationToken)
    {
        var socialAccountRequest = new GetSocialAccessInfoByIdRequest()
        {
            Id = request.Id
        };
        var socialAccountExist = await _mediator.Send(socialAccountRequest, cancellationToken);
        await _unitOfWork.SocialAccessInfoRepository.Delete(socialAccountExist.Id);
    }
}