using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Categories;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Services.Interfaces;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("Category API")]
[Route("api/v1/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        this.categoryService = categoryService;
    }

    /// <summary>
    /// Get category list
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(PagedResultModel<CategoryModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public ActionResult<PagedResultModel<CategoryModel>> GetList(
        [FromQuery] string sort = "Id,desc",
        [FromQuery] string search = "",
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string propertySort = sort.Split(",")[0];
        var propertyInfo = typeof(Category).GetProperty(propertySort);
        if (propertySort != null && propertyInfo != null)
        {
            PagedResultModel<CategoryModel>
                result = categoryService.GetAll
                (propertySort, sort.Split(",")[1].Equals("asc"), search, page, limit);
            return Ok(result);
        }
        return BadRequest("Invalid property of category");
    }

    /// <summary>
    /// Get category by id
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(CategoryModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult GetById(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return Ok(categoryService.GetById(id));
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Create category
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CategoryModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Create([FromForm] CreateCategoryModel creation)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var category = await categoryService.Add(creation);
            if (category != null)
            {
                return StatusCode(StatusCodes.Status201Created, category);
            }
            return NotFound("Create fail");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Update category
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CategoryModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Update(string id, [FromForm] UpdateCategoryModel update)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var category = await categoryService.Update(id, update);
            if (category != null)
            {
                return StatusCode(StatusCodes.Status200OK, category);
            }
            return NotFound("Update fail");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Delete category
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
            categoryService.Delete(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }
}
