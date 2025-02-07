using Application.Features.Posts.Queries;
using MediatR;
using Domain.Interfaces;

namespace Application.Features.Posts.Commands;

public class DeletePostCommandRequest : IRequest<bool>
{
    public Guid Id { get; init; }
}

public class DeletePostCommandRequestHandler(IUnitOfWork unitOfWork, IMediator mediator)
    : IRequestHandler<DeletePostCommandRequest, bool>
{
    public async Task<bool> Handle(DeletePostCommandRequest request, CancellationToken cancellationToken)
    {
        GetPostByIdQuery existPostQuery = new()
        {
            Id = request.Id
        };
        await mediator.Send(existPostQuery, cancellationToken);
        await unitOfWork.PostRepository.Delete(request.Id);

        return true;
    }
}