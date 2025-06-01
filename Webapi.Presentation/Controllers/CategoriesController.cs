using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webapi.Application.CategoryCQRS.Commands.CreateCategory;
using Webapi.Application.CategoryCQRS.Commands.DeleteCategory;
using Webapi.Application.CategoryCQRS.Commands.UpdateCategory;
using Webapi.Application.CategoryCQRS.Queries.GetCategories;
using Webapi.Application.CategoryCQRS.Queries.GetCategoryById;
using Webapi.Presentation.Extensions;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.Params;

namespace Webapi.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories([FromQuery] CategoryParams categoryParams)
    {
        var categories = await _mediator.Send(new GetCategoriesQuery(categoryParams));
        
        Response.AddPaginationHeader(categories);
        
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(Guid id)
    {
        var category = await _mediator.Send(new GetCategoryByIdQuery(id));
        return Ok(category);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto createCategoryDto)
    {
        var category = await _mediator.Send(new CreateCategoryCommand(createCategoryDto));
        
        return CreatedAtAction(
            nameof(GetCategory),
            new { id = category.Id },
            category
        );
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateCategory(Guid id, UpdateCategoryDto updateCategoryDto)
    {
        await _mediator.Send(new UpdateCategoryCommand(id, updateCategoryDto));
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        await _mediator.Send(new DeleteCategoryCommand(id));
        return NoContent();
    }
}

