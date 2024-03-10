using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Activities;
using Unibean.Service.Models.Bonuses;
using Unibean.Service.Models.CampaignDetails;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.Stores;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Models.VoucherItems;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class StoreControllerTest
{
    private readonly IStoreService storeService;

    private readonly IBonusService bonusService;

    private readonly IChartService chartService;

    public StoreControllerTest()
    {
        storeService = A.Fake<IStoreService>();
        bonusService = A.Fake<IBonusService>();
        chartService = A.Fake<IChartService>();
    }

    [Fact]
    public void StoreController_GetList_ReturnOK()
    {
        // Arrange
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
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.GetList(brandIds, areaIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<StoreModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StoreController_GetList_ReturnBadRequest1()
    {
        // Arrange
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
        var controller = new StoreController
            (storeService, bonusService, chartService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetList(brandIds, areaIds, state, paging));
    }

    [Fact]
    public void StoreController_GetList_ReturnBadRequest2()
    {
        // Arrange
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
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.GetList(brandIds, areaIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<StoreModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StoreController_GetById_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => storeService.GetById(id)).Returns(new());
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StoreController_GetById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => storeService.GetById(id)).Throws(new InvalidParameterException());
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StoreController_Create_ReturnCreated()
    {
        // Arrange
        CreateStoreModel create = new();
        A.CallTo(() => storeService.Add(create)).Returns<StoreExtraModel>(new());
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status201Created,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StoreController_Create_ReturnBadRequest1()
    {
        // Arrange
        CreateStoreModel create = new();
        var controller = new StoreController
            (storeService, bonusService, chartService);
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
    public void StoreController_Create_ReturnBadRequest2()
    {
        // Arrange
        CreateStoreModel create = new();
        A.CallTo(() => storeService.Add(create))
            .Throws(new InvalidParameterException());
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StoreController_Create_ReturnNotFound()
    {
        // Arrange
        CreateStoreModel create = new();
        A.CallTo(() => storeService.Add(create)).Returns<StoreExtraModel>(null);
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
        result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StoreController_Update_ReturnOK()
    {
        // Arrange
        string id = "";
        UpdateStoreModel update = new();
        A.CallTo(() => storeService.Update(id, update)).Returns<StoreExtraModel>(new());
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StoreController_Update_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        UpdateStoreModel update = new();
        var controller = new StoreController
            (storeService, bonusService, chartService);
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
    public void StoreController_Update_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        UpdateStoreModel update = new();
        A.CallTo(() => storeService.Update(id, update))
            .Throws(new InvalidParameterException());
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StoreController_Update_ReturnNotFound()
    {
        // Arrange
        string id = "";
        UpdateStoreModel update = new();
        A.CallTo(() => storeService.Update(id, update)).Returns<StoreExtraModel>(null);
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StoreController_Delete_ReturnNoContent()
    {
        // Arrange
        string id = "";
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StatusCodeResult));
        Assert.Equal(StatusCodes.Status204NoContent,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StoreController_Delete_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => storeService.Delete(id)).Throws(new InvalidParameterException());
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StoreController_CreateBonus_ReturnCreated()
    {
        // Arrange
        string id = "";
        CreateBonusModel create = new();
        A.CallTo(() => bonusService.Add(id, create)).Returns(new());
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.CreateBonus(id, create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status201Created,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StoreController_CreateBonus_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        CreateBonusModel create = new();
        var controller = new StoreController
            (storeService, bonusService, chartService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.CreateBonus(id, create));
    }

    [Fact]
    public void StoreController_CreateBonus_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        CreateBonusModel create = new();
        A.CallTo(() => bonusService.Add(id, create))
            .Throws(new InvalidParameterException());
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.CreateBonus(id, create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StoreController_CreateBonus_ReturnNotFound()
    {
        // Arrange
        string id = "";
        CreateBonusModel create = new();
        A.CallTo(() => bonusService.Add(id, create)).Returns(null);
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.CreateBonus(id, create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StoreController_GetVoucherListByStoreId_ReturnOK()
    {
        // Arrange
        string id = "";
        List<string> campaignIds = new();
        List<string> typeIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.GetVoucherListByStoreId(id, campaignIds, typeIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<CampaignDetailModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StoreController_GetVoucherListByStoreId_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        List<string> campaignIds = new();
        List<string> typeIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StoreController
            (storeService, bonusService, chartService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetVoucherListByStoreId(id, campaignIds, typeIds, state, paging));
    }

    [Fact]
    public void StoreController_GetVoucherListByStoreId_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        List<string> campaignIds = new();
        List<string> typeIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.GetVoucherListByStoreId(id, campaignIds, typeIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<CampaignDetailModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StoreController_GetCampaignDetailById_ReturnOK()
    {
        // Arrange
        string id = "";
        string detailId = "";
        A.CallTo(() => storeService.GetCampaignDetailById(id, detailId)).Returns(new());
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.GetCampaignDetailById(id, detailId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<CampaignDetailExtraModel>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StoreController_GetCampaignDetailById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        string detailId = "";
        A.CallTo(() => storeService.GetCampaignDetailById(id, detailId))
            .Throws(new InvalidParameterException());
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.GetCampaignDetailById(id, detailId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<CampaignDetailExtraModel>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StoreController_ScanVoucher_ReturnCreated()
    {
        // Arrange
        string id = "";
        string code = "";
        CreateUseActivityModel creation = new();
        A.CallTo(() => storeService.AddActivity(id, code, creation))
            .Returns(true);
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.ScanVoucher(id, code, creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<string>));
        Assert.Equal(StatusCodes.Status201Created,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StoreController_ScanVoucher_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        string code = "";
        CreateUseActivityModel creation = new();
        A.CallTo(() => storeService.AddActivity(id, code, creation))
            .Returns(true);
        var controller = new StoreController
            (storeService, bonusService, chartService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.ScanVoucher(id, code, creation));
    }

    [Fact]
    public void StoreController_ScanVoucher_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        string code = "";
        CreateUseActivityModel creation = new();
        A.CallTo(() => storeService.AddActivity(id, code, creation))
            .Throws(new InvalidParameterException());
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.ScanVoucher(id, code, creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<string>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StoreController_ScanVoucher_ReturnNotFound()
    {
        // Arrange
        string id = "";
        string code = "";
        CreateUseActivityModel creation = new();
        A.CallTo(() => storeService.AddActivity(id, code, creation))
            .Returns(false);
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.ScanVoucher(id, code, creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<string>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StoreController_GetVoucherInformation_ReturnOK()
    {
        // Arrange
        string id = "";
        string code = "";
        A.CallTo(() => storeService.GetVoucherItemByCode(id, code)).Returns(new());
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.GetVoucherInformation(id, code);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<VoucherItemExtraModel>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StoreController_GetVoucherInformation_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        string code = "";
        var controller = new StoreController
            (storeService, bonusService, chartService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetVoucherInformation(id, code));
    }

    [Fact]
    public void StoreController_GetVoucherInformation_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        string code = "";
        A.CallTo(() => storeService.GetVoucherItemByCode(id, code))
            .Throws(new InvalidParameterException());
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.GetVoucherInformation(id, code);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<VoucherItemExtraModel>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StoreController_GetColumnChartByStoreId_ReturnOK()
    {
        // Arrange
        string id = "";
        DateOnly fromDate = DateOnly.FromDateTime(DateTime.Now);
        DateOnly toDate = DateOnly.FromDateTime(DateTime.Now);
        bool? isAsc = null;
        A.CallTo(() => chartService.GetColumnChart
        (id, fromDate, toDate, isAsc, Role.Store)).Returns(new());
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.GetColumnChartByStoreId(id, fromDate, toDate, isAsc);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StoreController_GetColumnChartByStoreId_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        DateOnly fromDate = DateOnly.FromDateTime(DateTime.Now);
        DateOnly toDate = DateOnly.FromDateTime(DateTime.Now);
        bool? isAsc = null;
        A.CallTo(() => chartService.GetColumnChart
        (id, fromDate, toDate, isAsc, Role.Store))
            .Throws(new InvalidParameterException());
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.GetColumnChartByStoreId(id, fromDate, toDate, isAsc);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StoreController_GetHistoryTransactionByStoreId_ReturnOK()
    {
        // Arrange
        string id = "";
        List<StoreTransactionType> typeIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.GetHistoryTransactionByStoreId(id, typeIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<StoreTransactionModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StoreController_GetHistoryTransactionByStoreId_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        List<StoreTransactionType> typeIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StoreController
            (storeService, bonusService, chartService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetHistoryTransactionByStoreId(id, typeIds, state, paging));
    }

    [Fact]
    public void StoreController_GetHistoryTransactionByStoreId_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        List<StoreTransactionType> typeIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.GetHistoryTransactionByStoreId(id, typeIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<StoreTransactionModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StoreController_GetLineChartByStoreId_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => chartService.GetLineChart(id, Role.Store)).Returns(new());
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.GetLineChartByStoreId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StoreController_GetLineChartByStoreId_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => chartService.GetLineChart(id, Role.Store))
            .Throws(new InvalidParameterException());
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.GetLineChartByStoreId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StoreController_GetTitleByStoreId_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => chartService.GetTitleStore(id)).Returns(new());
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.GetTitleByStoreId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StoreController_GetTitleByStoreId_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => chartService.GetTitleStore(id))
            .Throws(new InvalidParameterException());
        var controller = new StoreController
            (storeService, bonusService, chartService);

        // Act
        var result = controller.GetTitleByStoreId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }
}
