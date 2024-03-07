using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Service.Models.Authens;
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
                State = "Pending",
                Status = true
            });
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
}
