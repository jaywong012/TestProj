using System.Text.Json.Serialization;
using MediatR;
using Domain.Interfaces;

namespace Application.Features.Products.Queries;

public class GetProductListQuery : IRequest<List<GetProductQueryResponse>>
{

}

public class GetProductListRequestHandler : IRequestHandler<GetProductListQuery, List<GetProductQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetProductListRequestHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<List<GetProductQueryResponse>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
    {
        var productList = _unitOfWork
            .ProductRepository
            .GetAll()
            .Select(p => new GetProductQueryResponse
            {
                Id = p.Id,
                Name = p.Name,
                CategoryId = p.CategoryId,
                LastSavedTime = p.LastSavedTime,
                Price = p.Price,
                CategoryName = p.Category != null ? p.Category.Name : "",
                CreatedTime = p.LastCreatedTime
            })
            .OrderByDescending(o => o.LastSavedTime)
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
    public DateTime? LastSavedTime { get; init; }

    [JsonPropertyName("createdTime")]
    public DateTime? CreatedTime { get; init; }

    [JsonPropertyName("categoryName")]
    public string? CategoryName { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }
}