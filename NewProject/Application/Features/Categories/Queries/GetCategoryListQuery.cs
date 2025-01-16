using MediatR;
using Domain.Interfaces;

namespace Application.Features.Categories.Queries;

public class GetCategoryListQuery : IRequest<List<GetCategoryQueryResponse>>
{
}

public class GetCategoryListQueryHandler : IRequestHandler<GetCategoryListQuery, List<GetCategoryQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetCategoryListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<List<GetCategoryQueryResponse>> Handle(GetCategoryListQuery query, CancellationToken cancellationToken)
    {
        var result = _unitOfWork
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