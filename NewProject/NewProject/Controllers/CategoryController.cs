using Application.Features.Categories.Commands;
using Application.Features.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewProject.APIs.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "AdminPolicy")]
public class CategoryController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetListCategories()
    {
        var query = new GetCategoryListQuery();
        var response = await mediator.Send(query);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategoryById(Guid id)
    {
        GetCategoryByIdQuery query = new()
        {
            Id = id
        };
        var response = await mediator.Send(query);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommandRequest request)
    {
        await mediator.Send(request);

        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryCommandRequest request)
    {
        request.Id = id;
        await mediator.Send(request);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        DeleteCategoryCommandRequest request = new()
        {
            Id = id
        };
        await mediator.Send(request);

        return Ok();
    }
}