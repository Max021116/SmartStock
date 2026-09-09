using Microsoft.AspNetCore.Mvc;
using SmartStock.Server.Services;
using SmartStock.Shared;

namespace SmartStock.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CategoryListDto>>> GetAll(CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetAllAsync(cancellationToken);
        var dtos = categories.Select(c => new CategoryListDto { Id = c.Id, Name = c.Name }).ToList();
        return Ok(dtos);
    }
}