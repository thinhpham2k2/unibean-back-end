using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.Requests;
using Unibean.Service.Services.Interfaces;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("💸Request API")]
[Route("api/v1/requests")]
public class RequestController : ControllerBase
{
    private readonly IRequestService requestService;

    public RequestController(IRequestService requestService)
    {
        this.requestService = requestService;
    }

    /// <summary>
    /// Get request list
    /// </summary>
    /// <param name="brandIds">Filter by brand Id.</param>
    /// <param name="adminIds">Filter by admin Id.</param>
    /// <param name="state">Filter by request state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet]
    [Authorize(Roles = "Admin, Brand, Store")]
    [ProducesResponseType(typeof(PagedResultModel<RequestModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public ActionResult<PagedResultModel<RequestModel>> GetList(
        [FromQuery] List<string> brandIds,
        [FromQuery] List<string> adminIds,
        [FromQuery] bool? state,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string propertySort = paging.Sort.Split(",")[0];
        var propertyInfo = typeof(Request).GetProperty(propertySort);
        if (propertySort != null && propertyInfo != null)
        {
            PagedResultModel<RequestModel>
                result = requestService.GetAll
                (brandIds, adminIds, state, propertySort, paging.Sort.Split(",")[1].Equals("asc"), 
                paging.Search, paging.Page, paging.Limit);
            return Ok(result);
        }
        return BadRequest("Invalid property of request");
    }

    /// <summary>
    /// Get request by id
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Brand, Store")]
    [ProducesResponseType(typeof(RequestExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult GetById(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return Ok(requestService.GetById(id));
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }
}
