using Domain.Entities;
using Domain.ErrorHandlingManagement;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.SocialAccessInfos.Queries;

public class GetSocialAccessInfoByIdRequest : IRequest<SocialAccessInfo>
{
    public Guid Id { get; set; }
}

public class GetSocialAccessInfoByIdRequestHandler : IRequestHandler<GetSocialAccessInfoByIdRequest, SocialAccessInfo>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSocialAccessInfoByIdRequestHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public Task<SocialAccessInfo?> Handle(GetSocialAccessInfoByIdRequest request, CancellationToken cancellationToken)
    {
        var response = _unitOfWork
            .SocialAccessInfoRepository
            .GetById(request.Id);

        if (response == null)
        {
            throw new ItemNotFoundException($"Account {response.Id} not found");
        }

        return response;
    }
}