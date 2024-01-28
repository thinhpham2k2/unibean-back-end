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
    /// Code validation
    /// </summary>
    [AllowAnonymous]
    [HttpPost("code")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult CodeValidation([FromBody] CodeModel code)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        return Ok(code.Code + " is valid");
    }

    /// <summary>
    /// Email validation
    /// </summary>
    [AllowAnonymous]
    [HttpPost("email")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult EmailValidation([FromBody] EmailModel email)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        return Ok(email.Email + " is valid");
    }

    /// <summary>
    /// Invite code validation
    /// </summary>
    [AllowAnonymous]
    [HttpPost("invite-code")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult InviteCodeValidation([FromBody] InviteCodeModel inviteCode)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        return Ok(inviteCode.InviteCode + " is valid");
    }

    /// <summary>
    /// Phone validation
    /// </summary>
    [AllowAnonymous]
    [HttpPost("phone")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult PhoneValidation([FromBody] PhoneModel phone)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        return Ok(phone.Phone + " is valid");
    }

    /// <summary>
    /// Username validation
    /// </summary>
    [AllowAnonymous]
    [HttpPost("username")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult UsernameValidation([FromBody] UserNameModel userName)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        return Ok(userName.UserName + " is valid");
    }
}
