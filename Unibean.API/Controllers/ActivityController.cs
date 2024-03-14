using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Activities;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Services.Interfaces;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("🧩Activity API")]
[Route("api/v1/activities")]
public class ActivityController : ControllerBase
{
    private readonly IActivityService activityService;

    public ActivityController(IActivityService activityService)
    {
        this.activityService = activityService;
    }

    /// <summary>
    /// Get activity list
    /// </summary>
    /// <param name="brandIds">Filter by brand id.</param>
    /// <param name="storeIds">Filter by store id.</param>
    /// <param name="studentIds">Filter by student id.</param>
    /// <param name="campaginIds">Filter by campagin id.</param>
    /// <param name="campaginDetailIds">Filter by campagin detail id.</param>
    /// <param name="voucherIds">Filter by voucher id.</param>
    /// <param name="voucherItemIds">Filter by voucher item id.</param>
    /// <param name="typeIds">Filter by type id.</param>
    /// <param name="state">Filter by activity state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PagedResultModel<ActivityModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<ActivityModel>> GetList(
        [FromQuery] List<string> brandIds,
        [FromQuery] List<string> storeIds,
        [FromQuery] List<string> studentIds,
        [FromQuery] List<string> campaginIds,
        [FromQuery] List<string> campaginDetailIds,
        [FromQuery] List<string> voucherIds,
        [FromQuery] List<string> voucherItemIds,
        [FromQuery] List<Type> typeIds,
        [FromQuery] bool? state,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string propertySort = paging.Sort.Split(",")[0];
        var propertyInfo = typeof(Activity).GetProperty(propertySort);
        if (propertySort != null && propertyInfo != null)
        {
            PagedResultModel<ActivityModel>
                result = activityService.GetAll
                (brandIds, storeIds, studentIds, campaginIds, campaginDetailIds, voucherIds,
                voucherItemIds, typeIds, state, propertySort, paging.Sort.Split(",")[1].Equals("asc"),
                paging.Search, paging.Page, paging.Limit);
            return StatusCode(StatusCodes.Status200OK, result);
        }
        return StatusCode(StatusCodes.Status400BadRequest, "Thuộc tính không hợp lệ của hoạt động");
    }

    /// <summary>
    /// Get activity by id
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ActivityExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GetById(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return StatusCode(StatusCodes.Status200OK, activityService.GetById(id));
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }
}
