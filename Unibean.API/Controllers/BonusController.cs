using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Bonuses;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Services.Interfaces;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("🎁Bonus API")]
[Route("api/v1/bonuses")]
public class BonusController : ControllerBase
{
    private readonly IBonusService bonusService;

    public BonusController(IBonusService bonusService)
    {
        this.bonusService = bonusService;
    }

    /// <summary>
    /// Get bonus list
    /// </summary>
    /// <param name="brandIds">Filter by brand Id.</param>
    /// <param name="storeIds">Filter by store Id.</param>
    /// <param name="studentIds">Filter by student Id.</param>
    /// <param name="state">Filter by bonus state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet]
    [Authorize(Roles = "Admin, Brand, Store")]
    [ProducesResponseType(typeof(PagedResultModel<BonusModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<BonusModel>> GetList(
        [FromQuery] List<string> brandIds,
        [FromQuery] List<string> storeIds,
        [FromQuery] List<string> studentIds,
        [FromQuery] bool? state,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string propertySort = paging.Sort.Split(",")[0];
        var propertyInfo = typeof(Bonus).GetProperty(propertySort);
        if (propertySort != null && propertyInfo != null)
        {
            PagedResultModel<BonusModel>
                result = bonusService.GetAll
                (brandIds, storeIds, studentIds, state, propertySort, 
                paging.Sort.Split(",")[1].Equals("asc"), paging.Search, paging.Page, paging.Limit);
            return StatusCode(StatusCodes.Status200OK, result);
        }
        return StatusCode(StatusCodes.Status400BadRequest, "Invalid property of bonus");
    }

    /// <summary>
    /// Get bonus by id
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Brand, Store")]
    [ProducesResponseType(typeof(BonusExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GetById(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return StatusCode(StatusCodes.Status200OK, bonusService.GetById(id));
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }
}
