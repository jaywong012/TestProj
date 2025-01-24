using System.Text.Json.Serialization;
using Application.Utilities;
using MediatR;
using Domain.Interfaces;
using Domain.ErrorHandlingManagement;

namespace Application.Features.Categories.Queries;

public class GetCategoryByIdQuery : IRequest<GetCategoryQueryResponse>
{
    public Guid Id { get; init; }
}

public class GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetCategoryByIdQuery, GetCategoryQueryResponse>
{
    public async Task<GetCategoryQueryResponse> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var existCategory = await unitOfWork
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
            LastSavedTime = FormatDateTime.HH_mm_MMM_dd(existCategory.LastSavedTime)
        };
        return category;
    }
}

public class GetCategoryQueryResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("lastSavedTime")]
    public string? LastSavedTime { get; init; }
}