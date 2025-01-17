using MediatR;
using Domain.ErrorHandlingManagement;
using Domain.Interfaces;
using Domain.Utilities;

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

    public async Task<GetProductQueryResponse?> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetById(query.Id);
        if (product == null)
        {
            throw new ItemNotFoundException($"Product with ID {query.Id} not found");
        }

        GetProductQueryResponse result = new()
        {
            Id = product.Id,
            CategoryName = product.Category?.Name,
            CategoryId = product.CategoryId,
            Name = product.Name,
            LastSavedTime = FormatDateTime.ToViewAbleDateTime(product.LastSavedTime),
            Price = (int)product.Price
        };

        return result;
    }
}