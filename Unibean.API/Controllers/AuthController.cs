using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Unibean.Service.Models.Accounts;
using Unibean.Service.Models.Authens;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Students;
using Unibean.Service.Services.Interfaces;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("🔐Authentication API")]
[Route("api/v1/auths")]
public class AuthController : ControllerBase
{
    private const string TOKEN_SECRET = "<3londonisblueforever<3trueblue<3";

    private static readonly TimeSpan TOKEN_LIFE_TIME = TimeSpan.FromDays(90);

    private readonly IAccountService accountService;

    private readonly IGoogleService googleService;

    private readonly IStudentService studentService;

    private readonly IEmailService emailService;

    public AuthController(IAccountService accountService,
        IGoogleService googleService,
        IStudentService studentService,
        IEmailService emailService)
    {
        this.accountService = accountService;
        this.googleService = googleService;
        this.studentService = studentService;
        this.emailService = emailService;
    }

    // Login by username & password API ////////////////////////////////
    /// <summary>
    /// Login to the system on the website
    /// </summary>
    [AllowAnonymous]
    [HttpPost("website/login")]
    [ProducesResponseType(typeof(JwtResponseModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(AccountModel), (int)HttpStatusCode.SeeOther)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GenerateWebsiteToken([FromBody] LoginFromModel requestLogin)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);
        try
        {
            var account = accountService.GetByUserNameAndPassword(requestLogin.UserName, requestLogin.Password);
            return AccountAuthentication(account != null
                && (account.Role.Equals("Admin")
                || account.Role.Equals("Brand")
                || account.Role.Equals("Staff"))
                ? account : null);
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Login to the system on the mobile
    /// </summary>
    [AllowAnonymous]
    [HttpPost("mobile/login")]
    [ProducesResponseType(typeof(JwtResponseModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GenerateMobileToken([FromBody] LoginFromModel requestLogin)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var account = accountService.GetByUserNameAndPassword(requestLogin.UserName, requestLogin.Password);
            return AccountAuthentication(account != null
                && (account.Role.Equals("Store")
                || account.Role.Equals("Student"))
                ? account : null);
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    private IActionResult AccountAuthentication(object user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(TOKEN_SECRET);

        if (user != null)
        {
            bool isVerify = (bool)(user.GetType().GetProperty("IsVerify").GetValue(user) ?? false);
            string role = (user.GetType().GetProperty("Role").GetValue(user) ?? string.Empty).ToString();
            if (isVerify)
            {
                JwtResponseModel response = new();

                var claims = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(JwtRegisteredClaimNames.Sid, (user.GetType().GetProperty("Id").GetValue(user) ?? string.Empty).ToString()),
                    new(JwtRegisteredClaimNames.Sub, (user.GetType().GetProperty("UserName").GetValue(user) ?? string.Empty).ToString()),
                    new(JwtRegisteredClaimNames.Email, (user.GetType().GetProperty("Email").GetValue(user) ?? string.Empty).ToString()),
                    new(JwtRegisteredClaimNames.Name, (user.GetType().GetProperty("Name").GetValue(user) ?? string.Empty).ToString()),
                    new("userid", (user.GetType().GetProperty("UserId").GetValue(user) ?? string.Empty).ToString()),
                    new(ClaimTypes.Role, role)
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
                response.Role = role;
                response.Jwt = tokenHandler.WriteToken(token);
                return StatusCode(StatusCodes.Status200OK, response);
            }
            else
            {
                return StatusCode(StatusCodes.Status303SeeOther, user);
            }
        }
        else
        {
            return StatusCode(StatusCodes.Status404NotFound, "Mật khẩu hoặc tài khoản không hợp lệ");
        }
    }

    //////////////////////////////////////////////////////////////////

    // Login with google API////////////////////////////////

    /// <summary>
    /// Login with Google on the website
    /// </summary>
    [AllowAnonymous]
    [HttpPost("website/login/google")]
    [ProducesResponseType(typeof(JwtResponseModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GenerateWebsiteTokenByGoogle(
        [FromBody] GoogleTokenModel token)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);
        
        try
        {
            var account = await googleService.LoginWithGoogle(token, "Brand");

            if (account == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Tài khoản google không hợp lệ");
            }
            else if (account.Role.Equals("Admin")
                || account.Role.Equals("Brand")
                || account.Role.Equals("Staff"))
            {
                return AccountAuthentication(account);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Tài khoản không được truy cập vào nền tảng này");
            }
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Login with Google on the mobile
    /// </summary>
    [AllowAnonymous]
    [HttpPost("mobile/login/google")]
    [ProducesResponseType(typeof(JwtResponseModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(AccountModel), (int)HttpStatusCode.SeeOther)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GenerateMobileTokenByGoogle([FromBody] GoogleTokenModel token)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);
        try
        {
            var account = await googleService.LoginWithGoogle(token, "Student");
            if(account == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Tài khoản google không hợp lệ");
            }
            else if (account.Role.Equals("Store") || account.Role.Equals("Student"))
            {
                return AccountAuthentication(account);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Tài khoản không được truy cập vào nền tảng này");
            }
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Register Google account on the mobile
    /// </summary>
    [AllowAnonymous]
    [HttpPost("mobile/register/google")]
    [ProducesResponseType(typeof(JwtResponseModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> RegisterMobileAccountByGoogle
        ([FromForm] CreateStudentGoogleModel student)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);
        try
        {
            await studentService.AddGoogle(student);
            return AccountAuthentication(accountService.GetByEmail(student.Email));
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    //////////////////////////////////////////////////////////////////

    // Register student and brand API ////////////////////////////////

    /// <summary>
    /// Register account on the website
    /// </summary>
    [AllowAnonymous]
    [HttpPost("website/register")]
    [ProducesResponseType(typeof(AccountModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> WebsiteRegister([FromForm] CreateBrandAccountModel register)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var account = await accountService.AddBrand(register);
            if (account != null)
            {
                return StatusCode(StatusCodes.Status201Created, account);
            }
            return StatusCode(StatusCodes.Status404NotFound, "Đăng kí thất bại");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Register account on the mobile
    /// </summary>
    [AllowAnonymous]
    [HttpPost("mobile/register")]
    [ProducesResponseType(typeof(AccountModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> MobileRegister([FromForm] CreateStudentAccountModel register)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var account = await accountService.AddStudent(register);
            if (account != null)
            {
                return StatusCode(StatusCodes.Status201Created, account);
            }
            return StatusCode(StatusCodes.Status404NotFound, "Đăng kí thất bại");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    //////////////////////////////////////////////////////////////////

    // Send mail (Verification code) API ////////////////////////////////

    /// <summary>
    /// Send email verification code
    /// </summary>
    /// <param name="email">The email will receive a verification code notification.</param>
    [AllowAnonymous]
    [HttpPost("website/mail")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult SendMail([FromQuery] string email)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return StatusCode(StatusCodes.Status201Created, emailService.SendEmailVerification(email));
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }
}
