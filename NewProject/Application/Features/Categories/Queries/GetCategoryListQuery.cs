using MediatR;
using Domain.Interfaces;

namespace Application.Features.Categories.Queries;

public class GetCategoryListQuery : IRequest<List<GetCategoryQueryResponse>>
{
}

public class GetCategoryListQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetCategoryListQuery, List<GetCategoryQueryResponse>>
{
    public Task<List<GetCategoryQueryResponse>> Handle(GetCategoryListQuery query, CancellationToken cancellationToken)
    {
        var result = unitOfWork
            .CategoryRepository
            .GetAll()
            .Select(c => new GetCategoryQueryResponse
            {
                Id = c.Id,
                Name = c.Name,
                LastSavedTime = c.LastSavedTime
            })
            .OrderByDescending(o => o.LastSavedTime)
            .ToList();

        return Task.FromResult(result);
    }
}