using MediatR;
using Domain.Interfaces;
using Application.Utilities;

namespace Application.Features.Categories.Queries;

public class GetCategoryListQuery : IRequest<List<GetCategoryQueryResponse>>;

public class GetCategoryListQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetCategoryListQuery, List<GetCategoryQueryResponse>>
{
    public Task<List<GetCategoryQueryResponse>> Handle(GetCategoryListQuery query, CancellationToken cancellationToken)
    {
        var result = unitOfWork
            .CategoryRepository
            .GetAll()
            .OrderByDescending(o => o.LastSavedTime)
            .Select(c => new GetCategoryQueryResponse
            {
                Id = c.Id,
                Name = c.Name,
                LastSavedTime = FormatDateTime.HH_mm_MMM_dd(c.LastSavedTime)
            })
            .ToList();

        return Task.FromResult(result);
    }
}