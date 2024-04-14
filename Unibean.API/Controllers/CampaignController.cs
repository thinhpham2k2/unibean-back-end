using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Activities;
using Unibean.Service.Models.CampaignActivities;
using Unibean.Service.Models.CampaignDetails;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.Campuses;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Majors;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.Stores;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Validations;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("📢Campaign API")]
[Route("api/v1/campaigns")]
public class CampaignController : ControllerBase
{
    private readonly ICampaignService campaignService;

    private readonly IJwtService jwtService;

    public CampaignController(
        ICampaignService campaignService,
        IJwtService jwtService)
    {
        this.campaignService = campaignService;
        this.jwtService = jwtService;
    }

    /// <summary>
    /// Get campaign list
    /// </summary>
    /// <param name="brandIds">Filter by brand id.</param>
    /// <param name="typeIds">Filter by campaign type id.</param>
    /// <param name="storeIds">Filter by store id.</param>
    /// <param name="majorIds">Filter by major id.</param>
    /// <param name="campusIds">Filter by campus id.</param>
    /// <param name="stateIds">Filter by campaign state --- Pending = 1, Rejected = 2, Active = 3, Inactive = 4, Finished = 5, Closed = 6, Cancelled = 7</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResultModel<CampaignModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<CampaignModel>> GetList(
        [FromQuery] List<string> brandIds,
        [FromQuery] List<string> typeIds,
        [FromQuery] List<string> storeIds,
        [FromQuery] List<string> majorIds,
        [FromQuery] List<string> campusIds,
        [FromQuery] List<CampaignState> stateIds,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string propertySort = paging.Sort.Split(",")[0];
        var propertyInfo = typeof(Campaign).GetProperty(propertySort);
        if (propertySort != null && propertyInfo != null)
        {
            PagedResultModel<CampaignModel>
                result = campaignService.GetAll
                (brandIds, typeIds, storeIds, majorIds, campusIds, stateIds, propertySort,
                paging.Sort.Split(",")[1].Equals("asc"), paging.Search, paging.Page, paging.Limit);
            return StatusCode(StatusCodes.Status200OK, result);
        }
        return StatusCode(StatusCodes.Status400BadRequest, "Thuộc tính của chiến dịch không hợp lệ");
    }

    /// <summary>
    /// Get campaign by id
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(CampaignExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GetById(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return StatusCode(StatusCodes.Status200OK, campaignService.GetById(id));
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Create campaign
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin, Brand")]
    [ProducesResponseType(typeof(CampaignExtraModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> Create([FromForm] CreateCampaignModel creation)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var campaign = await campaignService.Add(creation);
            if (campaign != null)
            {
                return StatusCode(StatusCodes.Status201Created, campaign);
            }
            return StatusCode(StatusCodes.Status404NotFound, "Tạo thất bại");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Update campaign
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, Brand")]
    [ProducesResponseType(typeof(CampaignExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> Update(string id, [FromForm] UpdateCampaignModel update)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var campaign = await campaignService.Update(id, update);
            if (campaign != null)
            {
                return StatusCode(StatusCodes.Status200OK, campaign);
            }
            return StatusCode(StatusCodes.Status404NotFound, "Cập nhật thất bại");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Update campaign state
    /// </summary>
    /// <param name="id">Campaign id.</param>
    /// <param name="stateId">Campaign state id --- Rejected = 2, Active = 3, Inactive = 4, Closed = 6, Cancelled = 7</param>
    /// <param name="note">Note for state</param>
    [HttpPut("{id}/states/{stateId}")]
    [Authorize(Roles = "Admin, Brand")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult UpdateState(
        [ValidCampaign(new[] {
            CampaignState.Pending,
            CampaignState.Rejected,
            CampaignState.Active,
            CampaignState.Inactive })] string id,
        CampaignState stateId,
        [FromBody] string note)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string jwtToken = HttpContext.Request.Headers["Authorization"];

        try
        {
            if (campaignService.UpdateState
                (id, stateId, note, jwtService.GetJwtRequest(jwtToken.Split(" ")[1])))
            {
                return StatusCode(StatusCodes.Status200OK, "Cập nhật trạng thái thành công");
            }
            return NotFound("Cập nhật trạng thái thất bại");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Delete campaign
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin, Brand")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult Delete(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            campaignService.Delete(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Get activity list by campaign id
    /// </summary>
    /// <param name="id">Campaign id.</param>
    /// <param name="stateIds">Filter by campaign state --- Pending = 1, Rejected = 2, Active = 3, Inactive = 4, Finished = 5, Closed = 6, Cancelled = 7</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/activities")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(PagedResultModel<CampaignActivityModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<CampaignActivityModel>> GetActivityListByCampaignId(
        string id,
        [FromQuery] List<CampaignState> stateIds,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            string propertySort = paging.Sort.Split(",")[0];
            var propertyInfo = typeof(CampaignActivity).GetProperty(propertySort);
            if (propertySort != null && propertyInfo != null)
            {
                PagedResultModel<CampaignActivityModel>
                result = campaignService.GetCampaignActivityListByCampaignId
                    (id, stateIds, propertySort, paging.Sort.Split(",")[1].Equals("asc"),
                    paging.Search, paging.Page, paging.Limit);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Thuộc tính không hợp lệ của hoạt động chiến dịch");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Get campus list by campaign id
    /// </summary>
    /// <param name="id">Campaign id.</param>
    /// <param name="universityIds">Filter by university id.</param>
    /// <param name="areaIds">Filter by area id.</param>
    /// <param name="state">Filter by campus state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/campuses")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(PagedResultModel<CampusModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<CampusModel>> GetCampusListByCampaignId(
        string id,
        [FromQuery] List<string> universityIds,
        [FromQuery] List<string> areaIds,
        [FromQuery] bool? state,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            string propertySort = paging.Sort.Split(",")[0];
            var propertyInfo = typeof(Campus).GetProperty(propertySort);
            if (propertySort != null && propertyInfo != null)
            {
                PagedResultModel<CampusModel>
                result = campaignService.GetCampusListByCampaignId
                    (id, universityIds, areaIds, state, propertySort,
                    paging.Sort.Split(",")[1].Equals("asc"), paging.Search, paging.Page, paging.Limit);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Thuộc tính không hợp lệ của cơ sở");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Get major list by campaign id
    /// </summary>
    /// <param name="id">Campaign id.</param>
    /// <param name="state">Filter by major state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/majors")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(PagedResultModel<MajorModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<MajorModel>> GetMajorListByCampaignId(string id,
        [FromQuery] bool? state,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            string propertySort = paging.Sort.Split(",")[0];
            var propertyInfo = typeof(Major).GetProperty(propertySort);
            if (propertySort != null && propertyInfo != null)
            {
                PagedResultModel<MajorModel>
                result = campaignService.GetMajorListByCampaignId
                    (id, state, propertySort, paging.Sort.Split(",")[1].Equals("asc"),
                    paging.Search, paging.Page, paging.Limit);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Thuộc tính không hợp lệ của chuyên ngành");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Get store list by campaign id
    /// </summary>
    /// <param name="id">Campaign id.</param>
    /// <param name="brandIds">Filter by brand id.</param>
    /// <param name="areaIds">Filter by area id.</param>
    /// <param name="state">Filter by store state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/stores")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(PagedResultModel<StoreModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<StoreModel>> GetStoreListByCampaignId(string id,
        [FromQuery] List<string> brandIds,
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
                result = campaignService.GetStoreListByCampaignId
                    (id, brandIds, areaIds, state, propertySort, paging.Sort.Split(",")[1].Equals("asc"),
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
    /// Get campaign detail list by campaign id
    /// </summary>
    /// <param name="id">Campaign id.</param>
    /// <param name="typeIds">Filter by voucher type id.</param>
    /// <param name="state">Filter by campaign detail state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/details")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(PagedResultModel<CampaignDetailModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<CampaignDetailModel>> GetCampaignDetailListByCampaignId(
        string id,
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
                PagedResultModel<CampaignDetailModel>
                result = campaignService.GetCampaignDetailListByCampaignId
                    (id, typeIds, state, propertySort, paging.Sort.Split(",")[1].Equals("asc"),
                    paging.Search, paging.Page, paging.Limit);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Thuộc tính không hợp lệ của chi tiết chiến dịch");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Get campaign detail by campaign id
    /// </summary>
    /// <param name="id">Campaign id.</param>
    /// <param name="detailId">Campaign detail id.</param>
    [HttpGet("{id}/details/{detailId}")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(CampaignDetailExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<CampaignDetailExtraModel> GetCampaignDetailById(string id, string detailId)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return StatusCode(StatusCodes.Status200OK, campaignService.GetCampaignDetailById(id, detailId));
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Redeem voucher
    /// </summary>
    /// <param name="id">Campaign id.</param>
    /// <param name="detailId">Campaign detail id.</param>
    /// <param name="creation">Buy activities model.</param>
    [HttpPost("{id}/details/{detailId}")]
    [Authorize(Roles = "Student")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<string> BuyVoucher(
        [ValidCampaign(new[] { CampaignState.Active })] string id,
        [ValidCampaignDetailId] string detailId,
        CreateBuyActivityModel creation)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return campaignService.AddActivity(id, detailId, creation) ?
                StatusCode(StatusCodes.Status201Created, "Thanh toán thành công") :
                StatusCode(StatusCodes.Status404NotFound, "Thanh toán khuyến mãi thất bại");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }
}
