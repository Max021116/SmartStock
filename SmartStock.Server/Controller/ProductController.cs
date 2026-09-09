using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartStock.Server.Services;
using SmartStock.Shared;

namespace SmartStock.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ProductListDto>>> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _productService.GetPagedListAsync(page, pageSize, search, cancellationToken: cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDetailDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(id, cancellationToken);
        if (product is null) return NotFound(new ApiErrorResponse { Message = $"Product {id} not found." });

        return Ok(new ProductDetailDto
        {
            Id = product.Id,
            SKU = product.SKU,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? "",
            IsActive = product.IsActive,
            RowVersion = Convert.ToBase64String(product.RowVersion)
        });
    }

    // Optional — needed for 409 testing
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePrice(
        int id,
        [FromBody] ProductUpdateDto dto,
        CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(id, cancellationToken);
        if (product is null) return NotFound(new ApiErrorResponse { Message = $"Product {id} not found." });

        product.Price = dto.Price;
        product.RowVersion = Convert.FromBase64String(dto.RowVersion);

        try
        {
            await _productService.UpdateAsync(product, cancellationToken);
            return Ok(new ApiErrorResponse { Message = "Product updated." });
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(new ApiErrorResponse
            {
                Message = "This product was changed by someone else. Refresh and try again."
            });
        }
    }
}