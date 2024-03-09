using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Orders;
using Unibean.Service.Models.OrderStates;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class OrderControllerTest
{
    private readonly IOrderService orderService;

    private readonly IOrderStateService orderStateService;

    public OrderControllerTest()
    {
        orderService = A.Fake<IOrderService>();
        orderStateService = A.Fake<IOrderStateService>();
    }

    [Fact]
    public void OrderController_GetList_ReturnOK()
    {
        // Arrange
        List<string> stationIds = new();
        List<string> studentIds = new();
        List<State> stateIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new OrderController(orderService, orderStateService);

        // Act
        var result = controller.GetList(stationIds, studentIds, stateIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<OrderModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void OrderController_GetList_ReturnBadRequest()
    {
        // Arrange
        List<string> stationIds = new();
        List<string> studentIds = new();
        List<State> stateIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new OrderController(orderService, orderStateService);

        // Act
        var result = controller.GetList(stationIds, studentIds, stateIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<OrderModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void OrderController_GetById_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => orderService.GetById(id)).Returns(new());
        var controller = new OrderController(orderService, orderStateService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void OrderController_GetById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => orderService.GetById(id))
            .Throws(new InvalidParameterException());
        var controller = new OrderController(orderService, orderStateService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void OrderController_CreateStateForOrder_ReturnCreated()
    {
        // Arrange
        string id = "";
        CreateOrderStateModel create = new();
        A.CallTo(() => orderStateService.Add(id, create))
            .Returns("");
        var controller = new OrderController(orderService, orderStateService);

        // Act
        var result = controller.CreateStateForOrder(id, create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status201Created,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void OrderController_CreateStateForOrder_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        CreateOrderStateModel create = new();
        A.CallTo(() => orderStateService.Add(id, create))
            .Throws(new InvalidParameterException());
        var controller = new OrderController(orderService, orderStateService);

        // Act
        var result = controller.CreateStateForOrder(id, create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }
}
