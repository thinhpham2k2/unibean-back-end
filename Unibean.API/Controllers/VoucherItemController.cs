using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.VoucherItems;
using Unibean.Service.Services.Interfaces;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("🎟️Voucher Item API")]
[Route("api/v1/voucher-items")]
public class VoucherItemController : ControllerBase
{
    private readonly IVoucherItemService voucherItemService;

    public VoucherItemController(IVoucherItemService voucherItemService)
    {
        this.voucherItemService = voucherItemService;
    }

    /// <summary>
    /// Create voucher item
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Brand")]
    [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult Create([FromBody] CreateVoucherItemModel creation)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return File(voucherItemService.Add(creation).ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Result(" + DateTime.UtcNow.ToString("R") + ").xlsx");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }
}
