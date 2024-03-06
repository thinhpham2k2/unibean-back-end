using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Brands;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.Charts;
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

    private readonly IChartService chartService;

    public BrandController(
        IBrandService brandService,
        IJwtService jwtService,
        IChartService chartService)
    {
        this.brandService = brandService;
        this.jwtService = jwtService;
        this.chartService = chartService;
    }

    /// <summary>
    /// Get brand list
    /// </summary>
    /// <param name="state">Filter by brand state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResultModel<BrandModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
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
                paging.Page, paging.Limit, jwtService.GetJwtRequest(jwtToken?.Split(" ")[1]));
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
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
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
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
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
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
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
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
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
    /// <param name="typeIds">Filter by campaign type id.</param>
    /// <param name="storeIds">Filter by store id.</param>
    /// <param name="majorIds">Filter by major id.</param>
    /// <param name="campusIds">Filter by campus id.</param>
    /// <param name="stateIds">Filter by campaign state --- Pending = 1, Rejected = 2, Active = 3, Inactive = 4, Finished = 5, Closed = 6, Cancelled = 7</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/campaigns")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResultModel<CampaignModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<CampaignModel>> GetCampaignListByBrandId(string id,
        [FromQuery] List<string> typeIds,
        [FromQuery] List<string> storeIds,
        [FromQuery] List<string> majorIds,
        [FromQuery] List<string> campusIds,
        [FromQuery] List<CampaignState> stateIds,
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
                    (id, typeIds, storeIds, majorIds, campusIds, stateIds, propertySort,
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
    /// Get column chart by brand id
    /// </summary>
    [HttpGet("{id}/column-chart")]
    [Authorize(Roles = "Admin, Brand")]
    [ProducesResponseType(typeof(List<ColumnChartModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GetColumnChartByBrandId(
        string id,
        [FromQuery] DateOnly fromDate,
        [FromQuery] DateOnly toDate,
        [FromQuery] bool? isAsc)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return StatusCode(StatusCodes.Status200OK, chartService.GetColumnChart(id, fromDate, toDate, isAsc, Role.Brand));
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
    /// <param name="walletTypeIds">Filter by wallet type id --- Green = 1, Red = 2</param>
    /// <param name="state">Filter by history transaction state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/histories")]
    [Authorize(Roles = "Admin, Brand")]
    [ProducesResponseType(typeof(PagedResultModel<TransactionModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<TransactionModel>> GetHistoryTransactionByStudentId(string id,
        [FromQuery] List<WalletType> walletTypeIds,
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
    /// Get line chart by brand id
    /// </summary>
    [HttpGet("{id}/line-chart")]
    [Authorize(Roles = "Admin, Brand")]
    [ProducesResponseType(typeof(List<LineChartModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GetLineChartByBrandId(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return StatusCode(StatusCodes.Status200OK, chartService.GetLineChart(id, Role.Brand));
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
    /// <param name="areaIds">Filter by area id.</param>
    /// <param name="state">Filter by store state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/stores")]
    [Authorize(Roles = "Admin, Brand")]
    [ProducesResponseType(typeof(PagedResultModel<StoreModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
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
    /// Get title by brand id
    /// </summary>
    [HttpGet("{id}/title")]
    [Authorize(Roles = "Admin, Brand")]
    [ProducesResponseType(typeof(TitleBrandModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GetTitleByBrandId(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return StatusCode(StatusCodes.Status200OK, chartService.GetTitleBrand(id));
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
    /// <param name="typeIds">Filter by voucher type id.</param>
    /// <param name="state">Filter by voucher state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/vouchers")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResultModel<VoucherModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
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
