using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.Requests;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class RequestControllerTest
{
    private readonly IRequestService requestService;

    public RequestControllerTest()
    {
        requestService = A.Fake<IRequestService>();
    }

    [Fact]
    public void RequestController_GetList_ReturnOK()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> adminIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new RequestController
            (requestService);

        // Act
        var result = controller.GetList(brandIds, adminIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<RequestModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void RequestController_GetList_ReturnBadRequest()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> adminIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new RequestController
            (requestService);

        // Act
        var result = controller.GetList(brandIds, adminIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<RequestModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void RequestController_GetById_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => requestService.GetById(id)).Returns(new());
        var controller = new RequestController
            (requestService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void RequestController_GetById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => requestService.GetById(id)).Throws(new InvalidParameterException());
        var controller = new RequestController
            (requestService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }
}
