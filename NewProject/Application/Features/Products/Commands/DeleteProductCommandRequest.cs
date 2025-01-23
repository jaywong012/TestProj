using MediatR;
using Domain.Interfaces;
using Application.Features.Products.Queries;

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
        await _mediator.Send(existProductQuery, cancellationToken);
        await _unitOfWork.ProductRepository.Delete(request.Id);

        return true;
    }
}