using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.Stations;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class StationControllerTest
{
    private readonly IStationService stationService;

    public StationControllerTest()
    {
        stationService = A.Fake<IStationService>();
    }

    [Fact]
    public void StationController_GetList_ReturnOK()
    {
        // Arrange
        List<StationState> stateIds = new();
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StationController(stationService);

        // Act
        var result = controller.GetList(stateIds, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<StationModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StationController_GetList_ReturnBadRequest1()
    {
        // Arrange
        List<StationState> stateIds = new();
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StationController(stationService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetList(stateIds, paging));
    }

    [Fact]
    public void StationController_GetList_ReturnBadRequest2()
    {
        // Arrange
        List<StationState> stateIds = new();
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StationController(stationService);

        // Act
        var result = controller.GetList(stateIds, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<StationModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StationController_GetById_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => stationService.GetById(id)).Returns(new());
        var controller = new StationController(stationService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StationController_GetById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => stationService.GetById(id)).Throws(new InvalidParameterException());
        var controller = new StationController(stationService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StationController_Create_ReturnCreated()
    {
        // Arrange
        CreateStationModel create = new();
        A.CallTo(() => stationService.Add(create)).Returns<StationExtraModel>(new());
        var controller = new StationController(stationService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status201Created,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void StationController_Create_ReturnBadRequest1()
    {
        // Arrange
        CreateStationModel create = new();
        var controller = new StationController(stationService);
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
    public void StationController_Create_ReturnBadRequest2()
    {
        // Arrange
        CreateStationModel create = new();
        A.CallTo(() => stationService.Add(create))
        .Throws(new InvalidParameterException());
        var controller = new StationController(stationService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StationController_Create_ReturnNotFound()
    {
        // Arrange
        CreateStationModel create = new();
        A.CallTo(() => stationService.Add(create)).Returns<StationExtraModel>(null);
        var controller = new StationController(stationService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StationController_Update_ReturnOK()
    {
        // Arrange
        string id = "";
        UpdateStationModel update = new();
        A.CallTo(() => stationService.Update(id, update)).Returns<StationExtraModel>(new());
        var controller = new StationController(stationService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void StationController_Update_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        UpdateStationModel update = new();
        var controller = new StationController(stationService);
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
    public void StationController_Update_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        UpdateStationModel update = new();
        A.CallTo(() => stationService.Update(id, update))
        .Throws(new InvalidParameterException());
        var controller = new StationController(stationService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StationController_Update_ReturnNotFound()
    {
        // Arrange
        string id = "";
        UpdateStationModel update = new();
        A.CallTo(() => stationService.Update(id, update)).Returns<StationExtraModel>(null);
        var controller = new StationController(stationService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void StationController_Delete_ReturnNoContent()
    {
        // Arrange
        string id = "";
        var controller = new StationController(stationService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StatusCodeResult));
        Assert.Equal(StatusCodes.Status204NoContent,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StationController_Delete_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => stationService.Delete(id)).Throws(new InvalidParameterException());
        var controller = new StationController(stationService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StationController_UpdateState_ReturnOK()
    {
        // Arrange
        string id = "";
        StationState stateId = new();
        A.CallTo(() => stationService.UpdateState(id, stateId))
            .Returns(true);
        var controller = new StationController(stationService);

        // Act
        var result = controller.UpdateState(id, stateId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StationController_UpdateState_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        StationState stateId = new();
        A.CallTo(() => stationService.UpdateState(id, stateId))
            .Throws(new InvalidParameterException());
        var controller = new StationController(stationService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.UpdateState(id, stateId));
    }

    [Fact]
    public void StationController_UpdateState_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        StationState stateId = new();
        A.CallTo(() => stationService.UpdateState(id, stateId))
            .Throws(new InvalidParameterException());
        var controller = new StationController(stationService);

        // Act
        var result = controller.UpdateState(id, stateId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StationController_UpdateState_ReturnNotFound()
    {
        // Arrange
        string id = "";
        StationState stateId = new();
        A.CallTo(() => stationService.UpdateState(id, stateId))
            .Returns(false);
        var controller = new StationController(stationService);

        // Act
        var result = controller.UpdateState(id, stateId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(NotFoundObjectResult));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }
}
