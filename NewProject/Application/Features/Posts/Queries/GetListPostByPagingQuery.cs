using Domain.Base;
using Domain.Interfaces;
using Infrastructure.Utilities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Application.Features.Posts.Queries;

public class GetListPostByPagingQuery : IRequest<GetListPostPagingResponse>
{
    public string? SearchKey { get; init; }
    public int PageIndex { get; init; }
    public int PageSize { get; init; } = 10;
}


public class GetListPostByPagingQueryHandler : IRequestHandler<GetListPostByPagingQuery, GetListPostPagingResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _key;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public GetListPostByPagingQueryHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IOptions<JwtSettings> jwtSettings)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _key = jwtSettings.Value.Key;
    }
    public Task<GetListPostPagingResponse> Handle(GetListPostByPagingQuery query, CancellationToken cancellationToken)
    {
        var authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

        var userDetail = ClaimAuthorizationToken.ClaimTokens(authHeader, _key);

        var postsQuery = _unitOfWork
            .PostRepository
            .GetAll();

        if (!string.IsNullOrEmpty(query.SearchKey))
        {
            postsQuery = postsQuery.Where(p => p.Url.Contains(query.SearchKey));
        }

        var postsCount = postsQuery.Count();

        var totalPages = (int)Math.Ceiling(postsCount / (decimal)query.PageSize);

        var pageIndex = query.PageIndex > 0 ? query.PageIndex - 1 : 0;

        var accountPost = _unitOfWork
            .AccountPostShareRepository
            .GetAll()
            .Where(aps => aps.AccountId == Guid.Parse(userDetail.UserId))
            .Select(aps => aps.PostId) 
            .ToHashSet();

        var pagedPosts = postsQuery
            .OrderByDescending(o => o.LastSavedTime)
            .Skip(pageIndex * query.PageSize)
            .Take(query.PageSize)
            .Select(p => new GetListPostResponse
            {
                Id = p.Id,
                Url = p.Url,
                Type = p.Type.GetDescription(),
                IsShared = accountPost.Contains(p.Id),
                LastSavedTime = FormatDateTime.HH_mm_MMM_dd(p.LastSavedTime)
            });


        GetListPostPagingResponse response = new()
        {
            Posts = pagedPosts,
            TotalPages = totalPages
        };

        return Task.FromResult(response);
    }
}

public class GetListPostPagingResponse
{
    public IEnumerable<GetListPostResponse> Posts { get; set; }
    public int TotalPages { get; set; }
}

public class GetListPostResponse
{
    public Guid Id { get; set; }

    public string Url { get; set; }

    public string Type { get; set; }

    public bool IsShared { get; set; }

    public string LastSavedTime { get; set; }
}