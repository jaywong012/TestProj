using MediatR;
using Domain.Interfaces;
using Application.Features.Categories.Queries;

namespace Application.Features.Categories.Commands;

public class DeleteCategoryCommandRequest : IRequest<bool>
{
    public Guid Id { get; init; }
}

public class DeleteCategoryCommandRequestHandler : IRequestHandler<DeleteCategoryCommandRequest, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    public DeleteCategoryCommandRequestHandler(IUnitOfWork unitOfWork ,IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<bool> Handle(DeleteCategoryCommandRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new GetCategoryByIdQuery { Id = request.Id }, cancellationToken);
        await _unitOfWork.CategoryRepository.Delete(request.Id);
        return true;
    }
}