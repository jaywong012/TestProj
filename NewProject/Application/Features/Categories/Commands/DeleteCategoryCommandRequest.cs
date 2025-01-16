using MediatR;
using Domain.Interfaces;
using Application.Features.Categories.Queries;
using Domain.ErrorHandlingManagement;

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


        var result = await _mediator.Send(new GetCategoryByIdQuery { Id = request.Id }, cancellationToken);

        if (result == null)
        {
            throw new ItemNotFoundException($"Category with ID {request.Id} not found");
        }

        await _unitOfWork.CategoryRepository.Delete(request.Id);
        return true;
    }
}