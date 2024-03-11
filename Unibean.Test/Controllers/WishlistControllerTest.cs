using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.Wishlists;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class WishlistControllerTest
{
    private readonly IWishlistService wishlistService;

    public WishlistControllerTest()
    {
        wishlistService = A.Fake<IWishlistService>();
    }

    [Fact]
    public void WishlistController_GetList_ReturnOK()
    {
        // Arrange
        List<string> studentIds = new();
        List<string> brandIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new WishlistController(wishlistService);

        // Act
        var result = controller.GetList(studentIds, brandIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<WishlistModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void WishlistController_GetList_ReturnBadRequest1()
    {
        // Arrange
        List<string> studentIds = new();
        List<string> brandIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new WishlistController(wishlistService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetList(studentIds, brandIds, state, paging));
    }

    [Fact]
    public void WishlistController_GetList_ReturnBadRequest2()
    {
        // Arrange
        List<string> studentIds = new();
        List<string> brandIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new WishlistController(wishlistService);

        // Act
        var result = controller.GetList(studentIds, brandIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<WishlistModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void WishlistController_Create_ReturnCreated()
    {
        // Arrange
        UpdateWishlistModel update = new();
        A.CallTo(() => wishlistService.UpdateWishlist(update)).Returns(new());
        var controller = new WishlistController(wishlistService);

        // Act
        var result = controller.Update(update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status201Created,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void WishlistController_Create_ReturnBadRequest1()
    {
        // Arrange
        UpdateWishlistModel update = new();
        var controller = new WishlistController(wishlistService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.Update(update));
    }

    [Fact]
    public void WishlistController_Create_ReturnBadRequest2()
    {
        // Arrange
        UpdateWishlistModel update = new();
        A.CallTo(() => wishlistService.UpdateWishlist(update))
            .Throws(new InvalidParameterException());
        var controller = new WishlistController(wishlistService);

        // Act
        var result = controller.Update(update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void WishlistController_Create_ReturnNotFound()
    {
        // Arrange
        UpdateWishlistModel update = new();
        A.CallTo(() => wishlistService.UpdateWishlist(update))
            .Returns(null);
        var controller = new WishlistController(wishlistService);

        // Act
        var result = controller.Update(update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }
}
