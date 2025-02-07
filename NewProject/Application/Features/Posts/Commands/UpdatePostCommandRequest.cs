using Application.Features.Posts.Queries;
using MediatR;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Utilities;

namespace Application.Features.Posts.Commands;

public class UpdatePostCommandRequest(Guid id, string url, string type)
    : IRequest<Post>
{
    public Guid Id { get; set; } = id;

    public string Url { get; set; } = url;

    public string Type { get; set; } = type;

}

public class UpdatePostCommandRequestHandler(IUnitOfWork unitOfWork, IMediator mediator)
    : IRequestHandler<UpdatePostCommandRequest, Post>
{
    public async Task<Post> Handle(UpdatePostCommandRequest request, CancellationToken cancellationToken)
    {
        GetPostByIdQuery existPostQuery = new()
        {
            Id = request.Id
        };
        await mediator.Send(existPostQuery, cancellationToken);

        var mappingProfile = new MappingProfile<UpdatePostCommandRequest, Post>();
        var post = mappingProfile.Map(request);

        await unitOfWork.PostRepository.Update(post);
        return post;
    }
}