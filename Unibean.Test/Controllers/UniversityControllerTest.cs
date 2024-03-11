using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.Universities;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class UniversityControllerTest
{
    private readonly IUniversityService universityService;

    public UniversityControllerTest()
    {
        universityService = A.Fake<IUniversityService>();
    }

    [Fact]
    public void UniversityController_GetList_ReturnOK()
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
        var controller = new UniversityController(universityService);

        // Act
        var result = controller.GetList(state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<UniversityModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void UniversityController_GetList_ReturnBadRequest1()
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
        var controller = new UniversityController(universityService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetList(state, paging));
    }

    [Fact]
    public void UniversityController_GetList_ReturnBadRequest2()
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
        var controller = new UniversityController(universityService);

        // Act
        var result = controller.GetList(state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<UniversityModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void UniversityController_GetById_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => universityService.GetById(id)).Returns(new());
        var controller = new UniversityController(universityService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void UniversityController_GetById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => universityService.GetById(id)).Throws(new InvalidParameterException());
        var controller = new UniversityController(universityService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void UniversityController_Create_ReturnCreated()
    {
        // Arrange
        CreateUniversityModel create = new();
        A.CallTo(() => universityService.Add(create)).Returns<UniversityExtraModel>(new());
        var controller = new UniversityController(universityService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status201Created,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void UniversityController_Create_ReturnBadRequest1()
    {
        // Arrange
        CreateUniversityModel create = new();
        var controller = new UniversityController(universityService);
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
    public void UniversityController_Create_ReturnBadRequest2()
    {
        // Arrange
        CreateUniversityModel create = new();
        A.CallTo(() => universityService.Add(create))
        .Throws(new InvalidParameterException());
        var controller = new UniversityController(universityService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void UniversityController_Create_ReturnNotFound()
    {
        // Arrange
        CreateUniversityModel create = new();
        A.CallTo(() => universityService.Add(create)).Returns<UniversityExtraModel>(null);
        var controller = new UniversityController(universityService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void UniversityController_Update_ReturnOK()
    {
        // Arrange
        string id = "";
        UpdateUniversityModel update = new();
        A.CallTo(() => universityService.Update(id, update)).Returns<UniversityExtraModel>(new());
        var controller = new UniversityController(universityService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void UniversityController_Update_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        UpdateUniversityModel update = new();
        var controller = new UniversityController(universityService);
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
    public void UniversityController_Update_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        UpdateUniversityModel update = new();
        A.CallTo(() => universityService.Update(id, update))
        .Throws(new InvalidParameterException());
        var controller = new UniversityController(universityService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void UniversityController_Update_ReturnNotFound()
    {
        // Arrange
        string id = "";
        UpdateUniversityModel update = new();
        A.CallTo(() => universityService.Update(id, update)).Returns<UniversityExtraModel>(null);
        var controller = new UniversityController(universityService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void UniversityController_Delete_ReturnNoContent()
    {
        // Arrange
        string id = "";
        var controller = new UniversityController(universityService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StatusCodeResult));
        Assert.Equal(StatusCodes.Status204NoContent,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void UniversityController_Delete_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => universityService.Delete(id)).Throws(new InvalidParameterException());
        var controller = new UniversityController(universityService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }
}
