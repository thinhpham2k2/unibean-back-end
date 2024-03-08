using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Authens;
using Unibean.Service.Models.Brands;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class BrandControllerTest
{
    private readonly IBrandService brandService;

    private readonly IJwtService jwtService;

    private readonly IChartService chartService;

    public BrandControllerTest()
    {
        brandService = A.Fake<IBrandService>();
        jwtService = A.Fake<IJwtService>();
        chartService = A.Fake<IChartService>();
    }

    [Fact]
    public void BrandController_GetList_ReturnOK()
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
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = "Bearer TOKEN";
        var controller = new BrandController(brandService, jwtService, chartService)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            }
        };

        // Act
        var result = controller.GetList(state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<BrandModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void BrandController_GetList_ReturnBadRequest()
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
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = "Bearer TOKEN";
        var controller = new BrandController(brandService, jwtService, chartService)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            }
        };

        // Act
        var result = controller.GetList(state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<BrandModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void BrandController_GetById_ReturnOK()
    {
        // Arrange
        string id = "";
        JwtRequestModel jwt = new();
        A.CallTo(() => jwtService.GetJwtRequest("Bearer TOKEN")).Returns(jwt);
        A.CallTo(() => brandService.GetById(id, jwt)).Returns(new());
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = "Bearer TOKEN";
        var controller = new BrandController(brandService, jwtService, chartService)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            }
        };

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void BrandController_GetById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        JwtRequestModel jwt = new();
        A.CallTo(() => jwtService.GetJwtRequest("TOKEN"))
            .Throws(new InvalidParameterException());
        A.CallTo(() => brandService.GetById(id, jwt))
            .Throws(new InvalidParameterException());
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = "Bearer TOKEN";
        var controller = new BrandController(brandService, jwtService, chartService)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            }
        };

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void BrandController_Create_ReturnCreated()
    {
        // Arrange
        CreateBrandModel create = new();
        A.CallTo(() => brandService.Add(create)).Returns<BrandExtraModel>(new());
        var controller = new BrandController(brandService, jwtService, chartService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status201Created,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void BrandController_Create_ReturnBadRequest1()
    {
        // Arrange
        CreateBrandModel create = new();
        var controller = new BrandController(brandService, jwtService, chartService);
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
    public void BrandController_Create_ReturnBadRequest2()
    {
        // Arrange
        CreateBrandModel create = new();
        A.CallTo(() => brandService.Add(create))
            .Throws(new InvalidParameterException());
        var controller = new BrandController(brandService, jwtService, chartService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void BrandController_Create_ReturnNotFound()
    {
        // Arrange
        CreateBrandModel create = new();
        A.CallTo(() => brandService.Add(create))
            .Returns<BrandExtraModel>(null);
        var controller = new BrandController(brandService, jwtService, chartService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void BrandController_Update_ReturnOK()
    {
        // Arrange
        string id = "";
        UpdateBrandModel update = new();
        A.CallTo(() => brandService.Update(id, update)).Returns<BrandExtraModel>(new());
        var controller = new BrandController(brandService, jwtService, chartService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void BrandController_Update_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        UpdateBrandModel update = new();
        var controller = new BrandController(brandService, jwtService, chartService);
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
    public void BrandController_Update_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        UpdateBrandModel update = new();
        A.CallTo(() => brandService.Update(id, update))
            .Throws(new InvalidParameterException());
        var controller = new BrandController(brandService, jwtService, chartService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void BrandController_Update_ReturnNotFound()
    {
        // Arrange
        string id = "";
        UpdateBrandModel update = new();
        A.CallTo(() => brandService.Update(id, update))
            .Returns<BrandExtraModel>(null);
        var controller = new BrandController(brandService, jwtService, chartService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void BrandController_Delete_ReturnNoContent()
    {
        // Arrange
        string id = "";
        var controller = new BrandController(brandService, jwtService, chartService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StatusCodeResult));
        Assert.Equal(StatusCodes.Status204NoContent,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void BrandController_Delete_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => brandService.Delete(id)).Throws(new InvalidParameterException());
        var controller = new BrandController(brandService, jwtService, chartService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void BrandController_GetCampaignListByBrandId_ReturnOK()
    {
        // Arrange
        string id = "";
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
        var controller = new BrandController(brandService, jwtService, chartService);

        // Act
        var result = controller.GetCampaignListByBrandId
            (id, typeIds, storeIds, majorIds, campusIds, stateIds, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<CampaignModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void BrandController_GetCampaignListByBrandId_ReturnBadRequest()
    {
        // Arrange
        string id = "";
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
        var controller = new BrandController(brandService, jwtService, chartService);

        // Act
        var result = controller.GetCampaignListByBrandId
            (id, typeIds, storeIds, majorIds, campusIds, stateIds, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<CampaignModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void BrandController_GetColumnChartByAdminId_ReturnOK()
    {
        // Arrange
        string id = "";
        DateOnly fromDate = DateOnly.FromDateTime(DateTime.Now);
        DateOnly toDate = DateOnly.FromDateTime(DateTime.Now);
        bool? isAsc = null;
        A.CallTo(() => chartService.GetColumnChart
        (id, fromDate, toDate, isAsc, Role.Admin)).Returns(new());
        var controller = new BrandController(brandService, jwtService, chartService);

        // Act
        var result = controller.GetColumnChartByBrandId(id, fromDate, toDate, isAsc);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void BrandController_GetColumnChartByAdminId_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        DateOnly fromDate = DateOnly.FromDateTime(DateTime.Now);
        DateOnly toDate = DateOnly.FromDateTime(DateTime.Now);
        bool? isAsc = null;
        A.CallTo(() => chartService.GetColumnChart
        (id, fromDate, toDate, isAsc, Role.Admin))
            .Throws(new InvalidParameterException());
        var controller = new BrandController(brandService, jwtService, chartService);

        // Act
        var result = controller.GetColumnChartByBrandId(id, fromDate, toDate, isAsc);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }
}
