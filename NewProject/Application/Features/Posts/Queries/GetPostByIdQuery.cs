using Domain.Entities;
using MediatR;
using Domain.ErrorHandlingManagement;
using Domain.Interfaces;

namespace Application.Features.Posts.Queries;

public class GetPostByIdQuery : IRequest<Post?>
{
    public Guid Id { get; init; }
}

public class GetProductByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetPostByIdQuery, Post?>
{
    public async Task<Post?> Handle(GetPostByIdQuery query, CancellationToken cancellationToken)
    {
        var post = await unitOfWork.PostRepository.GetById(query.Id);
        if (post == null)
        {
            throw new ItemNotFoundException($"Post with ID {query.Id} not found");
        }

        return post;
    }
}