using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Majors;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class MajorControllerTest
{
    private readonly IMajorService majorService;

    public MajorControllerTest()
    {
        majorService = A.Fake<IMajorService>();
    }

    [Fact]
    public void MajorController_GetList_ReturnOK()
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
        var controller = new MajorController(majorService);

        // Act
        var result = controller.GetList(state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<MajorModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void MajorController_GetList_ReturnBadRequest1()
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
        var controller = new MajorController(majorService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetList(state, paging));
    }

    [Fact]
    public void MajorController_GetList_ReturnBadRequest2()
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
        var controller = new MajorController(majorService);

        // Act
        var result = controller.GetList(state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<MajorModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void MajorController_GetById_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => majorService.GetById(id)).Returns(new());
        var controller = new MajorController(majorService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void MajorController_GetById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => majorService.GetById(id)).Throws(new InvalidParameterException());
        var controller = new MajorController(majorService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void MajorController_Create_ReturnCreated()
    {
        // Arrange
        CreateMajorModel create = new();
        A.CallTo(() => majorService.Add(create)).Returns<MajorExtraModel>(new());
        var controller = new MajorController(majorService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status201Created,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void MajorController_Create_ReturnBadRequest1()
    {
        // Arrange
        CreateMajorModel create = new();
        var controller = new MajorController(majorService);
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
    public void MajorController_Create_ReturnBadRequest2()
    {
        // Arrange
        CreateMajorModel create = new();
        A.CallTo(() => majorService.Add(create))
        .Throws(new InvalidParameterException());
        var controller = new MajorController(majorService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void MajorController_Create_ReturnNotFound()
    {
        // Arrange
        CreateMajorModel create = new();
        A.CallTo(() => majorService.Add(create)).Returns<MajorExtraModel>(null);
        var controller = new MajorController(majorService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void MajorController_Update_ReturnOK()
    {
        // Arrange
        string id = "";
        UpdateMajorModel update = new();
        A.CallTo(() => majorService.Update(id, update)).Returns<MajorExtraModel>(new());
        var controller = new MajorController(majorService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void MajorController_Update_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        UpdateMajorModel update = new();
        var controller = new MajorController(majorService);
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
    public void MajorController_Update_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        UpdateMajorModel update = new();
        A.CallTo(() => majorService.Update(id, update))
        .Throws(new InvalidParameterException());
        var controller = new MajorController(majorService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void MajorController_Update_ReturnNotFound()
    {
        // Arrange
        string id = "";
        UpdateMajorModel update = new();
        A.CallTo(() => majorService.Update(id, update)).Returns<MajorExtraModel>(null);
        var controller = new MajorController(majorService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void MajorController_Delete_ReturnNoContent()
    {
        // Arrange
        string id = "";
        var controller = new MajorController(majorService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StatusCodeResult));
        Assert.Equal(StatusCodes.Status204NoContent,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void MajorController_Delete_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => majorService.Delete(id)).Throws(new InvalidParameterException());
        var controller = new MajorController(majorService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }
}
