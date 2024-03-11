using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Validations;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("✔️Validation API")]
[Route("api/v1/validations")]
public class ValidationController : ControllerBase
{
    /// <summary>
    /// Brand id validation
    /// </summary>
    [AllowAnonymous]
    [HttpPost("brand-id")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult BrandIdValidation([FromBody] BrandIdModel id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        return StatusCode(StatusCodes.Status200OK, id.BrandId + " is valid");
    }

    /// <summary>
    /// Cost and campaign details  validation
    /// </summary>
    [AllowAnonymous]
    [HttpPost("campaign-cd")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult CampaignCDValidation([FromBody] CampaignCDModel cd)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        return StatusCode(StatusCodes.Status200OK, cd.ToString() + " is valid");
    }

    /// <summary>
    /// Campaign majors, stores, campuses  validation
    /// </summary>
    [AllowAnonymous]
    [HttpPost("campaign-msc")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult CampaignMSCValidation([FromBody] CampaignMSCModel msc)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        return StatusCode(StatusCodes.Status200OK, msc.ToString() + " is valid");
    }

    /// <summary>
    /// Code validation
    /// </summary>
    [AllowAnonymous]
    [HttpPost("code")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult CodeValidation([FromBody] CodeModel code)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        return StatusCode(StatusCodes.Status200OK, code.Code + " is valid");
    }

    /// <summary>
    /// Email validation
    /// </summary>
    [AllowAnonymous]
    [HttpPost("email")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult EmailValidation([FromBody] EmailModel email)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        return StatusCode(StatusCodes.Status200OK, email.Email + " is valid");
    }

    /// <summary>
    /// Invite code validation
    /// </summary>
    [AllowAnonymous]
    [HttpPost("invite-code")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult InviteCodeValidation([FromBody] InviteCodeModel inviteCode)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        return StatusCode(StatusCodes.Status200OK, inviteCode.InviteCode + " is valid");
    }

    /// <summary>
    /// Phone validation
    /// </summary>
    [AllowAnonymous]
    [HttpPost("phone")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult PhoneValidation([FromBody] PhoneModel phone)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        return StatusCode(StatusCodes.Status200OK, phone.Phone + " is valid");
    }

    /// <summary>
    /// Time validation
    /// </summary>
    [AllowAnonymous]
    [HttpPost("time")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult TimeValidation([FromBody] TimeModel time)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        return StatusCode(StatusCodes.Status200OK, time.StartOn + " & " + time.EndOn + " is valid");
    }

    /// <summary>
    /// Type id validation
    /// </summary>
    [AllowAnonymous]
    [HttpPost("type-id")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult TypeIdValidation([FromBody] TypeIdModel id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        return StatusCode(StatusCodes.Status200OK, id.TypeId + " is valid");
    }

    /// <summary>
    /// Username validation
    /// </summary>
    [AllowAnonymous]
    [HttpPost("username")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult UsernameValidation([FromBody] UserNameModel userName)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        return StatusCode(StatusCodes.Status200OK, userName.UserName + " is valid");
    }
}
