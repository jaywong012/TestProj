using System.Text.Json.Serialization;
using MediatR;
using Domain.Interfaces;
using Domain.Utilities;

namespace Application.Features.Products.Queries;

public class GetProductListQuery : IRequest<List<GetProductQueryResponse>>
{
    public string? SearchKey { get; set; }
}

public class GetProductListRequestHandler : IRequestHandler<GetProductListQuery, List<GetProductQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetProductListRequestHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<List<GetProductQueryResponse>> Handle(GetProductListQuery query, CancellationToken cancellationToken)
    {
        var productListQuery = _unitOfWork
            .ProductRepository
            .GetAll();

        if (!string.IsNullOrEmpty(query.SearchKey))
        {
            productListQuery = productListQuery.Where(p => p.Name.Contains(query.SearchKey));
        }
        var productList = productListQuery
            .OrderByDescending(o => o.LastSavedTime)
            .Select(p => new GetProductQueryResponse
            {
                Id = p.Id,
                Name = p.Name,
                CategoryId = p.CategoryId,
                LastSavedTime = FormatDateTime.ToViewAbleDateTime(p.LastSavedTime),
                Price = (int)p.Price,
                CategoryName = p.Category != null ? p.Category.Name : "",
                CreatedTime = p.LastCreatedTime
            })
            .ToList();
        return Task.FromResult(productList);
    }
}

public class GetProductQueryResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("categoryId")]
    public Guid? CategoryId { get; set; }

    [JsonPropertyName("lastSavedTime")]
    public string? LastSavedTime { get; init; }

    [JsonPropertyName("createdTime")]
    public DateTime? CreatedTime { get; init; }

    [JsonPropertyName("categoryName")]
    public string? CategoryName { get; set; }

    [JsonPropertyName("price")]
    public int Price { get; set; }
}