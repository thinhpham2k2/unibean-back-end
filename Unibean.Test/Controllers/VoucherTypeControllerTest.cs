using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.VoucherTypes;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class VoucherTypeControllerTest
{
    private readonly IVoucherTypeService voucherTypeService;

    public VoucherTypeControllerTest()
    {
        voucherTypeService = A.Fake<IVoucherTypeService>();
    }

    [Fact]
    public void VoucherTypeController_GetList_ReturnOK()
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
        var controller = new VoucherTypeController(voucherTypeService);

        // Act
        var result = controller.GetList(state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<VoucherTypeModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void VoucherTypeController_GetList_ReturnBadRequest1()
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
        var controller = new VoucherTypeController(voucherTypeService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetList(state, paging));
    }

    [Fact]
    public void VoucherTypeController_GetList_ReturnBadRequest2()
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
        var controller = new VoucherTypeController(voucherTypeService);

        // Act
        var result = controller.GetList(state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<VoucherTypeModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void VoucherTypeController_GetById_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => voucherTypeService.GetById(id)).Returns(new());
        var controller = new VoucherTypeController(voucherTypeService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void VoucherTypeController_GetById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => voucherTypeService.GetById(id)).Throws(new InvalidParameterException());
        var controller = new VoucherTypeController(voucherTypeService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void VoucherTypeController_Create_ReturnCreated()
    {
        // Arrange
        CreateVoucherTypeModel create = new();
        A.CallTo(() => voucherTypeService.Add(create)).Returns<VoucherTypeExtraModel>(new());
        var controller = new VoucherTypeController(voucherTypeService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status201Created,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void VoucherTypeController_Create_ReturnBadRequest1()
    {
        // Arrange
        CreateVoucherTypeModel create = new();
        var controller = new VoucherTypeController(voucherTypeService);
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
    public void VoucherTypeController_Create_ReturnBadRequest2()
    {
        // Arrange
        CreateVoucherTypeModel create = new();
        A.CallTo(() => voucherTypeService.Add(create))
        .Throws(new InvalidParameterException());
        var controller = new VoucherTypeController(voucherTypeService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void VoucherTypeController_Create_ReturnNotFound()
    {
        // Arrange
        CreateVoucherTypeModel create = new();
        A.CallTo(() => voucherTypeService.Add(create)).Returns<VoucherTypeExtraModel>(null);
        var controller = new VoucherTypeController(voucherTypeService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void VoucherTypeController_Update_ReturnOK()
    {
        // Arrange
        string id = "";
        UpdateVoucherTypeModel update = new();
        A.CallTo(() => voucherTypeService.Update(id, update)).Returns<VoucherTypeExtraModel>(new());
        var controller = new VoucherTypeController(voucherTypeService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void VoucherTypeController_Update_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        UpdateVoucherTypeModel update = new();
        var controller = new VoucherTypeController(voucherTypeService);
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
    public void VoucherTypeController_Update_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        UpdateVoucherTypeModel update = new();
        A.CallTo(() => voucherTypeService.Update(id, update))
        .Throws(new InvalidParameterException());
        var controller = new VoucherTypeController(voucherTypeService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void VoucherTypeController_Update_ReturnNotFound()
    {
        // Arrange
        string id = "";
        UpdateVoucherTypeModel update = new();
        A.CallTo(() => voucherTypeService.Update(id, update)).Returns<VoucherTypeExtraModel>(null);
        var controller = new VoucherTypeController(voucherTypeService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void VoucherTypeController_Delete_ReturnNoContent()
    {
        // Arrange
        string id = "";
        var controller = new VoucherTypeController(voucherTypeService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StatusCodeResult));
        Assert.Equal(StatusCodes.Status204NoContent,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void VoucherTypeController_Delete_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => voucherTypeService.Delete(id)).Throws(new InvalidParameterException());
        var controller = new VoucherTypeController(voucherTypeService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }
}
