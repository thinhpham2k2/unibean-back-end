using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.VoucherItems;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class VoucherItemControllerTest
{
    private readonly IVoucherItemService voucherItemService;

    public VoucherItemControllerTest()
    {
        voucherItemService = A.Fake<IVoucherItemService>();
    }

    [Fact]
    public void VoucherItemController_GetList_ReturnOK()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> campaignIds = new();
        List<string> voucherIds = new();
        List<string> typeIds = new();
        List<string> studentIds = new();
        bool? isLocked = null;
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new VoucherItemController(voucherItemService);

        // Act
        var result = controller.GetList
            (brandIds, campaignIds, voucherIds, typeIds, studentIds, isLocked, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<VoucherItemModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void VoucherItemController_GetList_ReturnBadRequest1()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> campaignIds = new();
        List<string> voucherIds = new();
        List<string> typeIds = new();
        List<string> studentIds = new();
        bool? isLocked = null;
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new VoucherItemController(voucherItemService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetList
            (brandIds, campaignIds, voucherIds, typeIds, studentIds, isLocked, state, paging));
    }

    [Fact]
    public void VoucherItemController_GetList_ReturnBadRequest2()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> campaignIds = new();
        List<string> voucherIds = new();
        List<string> typeIds = new();
        List<string> studentIds = new();
        bool? isLocked = null;
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new VoucherItemController(voucherItemService);

        // Act
        var result = controller.GetList
            (brandIds, campaignIds, voucherIds, typeIds, studentIds, isLocked, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<VoucherItemModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void VoucherItemController_GetById_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => voucherItemService.GetById(id)).Returns(new());
        var controller = new VoucherItemController(voucherItemService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void VoucherItemController_GetById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => voucherItemService.GetById(id))
            .Throws(new InvalidParameterException());
        var controller = new VoucherItemController(voucherItemService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void VoucherItemController_Create_ReturnOK()
    {
        // Arrange
        CreateVoucherItemModel creation = new();
        A.CallTo(() => voucherItemService.Add(creation)).Returns(new());
        var controller = new VoucherItemController(voucherItemService);

        // Act
        var result = controller.Create(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(FileContentResult));
        Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            result.GetType().GetProperty("ContentType")?.GetValue(result));
    }

    [Fact]
    public void VoucherItemController_Create_ReturnBadRequest()
    {
        // Arrange
        CreateVoucherItemModel creation = new();
        var controller = new VoucherItemController(voucherItemService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.Create(creation));
    }

    [Fact]
    public void VoucherItemController_Delete_ReturnNoContent()
    {
        // Arrange
        string id = "";
        var controller = new VoucherItemController(voucherItemService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StatusCodeResult));
        Assert.Equal(StatusCodes.Status204NoContent,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void VoucherItemController_Delete_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => voucherItemService.Delete(id))
            .Throws(new InvalidParameterException());
        var controller = new VoucherItemController(voucherItemService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void VoucherItemController_GetTemplate_ReturnOK()
    {
        // Arrange
        var controller = new VoucherItemController(voucherItemService);

        // Act
        var result = controller.GetTemplate();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(FileContentResult));
        Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            result.GetType().GetProperty("ContentType")?.GetValue(result));
    }

    [Fact]
    public void VoucherItemController_ImportTemplate_ReturnOK()
    {
        // Arrange
        InsertVoucherItemModel insert = new();
        A.CallTo(() => voucherItemService.AddTemplate(insert)).Returns<MemoryStream>(new());
        var controller = new VoucherItemController(voucherItemService);

        // Act
        var result = controller.ImportTemplate(insert);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            result.Result.GetType().GetProperty("ContentType")?.GetValue(result.Result));
    }

    [Fact]
    public void VoucherItemController_ImportTemplate_ReturnBadRequest1()
    {
        // Arrange
        InsertVoucherItemModel insert = new();
        var controller = new VoucherItemController(voucherItemService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act
        var result = controller.ImportTemplate(insert);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(typeof(InvalidParameterException).ToString(),
            result.Exception?.InnerException?.GetType().ToString());
    }

    [Fact]
    public void VoucherItemController_ImportTemplate_ReturnBadRequest2()
    {
        // Arrange
        InsertVoucherItemModel insert = new();
        A.CallTo(() => voucherItemService.AddTemplate(insert))
            .Throws(new InvalidParameterException());
        var controller = new VoucherItemController(voucherItemService);

        // Act
        var result = controller.ImportTemplate(insert);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
}
