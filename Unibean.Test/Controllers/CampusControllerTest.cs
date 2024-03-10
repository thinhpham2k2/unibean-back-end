using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Campuses;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class CampusControllerTest
{
    private readonly ICampusService campusService;

    public CampusControllerTest()
    {
        campusService = A.Fake<ICampusService>();
    }

    [Fact]
    public void CampusController_GetList_ReturnOK()
    {
        // Arrange
        List<string> universityIds = new();
        List<string> areaIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new CampusController(campusService);

        // Act
        var result = controller.GetList(universityIds, areaIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<CampusModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampusController_GetList_ReturnBadRequest1()
    {
        // Arrange
        List<string> universityIds = new();
        List<string> areaIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new CampusController(campusService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetList(universityIds, areaIds, state, paging));
    }

    [Fact]
    public void CampusController_GetList_ReturnBadRequest2()
    {
        // Arrange
        List<string> universityIds = new();
        List<string> areaIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new CampusController(campusService);

        // Act
        var result = controller.GetList(universityIds, areaIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<CampusModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampusController_GetById_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => campusService.GetById(id)).Returns(new());
        var controller = new CampusController(campusService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void CampusController_GetById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => campusService.GetById(id))
            .Throws(new InvalidParameterException());
        var controller = new CampusController(campusService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void CampusController_Create_ReturnCreated()
    {
        // Arrange
        CreateCampusModel create = new();
        A.CallTo(() => campusService.Add(create))
            .Returns<CampusExtraModel>(new());
        var controller = new CampusController(campusService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status201Created,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampusController_Create_ReturnBadRequest1()
    {
        // Arrange
        CreateCampusModel create = new();
        var controller = new CampusController(campusService);
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
    public void CampusController_Create_ReturnBadRequest2()
    {
        // Arrange
        CreateCampusModel create = new();
        A.CallTo(() => campusService.Add(create))
            .Throws(new InvalidParameterException());
        var controller = new CampusController(campusService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampusController_Create_ReturnNotFound()
    {
        // Arrange
        CreateCampusModel create = new();
        A.CallTo(() => campusService.Add(create))
            .Returns<CampusExtraModel>(null);
        var controller = new CampusController(campusService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampusController_Update_ReturnOK()
    {
        // Arrange
        string id = "";
        UpdateCampusModel update = new();
        A.CallTo(() => campusService.Update(id, update))
            .Returns<CampusExtraModel>(new());
        var controller = new CampusController(campusService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampusController_Update_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        UpdateCampusModel update = new();
        var controller = new CampusController(campusService);
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
    public void CampusController_Update_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        UpdateCampusModel update = new();
        A.CallTo(() => campusService.Update(id, update))
            .Throws(new InvalidParameterException());
        var controller = new CampusController(campusService);


        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampusController_Update_ReturnNotFound()
    {
        // Arrange
        string id = "";
        UpdateCampusModel update = new();
        A.CallTo(() => campusService.Update(id, update))
            .Returns<CampusExtraModel>(null);
        var controller = new CampusController(campusService);


        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampusController_Delete_ReturnNoContent()
    {
        // Arrange
        string id = "";
        var controller = new CampusController(campusService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StatusCodeResult));
        Assert.Equal(StatusCodes.Status204NoContent,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void CampusController_Delete_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => campusService.Delete(id))
            .Throws(new InvalidParameterException());
        var controller = new CampusController(campusService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }
}
