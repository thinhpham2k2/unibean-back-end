using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Unibean.Service.Models.Brands;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Jwts;
using Unibean.Service.Services.Interfaces;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("Authentication API")]
[Route("api/v1/auths")]
public class AuthController : ControllerBase
{
    private const string TOKEN_SECRET = "<3londonisblueforever<3trueblue<3";

    private static readonly TimeSpan TOKEN_LIFE_TIME = TimeSpan.FromDays(90);

    private readonly IAccountService accountService;

    public AuthController(IAccountService accountService)
    {
        this.accountService = accountService;
    }

    // Login by username & password API ////////////////////////////////
    /// <summary>
    /// Login to the system on the website
    /// </summary>
    [AllowAnonymous]
    [HttpPost("website/login")]
    [ProducesResponseType(typeof(JwtResponseModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult GenerateWebsiteToken([FromBody] LoginFromModel requestLogin)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);
        try
        {
            var account = accountService.GetByUserNameAndPassword(requestLogin.UserName, requestLogin.Password);
            return AccountAuthentication(account != null
                && (account.RoleName.Equals("Admin")
                || account.RoleName.Equals("Brand"))
                ? account : null);
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Login to the system on the mobile
    /// </summary>
    [AllowAnonymous]
    [HttpPost("mobile/login")]
    [ProducesResponseType(typeof(JwtResponseModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult GenerateMobileToken([FromBody] LoginFromModel requestLogin)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var account = accountService.GetByUserNameAndPassword(requestLogin.UserName, requestLogin.Password);
            return AccountAuthentication(account != null
                && (account.RoleName.Equals("Store")
                || account.RoleName.Equals("Student"))
                ? account : null);
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    private IActionResult AccountAuthentication(object user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(TOKEN_SECRET);

        if (user != null)
        {
            bool isVerify = (bool)(user.GetType().GetProperty("IsVerify").GetValue(user) ?? false);
            if (isVerify)
            {
                JwtResponseModel response = new JwtResponseModel();
                var role = (user.GetType().GetProperty("RoleName").GetValue(user) ?? string.Empty).ToString();
                var claims = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(JwtRegisteredClaimNames.Sid, (user.GetType().GetProperty("Id").GetValue(user) ?? string.Empty).ToString()),
                    new(JwtRegisteredClaimNames.Sub, (user.GetType().GetProperty("UserName").GetValue(user) ?? string.Empty).ToString()),
                    new(JwtRegisteredClaimNames.Email, (user.GetType().GetProperty("Email").GetValue(user) ?? string.Empty).ToString()),
                    new(JwtRegisteredClaimNames.Name, (user.GetType().GetProperty("Name").GetValue(user) ?? string.Empty).ToString()),
                    new Claim(ClaimTypes.Role, role)
                };

                response.User = user;

                var tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.Add(TOKEN_LIFE_TIME),
                    Issuer = "http://unibean.spring2024/",
                    Audience = "http://unibean.spring2024/",
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                };

                var token = tokenHandler.CreateToken(tokenDescription);
                response.role = role;
                response.Jwt = tokenHandler.WriteToken(token);
                return Ok(response);
            }
            else
            {
                return BadRequest("Account is not verified");
            }
        }
        else
        {
            return NotFound("Invalid username or password");
        }
    }

    //////////////////////////////////////////////////////////////////

    // Register student and brand API ////////////////////////////////
    /// <summary>
    /// Register account on the website
    /// </summary>
    [AllowAnonymous]
    [HttpPost("website/register")]
    [ProducesResponseType(typeof(JwtResponseModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> WebsiteRegister([FromForm] CreateBrandModel register)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            //var brand = await brandService.Add(register);
            //if (brand != null)
            //{
            //    return StatusCode(StatusCodes.Status201Created, brand);
            //}
            return NotFound("Create fail");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }
}
