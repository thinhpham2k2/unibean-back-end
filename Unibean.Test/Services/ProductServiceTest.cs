using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Products;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class ProductServiceTest
{
    private readonly IProductRepository productRepository;

    private readonly IFireBaseService fireBaseService;

    private readonly IImageRepository imageRepository;

    public ProductServiceTest()
    {
        productRepository = A.Fake<IProductRepository>();
        fireBaseService = A.Fake<IFireBaseService>();
        imageRepository = A.Fake<IImageRepository>();
    }

    [Fact]
    public void ProductService_Add()
    {
        // Arrange
        string id = "id";
        CreateProductModel creation = A.Fake<CreateProductModel>();
        A.CallTo(() => productRepository.Add(A<Product>.Ignored)).Returns(new()
        {
            Id = id
        });
        var service = new ProductService
            (productRepository, fireBaseService, imageRepository);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ProductExtraModel>));
        Assert.Equal(id, result.Result.Id);
    }

    [Fact]
    public void ProductService_Delete()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => productRepository.GetById(id)).Returns(new()
        {
            OrderDetails = new List<OrderDetail>()
        });
        A.CallTo(() => productRepository.Delete(id));
        var service = new ProductService
            (productRepository, fireBaseService, imageRepository);

        // Act & Assert
        service.Delete(id);
    }

    [Fact]
    public void ProductService_GetAll()
    {
        // Arrange
        List<string> categoryIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<Product> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => productRepository.GetAll(categoryIds, state, propertySort,
            isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new ProductService
            (productRepository, fireBaseService, imageRepository);

        // Act
        var result = service.GetAll
            (categoryIds, state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<ProductModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void ProductService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => productRepository.GetById(id))
        .Returns(new()
        {
            Id = id
        });
        var service = new ProductService
            (productRepository, fireBaseService, imageRepository);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ProductExtraModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void ProductService_Update()
    {
        // Arrange
        string id = "id";
        string productName = "productName";
        UpdateProductModel update = A.Fake<UpdateProductModel>();
        A.CallTo(() => productRepository.GetById(id));
        A.CallTo(() => productRepository.Update(A<Product>.Ignored))
        .Returns(new()
        {
            Id = id,
            ProductName = productName
        });
        var service = new ProductService
            (productRepository, fireBaseService, imageRepository);

        // Act
        var result = service.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ProductExtraModel>));
        Assert.Equal(id, result.Result.Id);
        Assert.Equal(productName, result.Result.ProductName);
    }
}
