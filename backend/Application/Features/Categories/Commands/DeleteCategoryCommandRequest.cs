using MediatR;
using Domain.Interfaces;
using Application.Features.Categories.Queries;

namespace Application.Features.Categories.Commands;

public class DeleteCategoryCommandRequest : IRequest<bool>
{
    public Guid Id { get; init; }
}

public class DeleteCategoryCommandRequestHandler(IUnitOfWork unitOfWork, IMediator mediator)
    : IRequestHandler<DeleteCategoryCommandRequest, bool>
{
    public async Task<bool> Handle(DeleteCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        await mediator.Send(new GetCategoryByIdQuery { Id = request.Id }, cancellationToken);
        await unitOfWork.CategoryRepository.Delete(request.Id);
        return true;
    }
}