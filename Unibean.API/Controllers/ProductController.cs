using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.Products;
using Unibean.Service.Services.Interfaces;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("🛍️Product API")]
[Route("api/v1/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService productService;

    private readonly IJwtService jwtService;

    public ProductController(IProductService productService,
        IJwtService jwtService)
    {
        this.productService = productService;
        this.jwtService = jwtService;
    }

    /// <summary>
    /// Get product list
    /// </summary>
    /// <param name="categoryIds">Filter by category Id.</param>
    /// <param name="levelIds">Filter by level Id.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(PagedResultModel<ProductModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public ActionResult<PagedResultModel<ProductModel>> GetList(
        [FromQuery] List<string> categoryIds,
        [FromQuery] List<string> levelIds,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string jwtToken = HttpContext.Request.Headers["Authorization"];

        string propertySort = paging.Sort.Split(",")[0];
        var propertyInfo = typeof(Product).GetProperty(propertySort);
        if (propertySort != null && propertyInfo != null)
        {
            PagedResultModel<ProductModel>
                result = productService.GetAll
                (categoryIds, levelIds, propertySort, paging.Sort.Split(",")[1].Equals("asc"), 
                paging.Search, paging.Page, paging.Limit, jwtService.GetJwtRequest(jwtToken.Split(" ")[1]));
            return Ok(result);
        }
        return BadRequest("Invalid property of product");
    }

    /// <summary>
    /// Get product by id
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(ProductExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult GetById(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string jwtToken = HttpContext.Request.Headers["Authorization"];

        try
        {
            return Ok(productService.GetById(id, jwtService.GetJwtRequest(jwtToken.Split(" ")[1])));
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Create product
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ProductModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Create([FromForm] CreateProductModel creation)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var product = await productService.Add(creation);
            if (product != null)
            {
                return StatusCode(StatusCodes.Status201Created, product);
            }
            return NotFound("Create fail");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Update product
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ProductModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Update(string id, [FromForm] UpdateProductModel update)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var product = await productService.Update(id, update);
            if (product != null)
            {
                return StatusCode(StatusCodes.Status200OK, product);
            }
            return NotFound("Update fail");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Delete product
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult Delete(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            productService.Delete(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }
}
