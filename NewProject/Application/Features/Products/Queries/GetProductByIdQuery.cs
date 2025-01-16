using MediatR;
using Domain.ErrorHandlingManagement;
using Domain.Interfaces;

namespace Application.Features.Products.Queries;

public class GetProductByIdQuery : IRequest<GetProductQueryResponse?>
{
    public Guid Id { get; init; }
}

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, GetProductQueryResponse?>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetProductByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GetProductQueryResponse?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetById(request.Id);
        if (product == null)
        {
            throw new ItemNotFoundException($"Product with ID {request.Id} not found");
        }

        GetProductQueryResponse result = new()
        {
            Id = product.Id,
            CategoryName = product.Category?.Name,
            CategoryId = product.CategoryId,
            Name = product.Name,
            LastSavedTime = product.LastSavedTime,
            Price = product.Price
        };

        return result;
    }
}