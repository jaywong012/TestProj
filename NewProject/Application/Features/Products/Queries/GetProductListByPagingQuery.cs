using System.Text.Json.Serialization;
using Application.Utilities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Products.Queries;

public class GetProductListByPagingQuery : IRequest<PagedProductListResponse>
{
    public string? SearchKey { get; set; }
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

    public Task<PagedProductListResponse> Handle(GetProductListByPagingQuery query, CancellationToken cancellationToken)
    {
        var productsQuery = _unitOfWork
            .ProductRepository
            .GetAll();

        if (!string.IsNullOrEmpty(query.SearchKey))
        {
            productsQuery = productsQuery.Where(p => p.Name.Contains(query.SearchKey));
        }

        var productsCount = productsQuery.Count();

        var totalPages = (int)Math.Ceiling(productsCount / (decimal)query.PageSize);

        var pageIndex = query.PageIndex > 0 ? query.PageIndex - 1 : 0;

        var pagedProducts= productsQuery
            .OrderByDescending(o => o.LastSavedTime)  
            .Skip(pageIndex * query.PageSize)
            .Take(query.PageSize)
            .Select(p => new GetProductQueryResponse
            {
                Id = p.Id,
                Name = p.Name,
                CategoryId = p.CategoryId,
                Price = (int)p.Price,
                CategoryName = p.Category != null ? p.Category.Name : "",
                LastSavedTime = FormatDateTime.HH_mm_MMM_dd(p.LastSavedTime)
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