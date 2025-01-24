using Application.Utilities;
using MediatR;
using Domain.ErrorHandlingManagement;
using Domain.Interfaces;

namespace Application.Features.Products.Queries;

public class GetProductByIdQuery : IRequest<GetProductQueryResponse?>
{
    public Guid Id { get; init; }
}

public class GetProductByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetProductByIdQuery, GetProductQueryResponse?>
{
    public async Task<GetProductQueryResponse?> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductRepository.GetById(query.Id);
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
            LastSavedTime = FormatDateTime.HH_mm_MMM_dd(product.LastSavedTime),
            Price = (int)product.Price
        };

        return result;
    }
}