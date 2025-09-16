using Application.Features.Logins.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NewProject.APIs.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] GenerateJwtTokenCommandRequest request)
    {
        var response = await mediator.Send(request);
        return Ok(response);
    }
}