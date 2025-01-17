using System.Text.Json.Serialization;
using MediatR;
using Domain.Interfaces;
using Domain.ErrorHandlingManagement;

namespace Application.Features.Categories.Queries;

public class GetCategoryByIdQuery : IRequest<GetCategoryQueryResponse>
{
    public Guid Id { get; init; }
}

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, GetCategoryQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GetCategoryQueryResponse> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var existCategory = await _unitOfWork
            .CategoryRepository
            .GetById(request.Id);

        if (existCategory == null)
        {
            throw new ItemNotFoundException($"Category with ID {request.Id} not found");
        }

        GetCategoryQueryResponse category = new()
        {
            Id = existCategory.Id,
            Name = existCategory.Name,
            LastSavedTime = existCategory.LastSavedTime
        };
        return category;
    }
}

public class GetCategoryQueryResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }
    [JsonPropertyName("lastSavedTime")]
    public DateTime? LastSavedTime { get; init; }
}