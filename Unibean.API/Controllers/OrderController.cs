using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Orders;
using Unibean.Service.Models.OrderStates;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Services.Interfaces;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("🛒Order API")]
[Route("api/v1/orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderService orderService;

    private readonly IOrderStateService orderStateService;

    public OrderController(IOrderService orderService,
        IOrderStateService orderStateService)
    {
        this.orderService = orderService;
        this.orderStateService = orderStateService;
    }

    /// <summary>
    /// Get order list
    /// </summary>
    /// <param name="stationIds">Filter by station id.</param>
    /// <param name="studentIds">Filter by student id.</param>
    /// <param name="stateIds">Filter by state id --- Order = 1, Confirmation = 2, Preparation = 3, Arrival = 4, Receipt = 5, Abort = 6</param>
    /// <param name="state">Filter by order state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet]
    [Authorize(Roles = "Admin, Staff")]
    [ProducesResponseType(typeof(PagedResultModel<OrderModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<OrderModel>> GetList(
        [FromQuery] List<string> stationIds,
        [FromQuery] List<string> studentIds,
        [FromQuery] List<State> stateIds,
        [FromQuery] bool? state,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string propertySort = paging.Sort.Split(",")[0];
        var propertyInfo = typeof(Order).GetProperty(propertySort);
        if (propertySort != null && propertyInfo != null || propertySort.Equals("StateCurrent"))
        {
            PagedResultModel<OrderModel>
                result = orderService.GetAll
                (stationIds, studentIds, stateIds, state, propertySort.Equals("StateCurrent")
                ? "OrderStates.Max(s => s.StateId)" : propertySort, paging.Sort.Split(",")[1].Equals("asc"),
                paging.Search, paging.Page, paging.Limit);
            return StatusCode(StatusCodes.Status200OK, result);
        }
        return StatusCode(StatusCodes.Status400BadRequest, "Invalid property of order");
    }

    /// <summary>
    /// Get order by id
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Staff")]
    [ProducesResponseType(typeof(OrderExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GetById(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return StatusCode(StatusCodes.Status200OK, orderService.GetById(id));
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Create state for order
    /// </summary>
    /// <param name="id">Order id.</param>
    /// <param name="create">State id --- Order = 1, Confirmation = 2, Preparation = 3, Arrival = 4, Receipt = 5, Abort = 6</param>
    [HttpPost("{id}/states")]
    [Authorize(Roles = "Admin, Staff")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult CreateStateForOrder(string id, [FromBody] CreateOrderStateModel create)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return StatusCode(StatusCodes.Status201Created, orderStateService.Add(id, create));
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }
}
