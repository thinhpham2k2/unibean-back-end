using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Service.Models.Accounts;
using Unibean.Service.Models.Authens;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Students;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class AuthControllerTest
{
    private readonly IAccountService accountService;

    private readonly IGoogleService googleService;

    private readonly IStudentService studentService;

    private readonly IEmailService emailService;

    public AuthControllerTest()
    {
        accountService = A.Fake<IAccountService>();
        googleService = A.Fake<IGoogleService>();
        studentService = A.Fake<IStudentService>();
        emailService = A.Fake<IEmailService>();
    }

    [Fact]
    public void AuthController_GenerateWebsiteToken_ReturnOK()
    {
        // Arrange
        LoginFromModel login = new();
        A.CallTo(() => accountService.GetByUserNameAndPassword(login.UserName, login.Password))
            .Returns(new()
            {
                Id = "Id",
                UserId = "UserId",
                Name = "Name",
                RoleId = 1,
                Role = "Admin",
                RoleName = "RoleName",
                UserName = login.UserName,
                Phone = "Phone",
                Email = "Email",
                Avatar = "Avatar",
                FileName = "FileName",
                IsVerify = true,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                DateVerified = DateTime.Now,
                Description = "Description",
                StateId = 1,
                State = "State",
                Status = true
            });
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.GenerateWebsiteToken(login);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AuthController_GenerateWebsiteToken_ReturnSeeOther()
    {
        // Arrange
        LoginFromModel login = new();
        A.CallTo(() => accountService.GetByUserNameAndPassword(login.UserName, login.Password))
            .Returns(new()
            {
                Id = "Id",
                UserId = "UserId",
                Name = "Name",
                RoleId = 1,
                Role = "Admin",
                RoleName = "RoleName",
                UserName = login.UserName,
                Phone = "Phone",
                Email = "Email",
                Avatar = "Avatar",
                FileName = "FileName",
                IsVerify = false,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                DateVerified = DateTime.Now,
                Description = "Description",
                StateId = 1,
                State = "State",
                Status = true
            });
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.GenerateWebsiteToken(login);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status303SeeOther,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AuthController_GenerateWebsiteToken_ReturnBadRequest()
    {
        // Arrange
        LoginFromModel login = new();
        A.CallTo(() => accountService.GetByUserNameAndPassword(login.UserName, login.Password))
            .Throws(new InvalidParameterException());
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.GenerateWebsiteToken(login);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AuthController_GenerateWebsiteToken_ReturnNotFound()
    {
        // Arrange
        LoginFromModel login = new();
        A.CallTo(() => accountService.GetByUserNameAndPassword(login.UserName, login.Password))
            .Returns(null);
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.GenerateWebsiteToken(login);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AuthController_GenerateMobileToken_ReturnOK()
    {
        // Arrange
        LoginFromModel login = new();
        A.CallTo(() => accountService.GetByUserNameAndPassword(login.UserName, login.Password))
            .Returns(new()
            {
                Id = "Id",
                UserId = "UserId",
                Name = "Name",
                RoleId = 1,
                Role = "Student",
                RoleName = "RoleName",
                UserName = login.UserName,
                Phone = "Phone",
                Email = "Email",
                Avatar = "Avatar",
                FileName = "FileName",
                IsVerify = true,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                DateVerified = DateTime.Now,
                Description = "Description",
                StateId = 1,
                State = "Pending",
                Status = true
            });
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.GenerateMobileToken(login);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AuthController_GenerateMobileToken_ReturnBadRequest()
    {
        // Arrange
        LoginFromModel login = new();
        A.CallTo(() => accountService.GetByUserNameAndPassword(login.UserName, login.Password))
            .Throws(new InvalidParameterException());
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.GenerateMobileToken(login);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AuthController_GenerateMobileToken_ReturnNotFound()
    {
        // Arrange
        LoginFromModel login = new();
        A.CallTo(() => accountService.GetByUserNameAndPassword(login.UserName, login.Password))
            .Returns(null);
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.GenerateMobileToken(login);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AuthController_GenerateWebsiteTokenByGoogle_ReturnOK()
    {
        // Arrange
        GoogleTokenModel token = new();
        A.CallTo(() => googleService.LoginWithGoogle(token, "Brand"))
            .Returns<AccountModel>(new()
            {
                Id = "Id",
                UserId = "UserId",
                Name = "Name",
                RoleId = 1,
                Role = "Brand",
                RoleName = "RoleName",
                UserName = "UserName",
                Phone = "Phone",
                Email = "Email",
                Avatar = "Avatar",
                FileName = "FileName",
                IsVerify = true,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                DateVerified = DateTime.Now,
                Description = "Description",
                StateId = 1,
                State = "Active",
                Status = true
            });
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.GenerateWebsiteTokenByGoogle(token);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<IActionResult>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AuthController_GenerateWebsiteTokenByGoogle_ReturnBadRequest1()
    {
        // Arrange
        GoogleTokenModel token = new();
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act
        var result = controller.GenerateWebsiteTokenByGoogle(token);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<IActionResult>));
        Assert.Equal(typeof(InvalidParameterException).ToString(),
            result.Exception?.InnerException?.GetType().ToString());
    }

    [Fact]
    public void AuthController_GenerateWebsiteTokenByGoogle_ReturnBadRequest2()
    {
        // Arrange
        GoogleTokenModel token = new();
        A.CallTo(() => googleService.LoginWithGoogle(token, "Brand"))
            .Throws(new InvalidParameterException());
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.GenerateWebsiteTokenByGoogle(token);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<IActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AuthController_GenerateWebsiteTokenByGoogle_ReturnBadRequest3()
    {
        // Arrange
        GoogleTokenModel token = new();
        A.CallTo(() => googleService.LoginWithGoogle(token, "Brand"))
            .Returns<AccountModel>(new()
            {
                Id = "Id",
                UserId = "UserId",
                Name = "Name",
                RoleId = 1,
                Role = "Student",
                RoleName = "RoleName",
                UserName = "UserName",
                Phone = "Phone",
                Email = "Email",
                Avatar = "Avatar",
                FileName = "FileName",
                IsVerify = true,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                DateVerified = DateTime.Now,
                Description = "Description",
                StateId = 1,
                State = "Active",
                Status = true
            });
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.GenerateWebsiteTokenByGoogle(token);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<IActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AuthController_GenerateWebsiteTokenByGoogle_ReturnNotFound()
    {
        // Arrange
        GoogleTokenModel token = new();
        A.CallTo(() => googleService.LoginWithGoogle(token, "Brand"))
            .Returns<AccountModel>(null);
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.GenerateWebsiteTokenByGoogle(token);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<IActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AuthController_GenerateMobileTokenByGoogle_ReturnOK()
    {
        // Arrange
        GoogleTokenModel token = new();
        A.CallTo(() => googleService.LoginWithGoogle(token, "Student"))
            .Returns<AccountModel>(new()
            {
                Id = "Id",
                UserId = "UserId",
                Name = "Name",
                RoleId = 1,
                Role = "Student",
                RoleName = "RoleName",
                UserName = "UserName",
                Phone = "Phone",
                Email = "Email",
                Avatar = "Avatar",
                FileName = "FileName",
                IsVerify = true,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                DateVerified = DateTime.Now,
                Description = "Description",
                StateId = 1,
                State = "Active",
                Status = true
            });
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.GenerateMobileTokenByGoogle(token);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<IActionResult>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AuthController_GenerateMobileTokenByGoogle_ReturnSeeOther()
    {
        // Arrange
        GoogleTokenModel token = new();
        A.CallTo(() => googleService.LoginWithGoogle(token, "Student"))
            .Returns<AccountModel>(new()
            {
                Id = "Id",
                UserId = "UserId",
                Name = "Name",
                RoleId = 1,
                Role = "Student",
                RoleName = "RoleName",
                UserName = "UserName",
                Phone = "Phone",
                Email = "Email",
                Avatar = "Avatar",
                FileName = "FileName",
                IsVerify = false,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                DateVerified = DateTime.Now,
                Description = "Description",
                StateId = 1,
                State = "Active",
                Status = true
            });
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.GenerateMobileTokenByGoogle(token);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<IActionResult>));
        Assert.Equal(StatusCodes.Status303SeeOther,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AuthController_GenerateMobileTokenByGoogle_ReturnBadRequest1()
    {
        // Arrange
        GoogleTokenModel token = new();
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act
        var result = controller.GenerateMobileTokenByGoogle(token);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<IActionResult>));
        Assert.Equal(typeof(InvalidParameterException).ToString(),
            result.Exception?.InnerException?.GetType().ToString());
    }

    [Fact]
    public void AuthController_GenerateMobileTokenByGoogle_ReturnBadRequest2()
    {
        // Arrange
        GoogleTokenModel token = new();
        A.CallTo(() => googleService.LoginWithGoogle(token, "Student"))
            .Throws(new InvalidParameterException());
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.GenerateMobileTokenByGoogle(token);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<IActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AuthController_GenerateMobileTokenByGoogle_ReturnBadRequest3()
    {
        // Arrange
        GoogleTokenModel token = new();
        A.CallTo(() => googleService.LoginWithGoogle(token, "Student"))
            .Returns<AccountModel>(new()
            {
                Id = "Id",
                UserId = "UserId",
                Name = "Name",
                RoleId = 1,
                Role = "Brand",
                RoleName = "RoleName",
                UserName = "UserName",
                Phone = "Phone",
                Email = "Email",
                Avatar = "Avatar",
                FileName = "FileName",
                IsVerify = false,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                DateVerified = DateTime.Now,
                Description = "Description",
                StateId = 1,
                State = "Active",
                Status = true
            });
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.GenerateMobileTokenByGoogle(token);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<IActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AuthController_GenerateMobileTokenByGoogle_ReturnNotFound()
    {
        // Arrange
        GoogleTokenModel token = new();
        A.CallTo(() => googleService.LoginWithGoogle(token, "Student"))
            .Returns<AccountModel>(null);
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.GenerateMobileTokenByGoogle(token);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<IActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AuthController_RegisterMobileAccountByGoogle_ReturnOK()
    {
        // Arrange
        CreateStudentGoogleModel student = new();
        A.CallTo(() => studentService.AddGoogle(student));
        A.CallTo(() => accountService.GetByEmail(student.Email))
            .Returns(new()
            {
                Id = "Id",
                UserId = "UserId",
                Name = "Name",
                RoleId = 1,
                Role = "Student",
                RoleName = "RoleName",
                UserName = "UserName",
                Phone = "Phone",
                Email = "Email",
                Avatar = "Avatar",
                FileName = "FileName",
                IsVerify = true,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                DateVerified = DateTime.Now,
                Description = "Description",
                StateId = 1,
                State = "Active",
                Status = true
            });
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.RegisterMobileAccountByGoogle(student);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<IActionResult>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AuthController_RegisterMobileAccountByGoogle_ReturnBadRequest1()
    {
        // Arrange
        CreateStudentGoogleModel student = new();
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);
        controller.ModelState.AddModelError("SessionName", "Required");


        // Act
        var result = controller.RegisterMobileAccountByGoogle(student);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<IActionResult>));
        Assert.Equal(typeof(InvalidParameterException).ToString(),
            result.Exception?.InnerException?.GetType().ToString());
    }

    [Fact]
    public void AuthController_RegisterMobileAccountByGoogle_ReturnBadRequest2()
    {
        // Arrange
        CreateStudentGoogleModel student = new();
        A.CallTo(() => studentService.AddGoogle(student));
        A.CallTo(() => accountService.GetByEmail(student.Email))
            .Throws(new InvalidParameterException());
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.RegisterMobileAccountByGoogle(student);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<IActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AuthController_RegisterMobileAccountByGoogle_ReturnNotFound()
    {
        // Arrange
        CreateStudentGoogleModel student = new();
        A.CallTo(() => studentService.AddGoogle(student));
        A.CallTo(() => accountService.GetByEmail(student.Email))
            .Returns(null);
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.RegisterMobileAccountByGoogle(student);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<IActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AuthController_WebsiteRegister_ReturnCreated()
    {
        // Arrange
        CreateBrandAccountModel register = new();
        A.CallTo(() => accountService.AddBrand(register))
            .Returns<AccountModel>(new()
            {
                Id = "Id",
                UserId = "UserId",
                Name = "Name",
                RoleId = 1,
                Role = "Brand",
                RoleName = "RoleName",
                UserName = "UserName",
                Phone = "Phone",
                Email = "Email",
                Avatar = "Avatar",
                FileName = "FileName",
                IsVerify = true,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                DateVerified = DateTime.Now,
                Description = "Description",
                StateId = 1,
                State = "Active",
                Status = true
            });
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.WebsiteRegister(register);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status201Created,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AuthController_WebsiteRegister_ReturnBadRequest()
    {
        // Arrange
        CreateBrandAccountModel register = new();
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act
        var result = controller.WebsiteRegister(register);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(typeof(InvalidParameterException).ToString(),
            result.Exception?.InnerException?.GetType().ToString());
    }

    [Fact]
    public void AuthController_WebsiteRegister_ReturnNotFound()
    {
        // Arrange
        CreateBrandAccountModel register = new();
        A.CallTo(() => accountService.AddBrand(register)).Returns<AccountModel>(null);
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.WebsiteRegister(register);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AuthController_MobileRegister_ReturnCreated()
    {
        // Arrange
        CreateStudentAccountModel register = new();
        A.CallTo(() => accountService.AddStudent(register))
            .Returns<AccountModel>(new()
            {
                Id = "Id",
                UserId = "UserId",
                Name = "Name",
                RoleId = 1,
                Role = "Student",
                RoleName = "RoleName",
                UserName = "UserName",
                Phone = "Phone",
                Email = "Email",
                Avatar = "Avatar",
                FileName = "FileName",
                IsVerify = true,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                DateVerified = DateTime.Now,
                Description = "Description",
                StateId = 1,
                State = "Active",
                Status = true
            });
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.MobileRegister(register);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status201Created,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AuthController_MobileRegister_ReturnBadRequest()
    {
        // Arrange
        CreateStudentAccountModel register = new();
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act
        var result = controller.MobileRegister(register);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(typeof(InvalidParameterException).ToString(),
            result.Exception?.InnerException?.GetType().ToString());
    }

    [Fact]
    public void AuthController_MobileRegister_ReturnNotFound()
    {
        // Arrange
        CreateStudentAccountModel register = new();
        A.CallTo(() => accountService.AddStudent(register)).Returns<AccountModel>(null);
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.MobileRegister(register);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AuthController_SendMail_ReturnCreated()
    {
        // Arrange
        string email = "";
        A.CallTo(() => emailService.SendEmailVerification(email))
            .Returns("HASH_CODE");
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.SendMail(email);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status201Created,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AuthController_SendMail_ReturnBadRequest()
    {
        // Arrange
        string email = "";
        A.CallTo(() => emailService.SendEmailVerification(email))
            .Throws(new InvalidParameterException());
        var controller = new AuthController
            (accountService, googleService, studentService, emailService);

        // Act
        var result = controller.SendMail(email);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }
}
