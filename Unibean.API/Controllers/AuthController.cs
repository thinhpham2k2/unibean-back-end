﻿using Microsoft.AspNetCore.Authorization;
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

    private readonly IAdminService adminService;

    private readonly IBrandService brandService;

    private readonly IStoreService storeService;

    private readonly IStudentService studentService;

    public AuthController(
        IAccountService accountService,
        IAdminService adminService, 
        IBrandService brandService, 
        IStoreService storeService, 
        IStudentService studentService)
    {
        this.accountService = accountService;
        this.adminService = adminService;
        this.brandService = brandService;
        this.storeService = storeService;
        this.studentService = studentService;
    }

    // Login by username & password API ////////////////////////////////
    /// <summary>
    /// Admin login to system
    /// </summary>
    [AllowAnonymous]
    [HttpPost("admin/login")]
    [ProducesResponseType(typeof(JwtResponseModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult GenerateAdminToken([FromBody] LoginFromModel requestLogin)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);
        try
        {
            var admin = accountService.GetByUserNameAndPassword(requestLogin.UserName, requestLogin.Password);
            return accountAuthentication(admin, "Admin");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Brand login to system
    /// </summary>
    [AllowAnonymous]
    [HttpPost("brand/login")]
    [ProducesResponseType(typeof(JwtResponseModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult GenerateBrandToken([FromBody] LoginFromModel requestLogin)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var brand = accountService.GetByUserNameAndPassword(requestLogin.UserName, requestLogin.Password);
            return accountAuthentication(brand, "Brand");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Store login to system
    /// </summary>
    [AllowAnonymous]
    [HttpPost("store/login")]
    [ProducesResponseType(typeof(JwtResponseModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult GenerateStoreToken([FromBody] LoginFromModel requestLogin)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var store = accountService.GetByUserNameAndPassword(requestLogin.UserName, requestLogin.Password);
            return accountAuthentication(store, "Store");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Student login to system
    /// </summary>
    [AllowAnonymous]
    [HttpPost("student/login")]
    [ProducesResponseType(typeof(JwtResponseModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult GenerateStudentToken([FromBody] LoginFromModel requestLogin)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var student = accountService.GetByUserNameAndPassword(requestLogin.UserName, requestLogin.Password);
            return accountAuthentication(student, "Student");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    private IActionResult accountAuthentication(object user, string role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(TOKEN_SECRET);

        if (user != null)
        {
            JwtResponseModel response = new JwtResponseModel();
            var claims = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(JwtRegisteredClaimNames.Sid, (user.GetType().GetProperty("Id").GetValue(user) ?? string.Empty).ToString()),
                    new(JwtRegisteredClaimNames.Sub, (user.GetType().GetProperty("UserName").GetValue(user) ?? string.Empty).ToString()),
                    new(JwtRegisteredClaimNames.Email, (user.GetType().GetProperty("Email").GetValue(user) ?? string.Empty).ToString()),
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
            return NotFound("Invalid username or password.");
        }
    }

    //////////////////////////////////////////////////////////////////

    // Register student and brand API ////////////////////////////////
    /// <summary>
    /// Brand register account
    /// </summary>
    [AllowAnonymous]
    [HttpPost("brand/register")]
    [ProducesResponseType(typeof(BrandModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> BrandRegister([FromForm] CreateBrandModel register)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var brand = await brandService.Add(register);
            if (brand != null)
            {
                return StatusCode(StatusCodes.Status201Created, brand);
            }
            return NotFound("Create fail");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }
}
