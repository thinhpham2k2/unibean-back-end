using FakeItEasy;
using FluentAssertions;
using Google.Apis.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Admins;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class AdminControllerTest
{
    private readonly IAdminService adminService;

    private readonly IChartService chartService;

    private readonly IRequestService requestService;

    private readonly IFireBaseService fireBaseService;

    public AdminControllerTest()
    {
        adminService = A.Fake<IAdminService>();
        chartService = A.Fake<IChartService>();
        requestService = A.Fake<IRequestService>();
        fireBaseService = A.Fake<IFireBaseService>();
    }

    [Fact]
    public void AdminController_GetList_ReturnOK()
    {
        // Arrange
        var controller = new AdminController
            (adminService, chartService, requestService, fireBaseService);

        // Act
        var result = controller.GetList(true, new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<AdminModel>>));
        Assert.Equal(StatusCodes.Status200OK, result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AdminController_GetList_ReturnBadRequest()
    {
        // Arrange
        var controller = new AdminController
            (adminService, chartService, requestService, fireBaseService);

        // Act
        var result = controller.GetList(true, new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<AdminModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest, result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AdminController_GetById_ReturnOK()
    {
        // Arrange
        string id = "";
        var controller = new AdminController
            (adminService, chartService, requestService, fireBaseService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AdminController_GetById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => adminService.GetById(id)).Throws(new InvalidParameterException());
        var controller = new AdminController
            (adminService, chartService, requestService, fireBaseService);

        // Act
        var result = controller.GetById("");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AdminController_Create_ReturnCreated()
    {
        // Arrange
        CreateAdminModel create = new();
        A.CallTo(() => adminService.Add(create)).Returns<AdminExtraModel>(new());
        var controller = new AdminController
            (adminService, chartService, requestService, fireBaseService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status201Created,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AdminController_Create_ReturnBadRequest()
    {
        // Arrange
        CreateAdminModel create = new();
        A.CallTo(() => adminService.Add(create)).Returns<AdminExtraModel>(null);
        var controller = new AdminController
            (adminService, chartService, requestService, fireBaseService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(typeof(InvalidParameterException).ToString(), 
            result.Exception?.InnerException?.GetType().ToString());
    }

    [Fact]
    public void AdminController_Create_ReturnNotFound()
    {
        // Arrange
        CreateAdminModel create = new();
        A.CallTo(() => adminService.Add(create)).Returns<AdminExtraModel>(null);
        var controller = new AdminController
            (adminService, chartService, requestService, fireBaseService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
}
