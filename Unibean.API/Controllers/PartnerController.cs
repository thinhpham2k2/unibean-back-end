using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Partners;
using Unibean.Service.Models.Types;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("Partner API")]
[Route("api/v1/partners")]
public class PartnerController : ControllerBase
{
    private readonly IPartnerService partnerService;

    public PartnerController(IPartnerService partnerService)
    {
        this.partnerService = partnerService;
    }

    /// <summary>
    /// Get partner by id
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Partner, Store, Student")]
    [ProducesResponseType(typeof(PartnerExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult GetById(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return Ok(partnerService.GetById(id));
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }
}
