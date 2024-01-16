using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.Stores;
using Unibean.Service.Models.Vouchers;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Validations;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("🏪Store API")]
[Route("api/v1/stores")]
public class StoreController : ControllerBase
{
    private readonly IStoreService storeService;

    public StoreController(IStoreService storeService)
    {
        this.storeService = storeService;
    }

    /// <summary>
    /// Get store list
    /// </summary>
    /// <param name="brandIds">Filter by brand Id.</param>
    /// <param name="areaIds">Filter by area Id.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(PagedResultModel<StoreModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public ActionResult<PagedResultModel<StoreModel>> GetList(
        [FromQuery] List<string> brandIds,
        [FromQuery] List<string> areaIds,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string propertySort = paging.Sort.Split(",")[0];
        var propertyInfo = typeof(Store).GetProperty(propertySort);
        if (propertySort != null && propertyInfo != null)
        {
            PagedResultModel<StoreModel>
                result = storeService.GetAll
                (brandIds, areaIds, propertySort, paging.Sort.Split(",")[1].Equals("asc"), 
                paging.Search, paging.Page, paging.Limit);
            return Ok(result);
        }
        return BadRequest("Invalid property of store");
    }

    /// <summary>
    /// Get store by id
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(StoreExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult GetById(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return Ok(storeService.GetById(id));
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Create store
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin, Brand")]
    [ProducesResponseType(typeof(StoreModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Create([FromForm] CreateStoreModel creation)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var store = await storeService.Add(creation);
            if (store != null)
            {
                return StatusCode(StatusCodes.Status201Created, store);
            }
            return NotFound("Create fail");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Update store
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, Brand")]
    [ProducesResponseType(typeof(StoreExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Update(string id, [FromForm] UpdateStoreModel update)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var store = await storeService.Update(id, update);
            if (store != null)
            {
                return StatusCode(StatusCodes.Status200OK, store);
            }
            return NotFound("Update fail");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Delete store
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin, Brand")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult Delete(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            storeService.Delete(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Get voucher list by store id
    /// </summary>
    /// <param name="id">Store id.</param>
    /// <param name="campaignIds">Filter by campaign Id.</param>
    /// <param name="typeIds">Filter by voucher type Id.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/vouchers")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(PagedResultModel<VoucherModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public ActionResult<PagedResultModel<VoucherModel>> GetVoucherListByStoreId([ValidStore] string id,
        [FromQuery] List<string> campaignIds,
        [FromQuery] List<string> typeIds,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            string propertySort = paging.Sort.Split(",")[0];
            var propertyInfo = typeof(Voucher).GetProperty(propertySort);
            if (propertySort != null && propertyInfo != null)
            {
                PagedResultModel<VoucherModel>
                result = storeService.GetVoucherListByStoreId
                    (id, campaignIds, typeIds, propertySort, paging.Sort.Split(",")[1].Equals("asc"), 
                    paging.Search, paging.Page, paging.Limit);
                return Ok(result);
            }
            return BadRequest("Invalid property of voucher");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }
}
