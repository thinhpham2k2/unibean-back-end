using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Challenges;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class ChallengeControllerTest
{
    private readonly IChallengeService challengeService;

    public ChallengeControllerTest()
    {
        challengeService = A.Fake<IChallengeService>();
    }

    [Fact]
    public void ChallengeController_GetList_ReturnOK()
    {
        // Arrange
        List<ChallengeType> typeIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new ChallengeController(challengeService);

        // Act
        var result = controller.GetList(typeIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<ChallengeModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void ChallengeController_GetList_ReturnBadRequest()
    {
        // Arrange
        List<ChallengeType> typeIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new ChallengeController(challengeService);

        // Act
        var result = controller.GetList(typeIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<ChallengeModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void ChallengeController_GetById_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => challengeService.GetById(id)).Returns(new());
        var controller = new ChallengeController(challengeService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void ChallengeController_GetById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => challengeService.GetById(id))
            .Throws(new InvalidParameterException());
        var controller = new ChallengeController(challengeService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void ChallengeController_Create_ReturnCreated()
    {
        // Arrange
        CreateChallengeModel create = new();
        A.CallTo(() => challengeService.Add(create))
            .Returns<ChallengeExtraModel>(new());
        var controller = new ChallengeController(challengeService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status201Created,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void ChallengeController_Create_ReturnBadRequest1()
    {
        // Arrange
        CreateChallengeModel create = new();
        var controller = new ChallengeController(challengeService);
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
    public void ChallengeController_Create_ReturnBadRequest2()
    {
        // Arrange
        CreateChallengeModel create = new();
        A.CallTo(() => challengeService.Add(create))
            .Throws(new InvalidParameterException());
        var controller = new ChallengeController(challengeService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void ChallengeController_Create_ReturnNotFound()
    {
        // Arrange
        CreateChallengeModel create = new();
        A.CallTo(() => challengeService.Add(create))
            .Returns<ChallengeExtraModel>(null);
        var controller = new ChallengeController(challengeService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void ChallengeController_Update_ReturnOK()
    {
        // Arrange
        string id = "";
        UpdateChallengeModel update = new();
        A.CallTo(() => challengeService.Update(id, update))
            .Returns<ChallengeExtraModel>(new());
        var controller = new ChallengeController(challengeService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void ChallengeController_Update_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        UpdateChallengeModel update = new();
        var controller = new ChallengeController(challengeService);
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
    public void ChallengeController_Update_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        UpdateChallengeModel update = new();
        A.CallTo(() => challengeService.Update(id, update))
            .Throws(new InvalidParameterException());
        var controller = new ChallengeController(challengeService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void ChallengeController_Update_ReturnNotFound()
    {
        // Arrange
        string id = "";
        UpdateChallengeModel update = new();
        A.CallTo(() => challengeService.Update(id, update))
            .Returns<ChallengeExtraModel>(null);
        var controller = new ChallengeController(challengeService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void ChallengeController_Delete_ReturnNoContent()
    {
        // Arrange
        string id = "";
        var controller = new ChallengeController(challengeService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StatusCodeResult));
        Assert.Equal(StatusCodes.Status204NoContent,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void ChallengeController_Delete_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => challengeService.Delete(id))
            .Throws(new InvalidParameterException());
        var controller = new ChallengeController(challengeService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }
}
