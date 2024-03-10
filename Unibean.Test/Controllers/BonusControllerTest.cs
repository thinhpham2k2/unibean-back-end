using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Bonuses;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class BonusControllerTest
{
    private readonly IBonusService bonusService;

    public BonusControllerTest()
    {
        bonusService = A.Fake<IBonusService>();
    }

    [Fact]
    public void BonusController_GetList_ReturnOK()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> storeIds = new();
        List<string> studentIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new BonusController(bonusService);

        // Act
        var result = controller.GetList(brandIds, storeIds, studentIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<BonusModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void BonusController_GetList_ReturnBadRequest1()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> storeIds = new();
        List<string> studentIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new BonusController(bonusService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetList(brandIds, storeIds, studentIds, state, paging));
    }

    [Fact]
    public void BonusController_GetList_ReturnBadRequest2()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> storeIds = new();
        List<string> studentIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new BonusController(bonusService);

        // Act
        var result = controller.GetList(brandIds, storeIds, studentIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<BonusModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void BonusController_GetById_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => bonusService.GetById(id)).Returns(new());
        var controller = new BonusController(bonusService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void BonusController_GetById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => bonusService.GetById(id)).Throws(new InvalidParameterException());
        var controller = new BonusController(bonusService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }
}
