using MediatR;
using Domain.Interfaces;
using Application.Features.Products.Queries;

namespace Application.Features.Products.Commands;

public class DeleteProductCommandRequest : IRequest<bool>
{
    public Guid Id { get; init; }
}

public class DeleteProductCommandRequestHandler(IUnitOfWork unitOfWork, IMediator mediator)
    : IRequestHandler<DeleteProductCommandRequest, bool>
{
    public async Task<bool> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
    {
        GetProductByIdQuery existProductQuery = new()
        {
            Id = request.Id
        };
        await mediator.Send(existProductQuery, cancellationToken);
        await unitOfWork.ProductRepository.Delete(request.Id);

        return true;
    }
}