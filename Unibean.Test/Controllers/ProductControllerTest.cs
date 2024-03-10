using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.Products;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class ProductControllerTest
{
    private readonly IProductService productService;

    public ProductControllerTest()
    {
        productService = A.Fake<IProductService>();
    }

    [Fact]
    public void ProductController_GetList_ReturnOK()
    {
        // Arrange
        List<string> categoryIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new ProductController(productService);

        // Act
        var result = controller.GetList(categoryIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<ProductModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void ProductController_GetList_ReturnBadRequest()
    {
        // Arrange
        List<string> categoryIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new ProductController(productService);

        // Act
        var result = controller.GetList(categoryIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<ProductModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void ProductController_GetById_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => productService.GetById(id)).Returns(new());
        var controller = new ProductController(productService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void ProductController_GetById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => productService.GetById(id)).Throws(new InvalidParameterException());
        var controller = new ProductController(productService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void ProductController_Create_ReturnCreated()
    {
        // Arrange
        CreateProductModel create = new();
        A.CallTo(() => productService.Add(create)).Returns<ProductExtraModel>(new());
        var controller = new ProductController(productService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status201Created,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void ProductController_Create_ReturnBadRequest1()
    {
        // Arrange
        CreateProductModel create = new();
        var controller = new ProductController(productService);
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
    public void ProductController_Create_ReturnBadRequest2()
    {
        // Arrange
        CreateProductModel create = new();
        A.CallTo(() => productService.Add(create))
        .Throws(new InvalidParameterException());
        var controller = new ProductController(productService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void ProductController_Create_ReturnNotFound()
    {
        // Arrange
        CreateProductModel create = new();
        A.CallTo(() => productService.Add(create)).Returns<ProductExtraModel>(null);
        var controller = new ProductController(productService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void ProductController_Update_ReturnOK()
    {
        // Arrange
        string id = "";
        UpdateProductModel update = new();
        A.CallTo(() => productService.Update(id, update)).Returns<ProductExtraModel>(new());
        var controller = new ProductController(productService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void ProductController_Update_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        UpdateProductModel update = new();
        var controller = new ProductController(productService);
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
    public void ProductController_Update_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        UpdateProductModel update = new();
        A.CallTo(() => productService.Update(id, update))
        .Throws(new InvalidParameterException());
        var controller = new ProductController(productService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void ProductController_Update_ReturnNotFound()
    {
        // Arrange
        string id = "";
        UpdateProductModel update = new();
        A.CallTo(() => productService.Update(id, update)).Returns<ProductExtraModel>(null);
        var controller = new ProductController(productService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void ProductController_Delete_ReturnNoContent()
    {
        // Arrange
        string id = "";
        var controller = new ProductController(productService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StatusCodeResult));
        Assert.Equal(StatusCodes.Status204NoContent,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void ProductController_Delete_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => productService.Delete(id)).Throws(new InvalidParameterException());
        var controller = new ProductController(productService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }
}
