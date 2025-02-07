using Domain.Entities;
using MediatR;
using Domain.ErrorHandlingManagement;
using Domain.Interfaces;
using Infrastructure.Utilities;

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

        var mappingProfile = new MappingProfile<Product, GetProductQueryResponse>();
        var result = mappingProfile.Map(product);
        result.LastSavedTime = FormatDateTime.HH_mm_MMM_dd(product.LastSavedTime);
        result.CategoryName = product.Category?.Name;

        return result;
    }
}