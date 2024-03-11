using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Validations;

namespace Unibean.Test.Controllers;

public class ValidationControllerTest
{
    [Fact]
    public void ValidationController_CodeValidation_ReturnOK()
    {
        // Arrange
        CodeModel code = new();
        var controller = new ValidationController();

        // Act
        var result = controller.CodeValidation(code);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void ValidationController_CodeValidation_ReturnBadRequest()
    {
        // Arrange
        CodeModel code = new();
        var controller = new ValidationController();
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.CodeValidation(code));
    }

    [Fact]
    public void ValidationController_EmailValidation_ReturnOK()
    {
        // Arrange
        EmailModel email = new();
        var controller = new ValidationController();

        // Act
        var result = controller.EmailValidation(email);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void ValidationController_EmailValidation_ReturnBadRequest()
    {
        // Arrange
        EmailModel email = new();
        var controller = new ValidationController();
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.EmailValidation(email));
    }

    [Fact]
    public void ValidationController_InviteCodeValidation_ReturnOK()
    {
        // Arrange
        InviteCodeModel inviteCode = new();
        var controller = new ValidationController();

        // Act
        var result = controller.InviteCodeValidation(inviteCode);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void ValidationController_InviteCodeValidation_ReturnBadRequest()
    {
        // Arrange
        InviteCodeModel inviteCode = new();
        var controller = new ValidationController();
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.InviteCodeValidation(inviteCode));
    }

    [Fact]
    public void ValidationController_PhoneValidation_ReturnOK()
    {
        // Arrange
        PhoneModel phone = new();
        var controller = new ValidationController();

        // Act
        var result = controller.PhoneValidation(phone);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void ValidationController_PhoneValidation_ReturnBadRequest()
    {
        // Arrange
        PhoneModel phone = new();
        var controller = new ValidationController();
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.PhoneValidation(phone));
    }

    [Fact]
    public void ValidationController_UsernameValidation_ReturnOK()
    {
        // Arrange
        UserNameModel userName = new();
        var controller = new ValidationController();

        // Act
        var result = controller.UsernameValidation(userName);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void ValidationController_UsernameValidation_ReturnBadRequest()
    {
        // Arrange
        UserNameModel userName = new();
        var controller = new ValidationController();
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.UsernameValidation(userName));
    }
}
