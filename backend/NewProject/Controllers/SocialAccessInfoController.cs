using Application.Features.SocialAccessInfos.Commands;
using Application.Features.SocialAccessInfos.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NewProject.APIs.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SocialAccessInfoController : ControllerBase
{
    private readonly IMediator _mediator;

    public SocialAccessInfoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserSocialAccessInfo()
    {
        var request = new RemoveUserSocialAccessInfoRequest();
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSocialAccessInfo([FromBody] CreateSocialAccessInfoCommandRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveSocialAccessInfo(Guid id)
    {
        var request = new RemoveSocialAccessInfoCommandRequest
        {
            Id = id
        };
        await _mediator.Send(request);
        return Ok();
    }
}