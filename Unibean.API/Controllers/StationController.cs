using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Models.Stations;
using Unibean.Service.Models.Exceptions;
using Unibean.Repository.Entities;
using Unibean.Service.Models.Orders;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("🏣Station API")]
[Route("api/v1/stations")]
public class StationController : ControllerBase
{
    private readonly IStationService stationService;

    public StationController(IStationService stationService)
    {
        this.stationService = stationService;
    }

    /// <summary>
    /// Get station list
    /// </summary>
    /// <param name="state">Filter by station state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(PagedResultModel<StationModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<StationModel>> GetList(
        [FromQuery] bool? state,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string propertySort = paging.Sort.Split(",")[0];
        var propertyInfo = typeof(Station).GetProperty(propertySort);
        if (propertySort != null && propertyInfo != null)
        {
            PagedResultModel<StationModel>
                result = stationService.GetAll
                (state, propertySort, paging.Sort.Split(",")[1].Equals("asc"), 
                paging.Search, paging.Page, paging.Limit);
            return Ok(result);
        }
        return BadRequest("Thuộc tính không hợp lệ của trạm");
    }

    /// <summary>
    /// Get station by id
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(StationModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GetById(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return Ok(stationService.GetById(id));
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Create station
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(StationModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> Create([FromForm] CreateStationModel creation)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var station = await stationService.Add(creation);
            if (station != null)
            {
                return StatusCode(StatusCodes.Status201Created, station);
            }
            return NotFound("Tạo thất bại");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Update station
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(StationModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> Update(string id, [FromForm] UpdateStationModel update)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var station = await stationService.Update(id, update);
            if (station != null)
            {
                return StatusCode(StatusCodes.Status200OK, station);
            }
            return NotFound("Cập nhật thất bại");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Delete station
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult Delete(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            stationService.Delete(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Get order list by station id
    /// </summary>
    /// <param name="id">Station id.</param>
    /// <param name="studentIds">Filter by student Id.</param>
    /// <param name="stateIds">Filter by state Id.</param>
    /// <param name="state">Filter by order state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/orders")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PagedResultModel<OrderModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public ActionResult<PagedResultModel<OrderModel>> GetOrderListByStudentId(string id,
        [FromQuery] List<string> studentIds,
        [FromQuery] List<string> stateIds,
        [FromQuery] bool? state,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            string propertySort = paging.Sort.Split(",")[0];
            var propertyInfo = typeof(Order).GetProperty(propertySort);
            if (propertySort != null && propertyInfo != null || propertySort.Equals("StateCurrent"))
            {
                PagedResultModel<OrderModel>
                result = stationService.GetOrderListByStudentId
                    (id, studentIds, stateIds, state, propertySort.Equals("StateCurrent")
                        ? "OrderStates.Max(s => s.StateId)" : propertySort, paging.Sort.Split(",")[1].Equals("asc"), 
                        paging.Search, paging.Page, paging.Limit);
                return Ok(result);
            }
            return BadRequest("Thuộc tính của đơn đặt hàng không hợp lệ");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }
}
