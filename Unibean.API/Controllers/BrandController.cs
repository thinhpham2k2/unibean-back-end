using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Brands;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.Stores;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Models.Vouchers;
using Unibean.Service.Services.Interfaces;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("🤝🏼Brand API")]
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
    /// <param name="state">Filter by brand state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(PagedResultModel<BrandModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<BrandModel>> GetList(
        [FromQuery] bool? state,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string jwtToken = HttpContext.Request.Headers["Authorization"];

        string propertySort = paging.Sort.Split(",")[0];
        var propertyInfo = typeof(Brand).GetProperty(propertySort);
        if (propertySort != null && propertyInfo != null)
        {
            PagedResultModel<BrandModel>
                result = brandService.GetAll
                (state, propertySort, paging.Sort.Split(",")[1].Equals("asc"), paging.Search, 
                paging.Page, paging.Limit, jwtService.GetJwtRequest(jwtToken.Split(" ")[1]));
            return StatusCode(StatusCodes.Status200OK, result);
        }
        return StatusCode(StatusCodes.Status400BadRequest, "Thuộc tính không hợp lệ của thương hiệu");
    }

    /// <summary>
    /// Get brand by id
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(BrandExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GetById(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string jwtToken = HttpContext.Request.Headers["Authorization"];

        try
        {
            return StatusCode(StatusCodes.Status200OK, brandService.GetById(id, jwtService.GetJwtRequest(jwtToken.Split(" ")[1])));
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Create brand
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(BrandModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> Create([FromForm] CreateBrandModel creation)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var brand = await brandService.Add(creation);
            if (brand != null)
            {
                return StatusCode(StatusCodes.Status201Created, brand);
            }
            return StatusCode(StatusCodes.Status404NotFound, "Tạo thất bại");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Update brand
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(BrandExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> Update(string id, [FromForm] UpdateBrandModel update)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var brand = await brandService.Update(id, update);
            if (brand != null)
            {
                return StatusCode(StatusCodes.Status200OK, brand);
            }
            return StatusCode(StatusCodes.Status404NotFound, "Cập nhật thất bại");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Delete brand
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
            brandService.Delete(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Get campaign list by brand id
    /// </summary>
    /// <param name="id">Brand id.</param>
    /// <param name="typeIds">Filter by campaign type Id.</param>
    /// <param name="storeIds">Filter by store Id.</param>
    /// <param name="majorIds">Filter by major Id.</param>
    /// <param name="campusIds">Filter by campus Id.</param>
    /// <param name="state">Filter by campaign state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/campaigns")]
    [Authorize(Roles = "Admin, Brand")]
    [ProducesResponseType(typeof(PagedResultModel<CampaignModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<CampaignModel>> GetCampaignListByBrandId(string id,
        [FromQuery] List<string> typeIds,
        [FromQuery] List<string> storeIds,
        [FromQuery] List<string> majorIds,
        [FromQuery] List<string> campusIds,
        [FromQuery] bool? state,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            string propertySort = paging.Sort.Split(",")[0];
            var propertyInfo = typeof(Campaign).GetProperty(propertySort);
            if (propertySort != null && propertyInfo != null)
            {
                PagedResultModel<CampaignModel>
                result = brandService.GetCampaignListByBrandId
                    (id, typeIds, storeIds, majorIds, campusIds, state, propertySort,
                    paging.Sort.Split(",")[1].Equals("asc"), paging.Search, paging.Page, paging.Limit);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Thuộc tính của chiến dịch không hợp lệ");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Get history transaction by brand id
    /// </summary>
    /// <param name="id">Brand id.</param>
    /// <param name="walletTypeIds">Filter by wallet type Id.</param>
    /// <param name="state">Filter by history transaction state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/histories")]
    [Authorize(Roles = "Admin, Brand")]
    [ProducesResponseType(typeof(PagedResultModel<TransactionModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<TransactionModel>> GetHistoryTransactionByStudentId(string id,
        [FromQuery] List<string> walletTypeIds,
        [FromQuery] bool? state,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            string propertySort = paging.Sort.Split(",")[0];
            var propertyInfo = typeof(TransactionModel).GetProperty(propertySort);
            if (propertySort != null && propertyInfo != null)
            {
                PagedResultModel<TransactionModel>
                result = brandService.GetHistoryTransactionListByStudentId
                    (id, walletTypeIds, state, propertySort, paging.Sort.Split(",")[1].Equals("asc"), 
                    paging.Search, paging.Page, paging.Limit);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Thuộc tính không hợp lệ của lịch sử giao dịch");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Get store list by brand id
    /// </summary>
    /// <param name="id">Brand id.</param>
    /// <param name="areaIds">Filter by area Id.</param>
    /// <param name="state">Filter by store state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/stores")]
    [Authorize(Roles = "Admin, Brand")]
    [ProducesResponseType(typeof(PagedResultModel<StoreModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<StoreModel>> GetStoreListByBrandId(string id,
        [FromQuery] List<string> areaIds,
        [FromQuery] bool? state,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            string propertySort = paging.Sort.Split(",")[0];
            var propertyInfo = typeof(Store).GetProperty(propertySort);
            if (propertySort != null && propertyInfo != null)
            {
                PagedResultModel<StoreModel>
                result = brandService.GetStoreListByBrandId
                    (id, areaIds, state, propertySort, paging.Sort.Split(",")[1].Equals("asc"), 
                    paging.Search, paging.Page, paging.Limit);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Thuộc tính không hợp lệ của cửa hàng");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Get voucher list by brand id
    /// </summary>
    /// <param name="id">Brand id.</param>
    /// <param name="typeIds">Filter by voucher type Id.</param>
    /// <param name="state">Filter by voucher state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/vouchers")]
    [Authorize(Roles = "Admin, Brand")]
    [ProducesResponseType(typeof(PagedResultModel<VoucherModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<VoucherModel>> GetVoucherListByBrandId(string id,
        [FromQuery] List<string> typeIds,
        [FromQuery] bool? state,
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
                result = brandService.GetVoucherListByBrandId
                    (id, typeIds, state, propertySort, paging.Sort.Split(",")[1].Equals("asc"), 
                    paging.Search, paging.Page, paging.Limit);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Thuộc tính không hợp lệ của khuyến mãi");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }
}
