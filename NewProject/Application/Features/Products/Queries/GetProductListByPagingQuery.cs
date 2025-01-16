using System.Text.Json.Serialization;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Products.Queries;

public class GetProductListByPagingQuery : IRequest<PagedProductListResponse>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; } = 10;
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
        var count = _unitOfWork
            .ProductRepository
            .GetAll()
            .Count();
            
        var totalPages = (int)Math.Ceiling(count / (decimal)request.PageSize);

        var pagedProducts= _unitOfWork
            .ProductRepository
            .GetAll()
            .OrderByDescending(o => o.LastSavedTime)  
            .Skip(request.PageIndex * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new GetProductQueryResponse
            {
                Id = p.Id,
                Name = p.Name,
                CategoryId = p.CategoryId,
                Price = p.Price,
                //CategoryName = p.Category != null ? p.Category.Name : "",
                CreatedTime = p.LastCreatedTime
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