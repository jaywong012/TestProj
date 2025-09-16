using Application.Features.Accounts.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NewProject.APIs.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> RegisterAccount([FromBody] RegisterAccountCommandRequest request)
    {
        var response = await mediator.Send(request);
        return Ok(response);
    }
}