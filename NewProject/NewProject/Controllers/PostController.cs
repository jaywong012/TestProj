using Application.Features.Posts.Commands;
using Application.Features.Posts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewProject.Services.Interface;
using NewProject.Services.Models;

namespace NewProject.APIs.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly IMediator _mediator;
    public PostController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetListPost([FromQuery] GetListPostByPagingQuery query)
    {
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostCommandRequest request)
    {
        await _mediator.Send(request);

        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePost(Guid id, [FromBody] UpdatePostCommandRequest request)
    {
        request.Id = id;
        var response = await _mediator.Send(request);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePost(Guid id)
    {
        DeletePostCommandRequest request = new()
        {
            Id = id
        };

        var response = await _mediator.Send(request);

        return Ok(response);
    }

    [HttpPost("check-retweet")]
    public async Task<IActionResult> CheckUserRetweet([FromBody] GetSocialAccountRetweetStatus request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}