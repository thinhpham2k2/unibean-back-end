using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.Campuses;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Majors;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.Stores;
using Unibean.Service.Models.Vouchers;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Validations;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("📢Campaign API")]
[Route("api/v1/campaigns")]
public class CampaignController : ControllerBase
{
    private readonly ICampaignService campaignService;

    public CampaignController(ICampaignService campaignService)
    {
        this.campaignService = campaignService;
    }

    /// <summary>
    /// Get campaign list
    /// </summary>
    /// <param name="brandIds">Filter by brand Id.</param>
    /// <param name="typeIds">Filter by campaign type Id.</param>
    /// <param name="storeIds">Filter by store Id.</param>
    /// <param name="majorIds">Filter by major Id.</param>
    /// <param name="campusIds">Filter by campus Id.</param>
    /// <param name="state">Filter by campaign state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(PagedResultModel<CampaignModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public ActionResult<PagedResultModel<CampaignModel>> GetList(
        [FromQuery] List<string> brandIds,
        [FromQuery] List<string> typeIds,
        [FromQuery] List<string> storeIds,
        [FromQuery] List<string> majorIds,
        [FromQuery] List<string> campusIds,
        [FromQuery] bool? state,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string propertySort = paging.Sort.Split(",")[0];
        var propertyInfo = typeof(Campaign).GetProperty(propertySort);
        if (propertySort != null && propertyInfo != null)
        {
            PagedResultModel<CampaignModel>
                result = campaignService.GetAll
                (brandIds, typeIds, storeIds, majorIds, campusIds, state, propertySort,
                paging.Sort.Split(",")[1].Equals("asc"), paging.Search, paging.Page, paging.Limit);
            return Ok(result);
        }
        return BadRequest("Invalid property of campaign");
    }

    /// <summary>
    /// Get campaign by id
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(CampaignModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult GetById(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return Ok(campaignService.GetById(id));
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Create campaign
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin, Brand")]
    [ProducesResponseType(typeof(CampaignModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
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
            return NotFound("Create fail");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Update campaign
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, Brand")]
    [ProducesResponseType(typeof(CampaignModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
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
            return NotFound("Update fail");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Update campaign state
    /// </summary>
    [HttpPut("{id}/state")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CampaignExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public ActionResult UpdateState(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var campaign = campaignService.UpdateState(id);
            if (campaign != null)
            {
                return StatusCode(StatusCodes.Status200OK, campaign);
            }
            return NotFound("Update state fail");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Delete campaign
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
            campaignService.Delete(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Get campus list by campaign id
    /// </summary>
    /// <param name="id">Campaign id.</param>
    /// <param name="universityIds">Filter by university Id.</param>
    /// <param name="areaIds">Filter by area Id.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/campuses")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(PagedResultModel<CampusModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public ActionResult<PagedResultModel<CampusModel>> GetCampusListByStoreId([ValidCampaign] string id,
        [FromQuery] List<string> universityIds,
        [FromQuery] List<string> areaIds,
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
                    (id, universityIds, areaIds, propertySort, paging.Sort.Split(",")[1].Equals("asc"),
                    paging.Search, paging.Page, paging.Limit);
                return Ok(result);
            }
            return BadRequest("Invalid property of campus");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Get major list by campaign id
    /// </summary>
    /// <param name="id">Campaign id.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/majors")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(PagedResultModel<MajorModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public ActionResult<PagedResultModel<MajorModel>> GetMajorListByStoreId([ValidCampaign] string id,
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
                    (id, propertySort, paging.Sort.Split(",")[1].Equals("asc"),
                    paging.Search, paging.Page, paging.Limit);
                return Ok(result);
            }
            return BadRequest("Invalid property of major");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Get store list by campaign id
    /// </summary>
    /// <param name="id">Campaign id.</param>
    /// <param name="brandIds">Filter by brand Id.</param>
    /// <param name="areaIds">Filter by area Id.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/stores")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(PagedResultModel<StoreModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public ActionResult<PagedResultModel<StoreModel>> GetStoreListByStoreId([ValidCampaign] string id,
        [FromQuery] List<string> brandIds,
        [FromQuery] List<string> areaIds,
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
                    (id, brandIds, areaIds, propertySort, paging.Sort.Split(",")[1].Equals("asc"),
                    paging.Search, paging.Page, paging.Limit);
                return Ok(result);
            }
            return BadRequest("Invalid property of store");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Get voucher list by campaign id
    /// </summary>
    /// <param name="id">Campaign id.</param>
    /// <param name="typeIds">Filter by voucher type Id.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/vouchers")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(PagedResultModel<VoucherModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public ActionResult<PagedResultModel<VoucherModel>> GetVoucherListByStoreId([ValidCampaign] string id,
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
                result = campaignService.GetVoucherListByCampaignId
                    (id, typeIds, propertySort, paging.Sort.Split(",")[1].Equals("asc"),
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
