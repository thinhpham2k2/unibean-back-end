using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Admins;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.Requests;
using Unibean.Service.Models.WebHooks;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Validations;
using static Unibean.Service.Models.WebHooks.DiscordWebhookModel;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("👨🏻‍✈️Admin API")]
[Route("api/v1/admins")]
public class AdminController : ControllerBase
{
    private readonly IAdminService adminService;

    private readonly IRequestService requestService;

    private readonly IFireBaseService fireBaseService;

    private readonly IDiscordService discordService;

    public AdminController(IAdminService adminService,
        IRequestService requestService,
        IFireBaseService fireBaseService,
        IDiscordService discordService)
    {
        this.adminService = adminService;
        this.requestService = requestService;
        this.fireBaseService = fireBaseService;
        this.discordService = discordService;
    }

    /// <summary>
    /// Get admin list
    /// </summary>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PagedResultModel<AdminModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public ActionResult<PagedResultModel<AdminModel>> GetList(
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string propertySort = paging.Sort.Split(",")[0];
        var propertyInfo = typeof(Admin).GetProperty(propertySort);
        if (propertySort != null && propertyInfo != null)
        {
            PagedResultModel<AdminModel>
                result = adminService.GetAll
                (propertySort, paging.Sort.Split(",")[1].Equals("asc"), paging.Search, paging.Page, paging.Limit);
            return Ok(result);
        }
        return BadRequest("Invalid property of admin");
    }

    /// <summary>
    /// Get admin by id
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(AdminModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult GetById(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return Ok(adminService.GetById(id));
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Create admin
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(AdminModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Create([FromForm] CreateAdminModel creation)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var admin = await adminService.Add(creation);
            if (admin != null)
            {
                return StatusCode(StatusCodes.Status201Created, admin);
            }
            return NotFound("Create fail");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Update admin
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(AdminModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Update(string id, [FromForm] UpdateAdminModel update)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var admin = await adminService.Update(id, update);
            if (admin != null)
            {
                return StatusCode(StatusCodes.Status200OK, admin);
            }
            return NotFound("Update fail");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Delete admin
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult Delete(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            adminService.Delete(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Create request
    /// </summary>
    [HttpPost("{id}/requests")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(RequestModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult CreateRequest([ValidAdmin] string id, [FromBody] CreateRequestModel creation)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var request = requestService.Add(id, creation);
            if (request != null)
            {
                return StatusCode(StatusCodes.Status201Created, request);
            }
            return NotFound("Create fail");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Push noification to topic
    /// </summary>
    [HttpPost("notification")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult PushNoification([FromBody] string topic)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return StatusCode(StatusCodes.Status201Created, fireBaseService.PushNotificationToTopic(topic));
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }
}
