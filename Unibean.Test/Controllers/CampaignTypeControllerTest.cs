using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Authens;
using Unibean.Service.Models.CampaignTypes;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class CampaignTypeControllerTest
{
    private readonly ICampaignTypeService campaignTypeService;

    public CampaignTypeControllerTest()
    {
        campaignTypeService = A.Fake<ICampaignTypeService>();
    }

    [Fact]
    public void CampaignTypeController_GetList_ReturnOK()
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
        var controller = new CampaignTypeController(campaignTypeService);

        // Act
        var result = controller.GetList(state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<CampaignTypeModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignTypeController_GetList_ReturnBadRequest1()
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
        var controller = new CampaignTypeController(campaignTypeService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetList(state, paging));
    }

    [Fact]
    public void CampaignTypeController_GetList_ReturnBadRequest2()
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
        var controller = new CampaignTypeController(campaignTypeService);

        // Act
        var result = controller.GetList(state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<CampaignTypeModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignTypeController_GetById_ReturnOK()
    {
        // Arrange
        string id = "";
        JwtRequestModel jwt = new();
        A.CallTo(() => campaignTypeService.GetById(id)).Returns(new());
        var controller = new CampaignTypeController(campaignTypeService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void CampaignTypeController_GetById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        JwtRequestModel jwt = new();
        A.CallTo(() => campaignTypeService.GetById(id))
            .Throws(new InvalidParameterException());
        var controller = new CampaignTypeController(campaignTypeService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void CampaignTypeController_Create_ReturnCreated()
    {
        // Arrange
        CreateCampaignTypeModel create = new();
        A.CallTo(() => campaignTypeService.Add(create))
            .Returns<CampaignTypeExtraModel>(new());
        var controller = new CampaignTypeController(campaignTypeService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status201Created,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignTypeController_Create_ReturnBadRequest1()
    {
        // Arrange
        CreateCampaignTypeModel create = new();
        var controller = new CampaignTypeController(campaignTypeService);
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
    public void CampaignTypeController_Create_ReturnBadRequest2()
    {
        // Arrange
        CreateCampaignTypeModel create = new();
        A.CallTo(() => campaignTypeService.Add(create))
            .Throws(new InvalidParameterException());
        var controller = new CampaignTypeController(campaignTypeService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignTypeController_Create_ReturnNotFound()
    {
        // Arrange
        CreateCampaignTypeModel create = new();
        A.CallTo(() => campaignTypeService.Add(create))
            .Returns<CampaignTypeExtraModel>(null);
        var controller = new CampaignTypeController(campaignTypeService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignTypeController_Update_ReturnOK()
    {
        // Arrange
        string id = "";
        UpdateCampaignTypeModel update = new();
        A.CallTo(() => campaignTypeService.Update(id, update))
            .Returns<CampaignTypeExtraModel>(new());
        var controller = new CampaignTypeController(campaignTypeService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignTypeController_Update_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        UpdateCampaignTypeModel update = new();
        var controller = new CampaignTypeController(campaignTypeService);
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
    public void CampaignTypeController_Update_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        UpdateCampaignTypeModel update = new();
        A.CallTo(() => campaignTypeService.Update(id, update))
            .Throws(new InvalidParameterException());
        var controller = new CampaignTypeController(campaignTypeService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignTypeController_Update_ReturnNotFound()
    {
        // Arrange
        string id = "";
        UpdateCampaignTypeModel update = new();
        A.CallTo(() => campaignTypeService.Update(id, update))
            .Returns<CampaignTypeExtraModel>(null);
        var controller = new CampaignTypeController(campaignTypeService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignTypeController_Delete_ReturnNoContent()
    {
        // Arrange
        string id = "";
        var controller = new CampaignTypeController(campaignTypeService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StatusCodeResult));
        Assert.Equal(StatusCodes.Status204NoContent,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void CampaignTypeController_Delete_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => campaignTypeService.Delete(id))
            .Throws(new InvalidParameterException());
        var controller = new CampaignTypeController(campaignTypeService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }
}
