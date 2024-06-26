﻿using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Admins;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.Requests;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class AdminControllerTest
{
    private readonly IAdminService adminService;

    private readonly IChartService chartService;

    private readonly IRequestService requestService;

    public AdminControllerTest()
    {
        adminService = A.Fake<IAdminService>();
        chartService = A.Fake<IChartService>();
        requestService = A.Fake<IRequestService>();
    }

    [Fact]
    public void AdminController_GetList_ReturnOK()
    {
        // Arrange
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new AdminController
            (adminService, chartService, requestService);

        // Act
        var result = controller.GetList(state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<AdminModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AdminController_GetList_ReturnBadRequest1()
    {
        // Arrange
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new AdminController
            (adminService, chartService, requestService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetList(state, paging));
    }

    [Fact]
    public void AdminController_GetList_ReturnBadRequest2()
    {
        // Arrange
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new AdminController
            (adminService, chartService, requestService);

        // Act
        var result = controller.GetList(state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<AdminModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AdminController_GetById_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => adminService.GetById(id)).Returns(new());
        var controller = new AdminController
            (adminService, chartService, requestService);

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
            (adminService, chartService, requestService);

        // Act
        var result = controller.GetById(id);

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
            (adminService, chartService, requestService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status201Created,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AdminController_Create_ReturnBadRequest1()
    {
        // Arrange
        CreateAdminModel create = new();
        var controller = new AdminController
            (adminService, chartService, requestService);
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
    public void AdminController_Create_ReturnBadRequest2()
    {
        // Arrange
        CreateAdminModel create = new();
        A.CallTo(() => adminService.Add(create))
            .Throws(new InvalidParameterException());
        var controller = new AdminController
            (adminService, chartService, requestService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AdminController_Create_ReturnNotFound()
    {
        // Arrange
        CreateAdminModel create = new();
        A.CallTo(() => adminService.Add(create)).Returns<AdminExtraModel>(null);
        var controller = new AdminController
            (adminService, chartService, requestService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AdminController_Update_ReturnOK()
    {
        // Arrange
        string id = "";
        UpdateAdminModel update = new();
        A.CallTo(() => adminService.Update(id, update)).Returns<AdminExtraModel>(new());
        var controller = new AdminController
            (adminService, chartService, requestService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AdminController_Update_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        UpdateAdminModel update = new();
        var controller = new AdminController
            (adminService, chartService, requestService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(typeof(InvalidParameterException).ToString(),
            result.Exception?.InnerException?.GetType().ToString());
    }

    [Fact]
    public void AdminController_Update_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        UpdateAdminModel update = new();
        A.CallTo(() => adminService.Update(id, update))
            .Throws(new InvalidParameterException());
        var controller = new AdminController
            (adminService, chartService, requestService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AdminController_Update_ReturnNotFound()
    {
        // Arrange
        string id = "";
        UpdateAdminModel update = new();
        A.CallTo(() => adminService.Update(id, update)).Returns<AdminExtraModel>(null);
        var controller = new AdminController
            (adminService, chartService, requestService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void AdminController_Delete_ReturnNoContent()
    {
        // Arrange
        string id = "";
        var controller = new AdminController
            (adminService, chartService, requestService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StatusCodeResult));
        Assert.Equal(StatusCodes.Status204NoContent,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AdminController_Delete_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => adminService.Delete(id)).Throws(new InvalidParameterException());
        var controller = new AdminController
            (adminService, chartService, requestService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AdminController_GetBrandRankingByAdminId_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => chartService.GetRankingChart
        (id, typeof(Brand), Role.Admin)).Returns(new());
        var controller = new AdminController
            (adminService, chartService, requestService);

        // Act
        var result = controller.GetBrandRankingByAdminId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AdminController_GetBrandRankingByAdminId_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => chartService.GetRankingChart
        (id, typeof(Brand), Role.Admin))
            .Throws(new InvalidParameterException());
        var controller = new AdminController
            (adminService, chartService, requestService);

        // Act
        var result = controller.GetBrandRankingByAdminId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AdminController_GetStudentRankingByAdminId_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => chartService.GetRankingChart
        (id, typeof(Student), Role.Admin)).Returns(new());
        var controller = new AdminController
            (adminService, chartService, requestService);

        // Act
        var result = controller.GetStudentRankingByAdminId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AdminController_GetStudentRankingByAdminId_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => chartService.GetRankingChart
        (id, typeof(Student), Role.Admin))
            .Throws(new InvalidParameterException());
        var controller = new AdminController
            (adminService, chartService, requestService);

        // Act
        var result = controller.GetStudentRankingByAdminId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AdminController_GetColumnChartByAdminId_ReturnOK()
    {
        // Arrange
        string id = "";
        DateOnly fromDate = DateOnly.FromDateTime(DateTime.Now);
        DateOnly toDate = DateOnly.FromDateTime(DateTime.Now);
        bool? isAsc = null;
        A.CallTo(() => chartService.GetColumnChart
        (id, fromDate, toDate, isAsc, Role.Admin)).Returns(new());
        var controller = new AdminController
            (adminService, chartService, requestService);

        // Act
        var result = controller.GetColumnChartByAdminId(id, fromDate, toDate, isAsc);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AdminController_GetColumnChartByAdminId_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        DateOnly fromDate = DateOnly.FromDateTime(DateTime.Now);
        DateOnly toDate = DateOnly.FromDateTime(DateTime.Now);
        bool? isAsc = null;
        A.CallTo(() => chartService.GetColumnChart
        (id, fromDate, toDate, isAsc, Role.Admin))
            .Throws(new InvalidParameterException());
        var controller = new AdminController
            (adminService, chartService, requestService);

        // Act
        var result = controller.GetColumnChartByAdminId(id, fromDate, toDate, isAsc);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AdminController_GetLineChartByAdminId_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => chartService.GetLineChart(id, Role.Admin)).Returns(new());
        var controller = new AdminController
            (adminService, chartService, requestService);

        // Act
        var result = controller.GetLineChartByAdminId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AdminController_GetLineChartByAdminId_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => chartService.GetLineChart(id, Role.Admin))
            .Throws(new InvalidParameterException());
        var controller = new AdminController
            (adminService, chartService, requestService);

        // Act
        var result = controller.GetLineChartByAdminId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AdminController_CreateRequest_ReturnCreated()
    {
        // Arrange
        string id = "";
        CreateRequestModel create = new();
        A.CallTo(() => requestService.Add(id, create)).Returns(new());
        var controller = new AdminController
            (adminService, chartService, requestService);

        // Act
        var result = controller.CreateRequest(id, create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status201Created,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AdminController_CreateRequest_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        CreateRequestModel create = new();
        var controller = new AdminController
            (adminService, chartService, requestService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.CreateRequest(id, create));
    }

    [Fact]
    public void AdminController_CreateRequest_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        CreateRequestModel create = new();
        A.CallTo(() => requestService.Add(id, create))
            .Throws(new InvalidParameterException());
        var controller = new AdminController
            (adminService, chartService, requestService);

        // Act
        var result = controller.CreateRequest(id, create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AdminController_CreateRequest_ReturnNotFound()
    {
        // Arrange
        string id = "";
        CreateRequestModel create = new();
        A.CallTo(() => requestService.Add(id, create)).Returns(null);
        var controller = new AdminController
            (adminService, chartService, requestService);

        // Act
        var result = controller.CreateRequest(id, create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AdminController_GetTitleByAdminId_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => chartService.GetTitleAdmin(id)).Returns(new());
        var controller = new AdminController
            (adminService, chartService, requestService);

        // Act
        var result = controller.GetTitleByAdminId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void AdminController_GetTitleByAdminId_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => chartService.GetTitleAdmin(id))
            .Throws(new InvalidParameterException());
        var controller = new AdminController
            (adminService, chartService, requestService);

        // Act
        var result = controller.GetTitleByAdminId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }
}
