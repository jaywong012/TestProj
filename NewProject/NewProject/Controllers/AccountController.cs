using Application.Accounts.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NewProject.APIs.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;
    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<IActionResult> RegisterAccount([FromBody] RegisterAccountCommandRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}