using Application.Features.Logins.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NewProject.APIs.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly IMediator _mediator;
    public LoginController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] GenerateJwtTokenCommandRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}