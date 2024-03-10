using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Activities;
using Unibean.Service.Models.Authens;
using Unibean.Service.Models.CampaignActivities;
using Unibean.Service.Models.CampaignDetails;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.Campuses;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Majors;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.Stores;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class CampaignControllerTest
{
    private readonly ICampaignService campaignService;

    private readonly IJwtService jwtService;

    public CampaignControllerTest()
    {
        campaignService = A.Fake<ICampaignService>();
        jwtService = A.Fake<IJwtService>();
    }

    [Fact]
    public void CampaignController_GetList_ReturnOK()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> typeIds = new();
        List<string> storeIds = new();
        List<string> majorIds = new();
        List<string> campusIds = new();
        List<CampaignState> stateIds = new();
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.GetList
            (brandIds, typeIds, storeIds, majorIds, campusIds, stateIds, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<CampaignModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignController_GetList_ReturnBadRequest1()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> typeIds = new();
        List<string> storeIds = new();
        List<string> majorIds = new();
        List<string> campusIds = new();
        List<CampaignState> stateIds = new();
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new CampaignController(campaignService, jwtService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetList(brandIds, typeIds,
            storeIds, majorIds, campusIds, stateIds, paging));
    }

    [Fact]
    public void CampaignController_GetList_ReturnBadRequest2()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> typeIds = new();
        List<string> storeIds = new();
        List<string> majorIds = new();
        List<string> campusIds = new();
        List<CampaignState> stateIds = new();
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.GetList
            (brandIds, typeIds, storeIds, majorIds, campusIds, stateIds, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<CampaignModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignController_GetById_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => campaignService.GetById(id)).Returns(new());
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void CampaignController_GetById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => campaignService.GetById(id))
            .Throws(new InvalidParameterException());
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void CampaignController_Create_ReturnCreated()
    {
        // Arrange
        CreateCampaignModel create = new();
        A.CallTo(() => campaignService.Add(create))
            .Returns<CampaignExtraModel>(new());
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status201Created,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignController_Create_ReturnBadRequest1()
    {
        // Arrange
        CreateCampaignModel create = new();
        var controller = new CampaignController(campaignService, jwtService);
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
    public void CampaignController_Create_ReturnBadRequest2()
    {
        // Arrange
        CreateCampaignModel create = new();
        A.CallTo(() => campaignService.Add(create))
            .Throws(new InvalidParameterException());
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignController_Create_ReturnNotFound()
    {
        // Arrange
        CreateCampaignModel create = new();
        A.CallTo(() => campaignService.Add(create))
            .Returns<CampaignExtraModel>(null);
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignController_Update_ReturnOK()
    {
        // Arrange
        string id = "";
        UpdateCampaignModel update = new();
        A.CallTo(() => campaignService.Update(id, update))
            .Returns<CampaignExtraModel>(new());
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignController_Update_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        UpdateCampaignModel update = new();
        var controller = new CampaignController(campaignService, jwtService);
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
    public void CampaignController_Update_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        UpdateCampaignModel update = new();
        A.CallTo(() => campaignService.Update(id, update))
            .Throws(new InvalidParameterException());
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignController_Update_ReturnNotFound()
    {
        // Arrange
        string id = "";
        UpdateCampaignModel update = new();
        A.CallTo(() => campaignService.Update(id, update))
            .Returns<CampaignExtraModel>(null);
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignController_UpdateState_ReturnOK()
    {
        // Arrange
        string id = "";
        JwtRequestModel jwt = new();
        CampaignState stateId = new();
        A.CallTo(() => jwtService.GetJwtRequest("TOKEN")).Returns(jwt);
        A.CallTo(() => campaignService.UpdateState(id, stateId, jwt))
            .Returns(true);
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = "Bearer TOKEN";
        var controller = new CampaignController(campaignService, jwtService)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            }
        };

        // Act
        var result = controller.UpdateState(id, stateId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void CampaignController_UpdateState_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        JwtRequestModel jwt = new();
        CampaignState stateId = new();
        A.CallTo(() => jwtService.GetJwtRequest("TOKEN")).Returns(jwt);
        A.CallTo(() => campaignService.UpdateState(id, stateId, jwt))
            .Throws(new InvalidParameterException());
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = "Bearer TOKEN";
        var controller = new CampaignController(campaignService, jwtService)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            }
        };
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.UpdateState(id, stateId));
    }

    [Fact]
    public void CampaignController_UpdateState_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        JwtRequestModel jwt = new();
        CampaignState stateId = new();
        A.CallTo(() => jwtService.GetJwtRequest("TOKEN")).Returns(jwt);
        A.CallTo(() => campaignService.UpdateState(id, stateId, jwt))
            .Throws(new InvalidParameterException());
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = "Bearer TOKEN";
        var controller = new CampaignController(campaignService, jwtService)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            }
        };

        // Act
        var result = controller.UpdateState(id, stateId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void CampaignController_UpdateState_ReturnNotFound()
    {
        // Arrange
        string id = "";
        JwtRequestModel jwt = new();
        CampaignState stateId = new();
        A.CallTo(() => jwtService.GetJwtRequest("TOKEN")).Returns(jwt);
        A.CallTo(() => campaignService.UpdateState(id, stateId, jwt))
            .Returns(false);
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = "Bearer TOKEN";
        var controller = new CampaignController(campaignService, jwtService)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            }
        };

        // Act
        var result = controller.UpdateState(id, stateId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(NotFoundObjectResult));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void CampaignController_Delete_ReturnNoContent()
    {
        // Arrange
        string id = "";
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StatusCodeResult));
        Assert.Equal(StatusCodes.Status204NoContent,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void CampaignController_Delete_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => campaignService.Delete(id)).Throws(new InvalidParameterException());
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void CampaignController_GetActivityListByCampaignId_ReturnOK()
    {
        // Arrange
        string id = "";
        List<CampaignState> stateIds = new();
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.GetActivityListByCampaignId(id, stateIds, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<CampaignActivityModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignController_GetActivityListByCampaignId_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        List<CampaignState> stateIds = new();
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new CampaignController(campaignService, jwtService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetActivityListByCampaignId(id, stateIds, paging));
    }

    [Fact]
    public void CampaignController_GetActivityListByCampaignId_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        List<CampaignState> stateIds = new();
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.GetActivityListByCampaignId(id, stateIds, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<CampaignActivityModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignController_GetCampusListByCampaignId_ReturnOK()
    {
        // Arrange
        string id = "";
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
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.GetCampusListByCampaignId
            (id, universityIds, areaIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<CampusModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignController_GetCampusListByCampaignId_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
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
        var controller = new CampaignController(campaignService, jwtService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetCampusListByCampaignId
            (id, universityIds, areaIds, state, paging));
    }

    [Fact]
    public void CampaignController_GetCampusListByCampaignId_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
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
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.GetCampusListByCampaignId
            (id, universityIds, areaIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<CampusModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignController_GetMajorListByCampaignId_ReturnOK()
    {
        // Arrange
        string id = "";
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.GetMajorListByCampaignId(id, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<MajorModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignController_GetMajorListByCampaignId_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new CampaignController(campaignService, jwtService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetMajorListByCampaignId(id, state, paging));
    }

    [Fact]
    public void CampaignController_GetMajorListByCampaignId_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.GetMajorListByCampaignId(id, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<MajorModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignController_GetStoreListByCampaignId_ReturnOK()
    {
        // Arrange
        string id = "";
        List<string> brandIds = new();
        List<string> areaIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.GetStoreListByCampaignId(id, brandIds, areaIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<StoreModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignController_GetStoreListByCampaignId_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        List<string> brandIds = new();
        List<string> areaIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new CampaignController(campaignService, jwtService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetStoreListByCampaignId(id, brandIds, areaIds, state, paging));
    }

    [Fact]
    public void CampaignController_GetStoreListByCampaignId_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        List<string> brandIds = new();
        List<string> areaIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.GetStoreListByCampaignId(id, brandIds, areaIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<StoreModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignController_GetCampaignDetailListByCampaignId_ReturnOK()
    {
        // Arrange
        string id = "";
        List<string> typeIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.GetCampaignDetailListByCampaignId(id, typeIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<CampaignDetailModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignController_GetCampaignDetailListByCampaignId_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        List<string> typeIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new CampaignController(campaignService, jwtService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetCampaignDetailListByCampaignId(id, typeIds, state, paging));
    }

    [Fact]
    public void CampaignController_GetCampaignDetailListByCampaignId_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        List<string> typeIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.GetCampaignDetailListByCampaignId(id, typeIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<CampaignDetailModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignController_GetCampaignDetailById_ReturnOK()
    {
        // Arrange
        string id = "";
        string detailId = "";
        A.CallTo(() => campaignService.GetCampaignDetailById(id, detailId)).Returns(new());
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.GetCampaignDetailById(id, detailId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<CampaignDetailExtraModel>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignController_GetCampaignDetailById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        string detailId = "";
        A.CallTo(() => campaignService.GetCampaignDetailById(id, detailId))
            .Throws(new InvalidParameterException());
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.GetCampaignDetailById(id, detailId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<CampaignDetailExtraModel>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignController_BuyVoucher_ReturnOK()
    {
        // Arrange
        string id = "";
        string detailId = "";
        CreateBuyActivityModel creation = new();
        A.CallTo(() => campaignService.AddActivity(id, detailId, creation)).Returns(true);
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.BuyVoucher(id, detailId, creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<string>));
        Assert.Equal(StatusCodes.Status201Created,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignController_BuyVoucher_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        string detailId = "";
        CreateBuyActivityModel creation = new();
        A.CallTo(() => campaignService.AddActivity(id, detailId, creation))
            .Throws(new InvalidParameterException());
        var controller = new CampaignController(campaignService, jwtService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.BuyVoucher(id, detailId, creation));
    }

    [Fact]
    public void CampaignController_BuyVoucher_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        string detailId = "";
        CreateBuyActivityModel creation = new();
        A.CallTo(() => campaignService.AddActivity(id, detailId, creation))
            .Throws(new InvalidParameterException());
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.BuyVoucher(id, detailId, creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<string>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CampaignController_BuyVoucher_ReturnNotFound()
    {
        // Arrange
        string id = "";
        string detailId = "";
        CreateBuyActivityModel creation = new();
        A.CallTo(() => campaignService.AddActivity(id, detailId, creation))
            .Returns(false);
        var controller = new CampaignController(campaignService, jwtService);

        // Act
        var result = controller.BuyVoucher(id, detailId, creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<string>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
}
