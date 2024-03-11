using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.Vouchers;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class VoucherControllerTest
{
    private readonly IVoucherService voucherService;

    public VoucherControllerTest()
    {
        voucherService = A.Fake<IVoucherService>();
    }

    [Fact]
    public void VoucherController_GetList_ReturnOK()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> typeIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new VoucherController
            (voucherService);

        // Act
        var result = controller.GetList(brandIds, typeIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<VoucherModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void VoucherController_GetList_ReturnBadRequest1()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> typeIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new VoucherController
            (voucherService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetList(brandIds, typeIds, state, paging));
    }

    [Fact]
    public void VoucherController_GetList_ReturnBadRequest2()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> typeIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new VoucherController
            (voucherService);

        // Act
        var result = controller.GetList(brandIds, typeIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<VoucherModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void VoucherController_GetById_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => voucherService.GetById(id)).Returns(new());
        var controller = new VoucherController
            (voucherService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void VoucherController_GetById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => voucherService.GetById(id)).Throws(new InvalidParameterException());
        var controller = new VoucherController
            (voucherService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void VoucherController_Create_ReturnCreated()
    {
        // Arrange
        CreateVoucherModel create = new();
        A.CallTo(() => voucherService.Add(create)).Returns<VoucherExtraModel>(new());
        var controller = new VoucherController
            (voucherService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status201Created,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void VoucherController_Create_ReturnBadRequest1()
    {
        // Arrange
        CreateVoucherModel create = new();
        var controller = new VoucherController
            (voucherService);
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
    public void VoucherController_Create_ReturnBadRequest2()
    {
        // Arrange
        CreateVoucherModel create = new();
        A.CallTo(() => voucherService.Add(create))
        .Throws(new InvalidParameterException());
        var controller = new VoucherController
            (voucherService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void VoucherController_Create_ReturnNotFound()
    {
        // Arrange
        CreateVoucherModel create = new();
        A.CallTo(() => voucherService.Add(create)).Returns<VoucherExtraModel>(null);
        var controller = new VoucherController
            (voucherService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void VoucherController_Update_ReturnOK()
    {
        // Arrange
        string id = "";
        UpdateVoucherModel update = new();
        A.CallTo(() => voucherService.Update(id, update)).Returns<VoucherExtraModel>(new());
        var controller = new VoucherController
            (voucherService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void VoucherController_Update_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        UpdateVoucherModel update = new();
        var controller = new VoucherController
            (voucherService);
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
    public void VoucherController_Update_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        UpdateVoucherModel update = new();
        A.CallTo(() => voucherService.Update(id, update))
        .Throws(new InvalidParameterException());
        var controller = new VoucherController
            (voucherService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void VoucherController_Update_ReturnNotFound()
    {
        // Arrange
        string id = "";
        UpdateVoucherModel update = new();
        A.CallTo(() => voucherService.Update(id, update)).Returns<VoucherExtraModel>(null);
        var controller = new VoucherController
            (voucherService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void VoucherController_Delete_ReturnNoContent()
    {
        // Arrange
        string id = "";
        var controller = new VoucherController
            (voucherService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StatusCodeResult));
        Assert.Equal(StatusCodes.Status204NoContent,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void VoucherController_Delete_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => voucherService.Delete(id)).Throws(new InvalidParameterException());
        var controller = new VoucherController
            (voucherService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }
}
