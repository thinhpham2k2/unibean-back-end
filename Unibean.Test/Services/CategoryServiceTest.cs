using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Categories;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class CategoryServiceTest
{
    private readonly ICategoryRepository categoryRepository;

    private readonly IFireBaseService fireBaseService;

    public CategoryServiceTest()
    {
        categoryRepository = A.Fake<ICategoryRepository>();
        fireBaseService = A.Fake<IFireBaseService>();
    }

    [Fact]
    public void CategoryService_Add()
    {
        // Arrange
        string id = "id";
        CreateCategoryModel creation = A.Fake<CreateCategoryModel>();
        A.CallTo(() => categoryRepository.Add(A<Category>.Ignored)).Returns(new()
        {
            Id = id
        });
        var service = new CategoryService(categoryRepository, fireBaseService);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<CategoryExtraModel>));
        Assert.Equal(id, result.Result.Id);
    }

    [Fact]
    public void CategoryService_Delete()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => categoryRepository.GetById(id)).Returns(new()
        {
            Id = id,
            Products = new List<Product>(),
        });
        A.CallTo(() => categoryRepository.Delete(id));
        var service = new CategoryService(categoryRepository, fireBaseService);

        // Act & Assert
        service.Delete(id);
    }

    [Fact]
    public void CategoryService_GetAll()
    {
        // Arrange
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<Category> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => categoryRepository.GetAll(state, propertySort, isAsc, search, page, limit))
            .Returns(pagedResultModel);
        var service = new CategoryService(categoryRepository, fireBaseService);

        // Act
        var result = service.GetAll(state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<CategoryModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void CategoryService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => categoryRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        var service = new CategoryService(categoryRepository, fireBaseService);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(CategoryExtraModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void CategoryService_Update()
    {
        // Arrange
        string id = "id";
        string categoryName = "categoryName";
        UpdateCategoryModel update = A.Fake<UpdateCategoryModel>();
        A.CallTo(() => categoryRepository.GetById(id));
        A.CallTo(() => categoryRepository.Update(A<Category>.Ignored))
            .Returns(new()
            {
                Id = id,
                CategoryName = categoryName
            });
        var service = new CategoryService(categoryRepository, fireBaseService);

        // Act
        var result = service.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<CategoryExtraModel>));
        Assert.Equal(id, result.Result.Id);
        Assert.Equal(categoryName, result.Result.CategoryName);
    }
}
