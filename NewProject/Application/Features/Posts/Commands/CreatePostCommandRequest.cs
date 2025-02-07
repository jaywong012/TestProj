using Domain.Common.Enums;
using MediatR;
using Domain.Entities;
using Domain.ErrorHandlingManagement;
using Domain.Interfaces;
using Infrastructure.Utilities;
using NewProject.Services.Interface;

namespace Application.Features.Posts.Commands;

public class CreatePostCommandRequest : IRequest
{
    public string? Url { get; set; }

    public string? Title { get; set; }

    public required string Type { get; init; }
}

public class CreatePostCommandRequestHandler : IRequestHandler<CreatePostCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFacebookApiService _facebookApiService;

    public CreatePostCommandRequestHandler(IUnitOfWork unitOfWork, IFacebookApiService facebookApiService)
    {
        _unitOfWork = unitOfWork;
        _facebookApiService = facebookApiService;
    }

    public async Task Handle(CreatePostCommandRequest request, CancellationToken cancellationToken)
    {
        if (request.Type.ToEnum<PostTypeEnum>() == PostTypeEnum.Facebook)
        {
            var url = await _facebookApiService.RetrieveUrlByTitle(request.Title);
            if (url == null) throw new ItemNotFoundException("Can't find url");
            request.Url = url;
        }

        var mappingProfile = new MappingProfile<CreatePostCommandRequest, Post>();
        var post = mappingProfile.Map(request);

        await _unitOfWork.PostRepository.Add(post);
    }
}