using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.Staffs;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class StaffControllerTest
{
    private readonly IChartService chartService;

    private readonly IStaffService staffService;

    public StaffControllerTest()
    {
        chartService = A.Fake<IChartService>();
        staffService = A.Fake<IStaffService>();
    }

    [Fact]
    public void StaffController_GetList_ReturnOK()
    {
        // Arrange
        List<string> stationIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StaffController(chartService, staffService);

        // Act
        var result = controller.GetList(stationIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<StaffModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StaffController_GetList_ReturnBadRequest1()
    {
        // Arrange
        List<string> stationIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StaffController(chartService, staffService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetList(stationIds, state, paging));
    }

    [Fact]
    public void StaffController_GetList_ReturnBadRequest2()
    {
        // Arrange
        List<string> stationIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StaffController(chartService, staffService);

        // Act
        var result = controller.GetList(stationIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<StaffModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StaffController_GetById_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => staffService.GetById(id)).Returns(new());
        var controller = new StaffController(chartService, staffService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StaffController_GetById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => staffService.GetById(id)).Throws(new InvalidParameterException());
        var controller = new StaffController(chartService, staffService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StaffController_Create_ReturnCreated()
    {
        // Arrange
        CreateStaffModel create = new();
        A.CallTo(() => staffService.Add(create)).Returns<StaffExtraModel>(new());
        var controller = new StaffController(chartService, staffService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status201Created,
        result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StaffController_Create_ReturnBadRequest1()
    {
        // Arrange
        CreateStaffModel create = new();
        var controller = new StaffController(chartService, staffService);
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
    public void StaffController_Create_ReturnBadRequest2()
    {
        // Arrange
        CreateStaffModel create = new();
        A.CallTo(() => staffService.Add(create))
            .Throws(new InvalidParameterException());
        var controller = new StaffController(chartService, staffService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StaffController_Create_ReturnNotFound()
    {
        // Arrange
        CreateStaffModel create = new();
        A.CallTo(() => staffService.Add(create)).Returns<StaffExtraModel>(null);
        var controller = new StaffController(chartService, staffService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StaffController_Update_ReturnOK()
    {
        // Arrange
        string id = "";
        UpdateStaffModel update = new();
        A.CallTo(() => staffService.Update(id, update)).Returns<StaffExtraModel>(new());
        var controller = new StaffController(chartService, staffService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StaffController_Update_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        UpdateStaffModel update = new();
        var controller = new StaffController(chartService, staffService);
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
    public void StaffController_Update_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        UpdateStaffModel update = new();
        A.CallTo(() => staffService.Update(id, update))
            .Throws(new InvalidParameterException());
        var controller = new StaffController(chartService, staffService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StaffController_Update_ReturnNotFound()
    {
        // Arrange
        string id = "";
        UpdateStaffModel update = new();
        A.CallTo(() => staffService.Update(id, update)).Returns<StaffExtraModel>(null);
        var controller = new StaffController(chartService, staffService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
        result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StaffController_Delete_ReturnNoContent()
    {
        // Arrange
        string id = "";
        var controller = new StaffController(chartService, staffService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StatusCodeResult));
        Assert.Equal(StatusCodes.Status204NoContent,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StaffController_Delete_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => staffService.Delete(id)).Throws(new InvalidParameterException());
        var controller = new StaffController(chartService, staffService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StaffController_GetColumnChartByStaffId_ReturnOK()
    {
        // Arrange
        string id = "";
        DateOnly fromDate = DateOnly.FromDateTime(DateTime.Now);
        DateOnly toDate = DateOnly.FromDateTime(DateTime.Now);
        bool? isAsc = null;
        A.CallTo(() => chartService.GetColumnChart
        (id, fromDate, toDate, isAsc, Role.Staff)).Returns(new());
        var controller = new StaffController(chartService, staffService);

        // Act
        var result = controller.GetColumnChartByStaffId(id, fromDate, toDate, isAsc);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StaffController_GetColumnChartByStaffId_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        DateOnly fromDate = DateOnly.FromDateTime(DateTime.Now);
        DateOnly toDate = DateOnly.FromDateTime(DateTime.Now);
        bool? isAsc = null;
        A.CallTo(() => chartService.GetColumnChart
        (id, fromDate, toDate, isAsc, Role.Staff))
            .Throws(new InvalidParameterException());
        var controller = new StaffController(chartService, staffService);

        // Act
        var result = controller.GetColumnChartByStaffId(id, fromDate, toDate, isAsc);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StaffController_GetProductRankingByStaffId_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => chartService.GetRankingChart
        (id, typeof(Product), Role.Staff)).Returns(new());
        var controller = new StaffController(chartService, staffService);

        // Act
        var result = controller.GetProductRankingByStaffId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StaffController_GetProductRankingByStaffId_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => chartService.GetRankingChart
        (id, typeof(Product), Role.Staff))
            .Throws(new InvalidParameterException());
        var controller = new StaffController(chartService, staffService);

        // Act
        var result = controller.GetProductRankingByStaffId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StaffController_GetStudentRankingByStaffId_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => chartService.GetRankingChart
        (id, typeof(Student), Role.Staff)).Returns(new());
        var controller = new StaffController(chartService, staffService);

        // Act
        var result = controller.GetStudentRankingByStaffId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StaffController_GetStudentRankingByStaffId_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => chartService.GetRankingChart
        (id, typeof(Student), Role.Staff))
            .Throws(new InvalidParameterException());
        var controller = new StaffController(chartService, staffService);

        // Act
        var result = controller.GetStudentRankingByStaffId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StaffController_GetLineChartByStaffId_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => chartService.GetLineChart(id, Role.Staff)).Returns(new());
        var controller = new StaffController(chartService, staffService);

        // Act
        var result = controller.GetLineChartByStaffId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StaffController_GetLineChartByStaffId_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => chartService.GetLineChart(id, Role.Staff))
            .Throws(new InvalidParameterException());
        var controller = new StaffController(chartService, staffService);

        // Act
        var result = controller.GetLineChartByStaffId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StaffController_GetTitleByStaffId_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => chartService.GetTitleStaff(id)).Returns(new());
        var controller = new StaffController(chartService, staffService);

        // Act
        var result = controller.GetTitleByStaffId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StaffController_GetTitleByStaffId_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => chartService.GetTitleStaff(id))
            .Throws(new InvalidParameterException());
        var controller = new StaffController(chartService, staffService);

        // Act
        var result = controller.GetTitleByStaffId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }
}
