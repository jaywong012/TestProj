using System.Text;
using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewProject.APIs.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetListProduct()
    {
        GetProductListQuery query = new();
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        GetProductByIdQuery query = new()
        {
            Id = id
        };
        var response = await _mediator.Send(query);

        return Ok(response);
    }

    [HttpGet("paged")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetProductListByPaging([FromQuery] GetProductListByPagingQuery request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpGet("download-file")]
    public async Task<IActionResult> DownloadCsv([FromQuery] GenerateCsvCommandRequest request)
    {
        var response = await _mediator.Send(request);
        var byteArray = Encoding.UTF8.GetBytes(response);
        var stream = new MemoryStream(byteArray);

        return File(stream, "text/csv", "product.csv");
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommandRequest request)
    {
        await _mediator.Send(request);

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductCommandRequest request)
    {
        request.Id = id;
        var response = await _mediator.Send(request);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        DeleteProductCommandRequest request = new()
        {
            Id = id
        };

        var response = await _mediator.Send(request);

        return Ok(response);
    }
}