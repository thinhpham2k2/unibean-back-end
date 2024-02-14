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
    /// <param name="state">Filter by admin state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PagedResultModel<AdminModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<AdminModel>> GetList(
        [FromQuery] bool? state,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string propertySort = paging.Sort.Split(",")[0];
        var propertyInfo = typeof(Admin).GetProperty(propertySort);
        if (propertySort != null && propertyInfo != null)
        {
            PagedResultModel<AdminModel>
                result = adminService.GetAll
                (state, propertySort, paging.Sort.Split(",")[1].Equals("asc"), 
                paging.Search, paging.Page, paging.Limit);
            return StatusCode(StatusCodes.Status200OK, result);
        }
        return StatusCode(StatusCodes.Status400BadRequest, "Thuộc tính không hợp lệ của quản trị viên");
    }

    /// <summary>
    /// Get admin by id
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(AdminModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GetById(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return StatusCode(StatusCodes.Status200OK, adminService.GetById(id));
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Create admin
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(AdminModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
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
            return StatusCode(StatusCodes.Status404NotFound, "Tạo thất bại");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Update admin
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(AdminModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
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
            return StatusCode(StatusCodes.Status404NotFound, "Cập nhật thất bại");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Delete admin
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
            adminService.Delete(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Create request
    /// </summary>
    [HttpPost("{id}/requests")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(RequestModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
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
            return StatusCode(StatusCodes.Status404NotFound, "Tạo thất bại");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Push noification to topic
    /// </summary>
    [HttpPost("notification")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult PushNoification([FromBody] string topic)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return StatusCode(StatusCodes.Status201Created, 
                fireBaseService.PushNotificationToTopic(topic));
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Push noification to discord
    /// </summary>
    [HttpPost("discord")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult PushDiscord()
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            discordService.CreateWebHooks(new DiscordWebhookModel
            {
                Embeds = new() {
                    new() {
                        Author = new()
                        {
                            Name = "Test",
                            Url = "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/accounts%2Fbrand-image.png?alt=media&token=91104a43-2e4a-4c6f-bef6-ba7e40660022",
                            IconUrl = "Test"
                        },

                        Fields = new()
                        {
                            new()
                            {
                                Name = "📢 Chiến dịch mới",
                                Value = "Test"
                            },
                            new()
                            {
                                Name = "🆔 chiến dịch mới",
                                Value = "||" + "Test" + "||"
                            },
                            new()
                            {
                                Name = "💸 Tổng chi phí",
                                Value = "Test" + " đậu"
                            },
                            new()
                            {
                                Name = "▶️ Bắt đầu",
                                Value = "Test"
                            },
                            new()
                            {
                                Name = "⏸️ Kết thúc",
                                Value = "Test"
                            },
                        },

                        Image = new()
                        {
                            Url = "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/campaigns%2F01HNHBV2BHFN0R5TQ6KP87H19G?alt=media&token=60a1dc39-1d86-4aab-9df5-95d23094cefa"
                        },

                        Footer = new()
                        {
                             Text = "Test"
                        },
                    }
                },
            });
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }
}
