using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Brands;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Services.Interfaces;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("Brand API")]
[Route("api/v1/brands")]
public class BrandController : ControllerBase
{
    private readonly IBrandService brandService;

    private readonly IJwtService jwtService;

    public BrandController(IBrandService brandService, 
        IJwtService jwtService)
    {
        this.brandService = brandService;
        this.jwtService = jwtService;
    }

    /// <summary>
    /// Get brand list
    /// </summary>
    /// <param name="sort">Sorting criteria for the results.</param>
    /// <param name="search">Search query.</param>
    /// <param name="page">Current page in the paginated results.</param>
    /// <param name="limit">Number of results per page.</param>
    [HttpGet]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(PagedResultModel<BrandModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public ActionResult<PagedResultModel<BrandModel>> GetList(
        [FromQuery] string sort = "Id,desc",
        [FromQuery] string search = "",
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string jwtToken = HttpContext.Request.Headers["Authorization"];

        string propertySort = sort.Split(",")[0];
        var propertyInfo = typeof(Brand).GetProperty(propertySort);
        if (propertySort != null && propertyInfo != null)
        {
            PagedResultModel<BrandModel>
                result = brandService.GetAll
                (propertySort, sort.Split(",")[1].Equals("asc"), search, page, limit, jwtService.GetJwtRequest(jwtToken.Split(" ")[1]));
            return Ok(result);
        }
        return BadRequest("Invalid property of brand");
    }

    /// <summary>
    /// Get brand by id
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(BrandExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult GetById(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string jwtToken = HttpContext.Request.Headers["Authorization"];

        try
        {
            return Ok(brandService.GetById(id, jwtService.GetJwtRequest(jwtToken.Split(" ")[1])));
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }
}
