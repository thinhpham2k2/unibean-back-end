using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Categories;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class CategoryControllerTest
{
    private readonly ICategoryService categoryService;

    public CategoryControllerTest()
    {
        categoryService = A.Fake<ICategoryService>();
    }

    [Fact]
    public void CategoryController_GetList_ReturnOK()
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
        var controller = new CategoryController(categoryService);

        // Act
        var result = controller.GetList(state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<CategoryModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CategoryController_GetList_ReturnBadRequest1()
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
        var controller = new CategoryController(categoryService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetList(state, paging));
    }

    [Fact]
    public void CategoryController_GetList_ReturnBadRequest2()
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
        var controller = new CategoryController(categoryService);

        // Act
        var result = controller.GetList(state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<CategoryModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CategoryController_GetById_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => categoryService.GetById(id)).Returns(new());
        var controller = new CategoryController(categoryService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void CategoryController_GetById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => categoryService.GetById(id))
            .Throws(new InvalidParameterException());
        var controller = new CategoryController(categoryService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void CategoryController_Create_ReturnCreated()
    {
        // Arrange
        CreateCategoryModel create = new();
        A.CallTo(() => categoryService.Add(create))
            .Returns<CategoryExtraModel>(new());
        var controller = new CategoryController(categoryService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status201Created,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CategoryController_Create_ReturnBadRequest1()
    {
        // Arrange
        CreateCategoryModel create = new();
        var controller = new CategoryController(categoryService);
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
    public void CategoryController_Create_ReturnBadRequest2()
    {
        // Arrange
        CreateCategoryModel create = new();
        A.CallTo(() => categoryService.Add(create))
            .Throws(new InvalidParameterException());
        var controller = new CategoryController(categoryService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CategoryController_Create_ReturnNotFound()
    {
        // Arrange
        CreateCategoryModel create = new();
        A.CallTo(() => categoryService.Add(create))
            .Returns<CategoryExtraModel>(null);
        var controller = new CategoryController(categoryService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CategoryController_Update_ReturnOK()
    {
        // Arrange
        string id = "";
        UpdateCategoryModel update = new();
        A.CallTo(() => categoryService.Update(id, update))
            .Returns<CategoryExtraModel>(new());
        var controller = new CategoryController(categoryService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CategoryController_Update_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        UpdateCategoryModel update = new();
        var controller = new CategoryController(categoryService);
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
    public void CategoryController_Update_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        UpdateCategoryModel update = new();
        A.CallTo(() => categoryService.Update(id, update))
            .Throws(new InvalidParameterException());
        var controller = new CategoryController(categoryService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CategoryController_Update_ReturnNotFound()
    {
        // Arrange
        string id = "";
        UpdateCategoryModel update = new();
        A.CallTo(() => categoryService.Update(id, update))
            .Returns<CategoryExtraModel>(null);
        var controller = new CategoryController(categoryService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void CategoryController_Delete_ReturnNoContent()
    {
        // Arrange
        string id = "";
        var controller = new CategoryController(categoryService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StatusCodeResult));
        Assert.Equal(StatusCodes.Status204NoContent,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void CategoryController_Delete_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => categoryService.Delete(id))
            .Throws(new InvalidParameterException());
        var controller = new CategoryController(categoryService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }
}
