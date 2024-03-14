using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Activities;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Services.Interfaces;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.Test.Controllers;

public class ActivityControllerTest
{
    private readonly IActivityService activityService;

    public ActivityControllerTest()
    {
        activityService = A.Fake<IActivityService>();
    }

    [Fact]
    public void ActivityController_GetList_ReturnOK()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> storeIds = new();
        List<string> studentIds = new();
        List<string> campaginIds = new();
        List<string> campaginDetailIds = new();
        List<string> voucherIds = new();
        List<string> voucherItemIds = new();
        List<Type> typeIds = new();
        bool ? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new ActivityController(activityService);

        // Act
        var result = controller.GetList(brandIds, storeIds, studentIds, campaginIds, 
            campaginDetailIds, voucherIds, voucherItemIds, typeIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<ActivityModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void ActivityController_GetList_ReturnBadRequest1()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> storeIds = new();
        List<string> studentIds = new();
        List<string> campaginIds = new();
        List<string> campaginDetailIds = new();
        List<string> voucherIds = new();
        List<string> voucherItemIds = new();
        List<Type> typeIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new ActivityController(activityService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetList(brandIds, storeIds, studentIds, campaginIds,
            campaginDetailIds, voucherIds, voucherItemIds, typeIds, state, paging));
    }

    [Fact]
    public void ActivityController_GetList_ReturnBadRequest2()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> storeIds = new();
        List<string> studentIds = new();
        List<string> campaginIds = new();
        List<string> campaginDetailIds = new();
        List<string> voucherIds = new();
        List<string> voucherItemIds = new();
        List<Type> typeIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new ActivityController(activityService);

        // Act
        var result = controller.GetList(brandIds, storeIds, studentIds, campaginIds,
            campaginDetailIds, voucherIds, voucherItemIds, typeIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<ActivityModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void ActivityController_GetById_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => activityService.GetById(id)).Returns(new());
        var controller = new ActivityController(activityService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void ActivityController_GetById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => activityService.GetById(id)).Throws(new InvalidParameterException());
        var controller = new ActivityController(activityService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }
}
