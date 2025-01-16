using System.Text.Json.Serialization;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Products.Queries;

public class GetProductListByPagingQuery : IRequest<PagedProductListResponse>
{
    public int PageIndex { get; init; }
    public int PageSize { get; init; } = 10;
}

public class GetProductListByPagingQueryHandler : IRequestHandler<GetProductListByPagingQuery, PagedProductListResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetProductListByPagingQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<PagedProductListResponse> Handle(GetProductListByPagingQuery request, CancellationToken cancellationToken)
    {
        var productsCount = _unitOfWork
            .ProductRepository
            .GetAll()
            .Count();

        var totalPages = (int)Math.Ceiling(productsCount / (decimal)request.PageSize);

        var pageIndex = request.PageIndex > 0 ? request.PageIndex - 1 : 0;

        var pagedProducts= _unitOfWork
            .ProductRepository
            .GetAll()
            .OrderByDescending(o => o.LastSavedTime)  
            .Skip(pageIndex * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new GetProductQueryResponse
            {
                Id = p.Id,
                Name = p.Name,
                CategoryId = p.CategoryId,
                Price = p.Price,
                CategoryName = p.Category != null ? p.Category.Name : "",
                LastSavedTime = p.GetFormattedLastSavedTime()
            });

        PagedProductListResponse response = new()
        {
            Products = pagedProducts,
            TotalPages = totalPages
        };

        return Task.FromResult(response);
    }
}

public class PagedProductListResponse
{
    [JsonPropertyName("products")]
    public required IEnumerable<GetProductQueryResponse> Products { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
}