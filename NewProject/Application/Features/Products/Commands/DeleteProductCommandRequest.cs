using MediatR;
using Domain.Interfaces;
using Application.Features.Products.Queries;
using Domain.ErrorHandlingManagement;

namespace Application.Features.Products.Commands;

public class DeleteProductCommandRequest : IRequest<bool>
{
    public Guid Id { get; init; }
}

public class DeleteProductCommandRequestHandler : IRequestHandler<DeleteProductCommandRequest, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public DeleteProductCommandRequestHandler(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<bool> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
    {
        GetProductByIdQuery existProductQuery = new()
        {
            Id = request.Id
        };
        var existProduct = await _mediator.Send(existProductQuery, cancellationToken);
        if (existProduct == null)
        {
            throw new ItemNotFoundException($"Product with ID {request.Id} not found");
        }
        await _unitOfWork.ProductRepository.Delete(request.Id);

        return true;
    }
}