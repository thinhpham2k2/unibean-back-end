using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class ProductRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Products.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.Products.Add(
                new Product()
                {
                    Id = i.ToString(),
                    CategoryId = i.ToString(),
                    ProductName = "productName" + i,
                    Price = 100,
                    Weight = 0.2M,
                    Quantity = i,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Description = "description" + i,
                    State = true,
                    Status = true,
                });
                await databaseContext.SaveChangesAsync();
            }
        }
        return databaseContext;
    }

    [Fact]
    public async void ProductRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new ProductRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Product>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void ProductRepository_CountProduct()
    {
        // Arrange
        var dbContext = await UnibeanDBContext();
        var repository = new ProductRepository(dbContext);

        // Act
        var result = repository.CountProduct();

        // Assert
        result.Should().Be(10);
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public async void ProductRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new ProductRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.Products.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void ProductRepository_GetAll()
    {
        // Arrange
        List<string> categoryIds = new();
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new ProductRepository(dbContext);

        // Act
        var result = repository.GetAll(categoryIds, state,
            propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<Product>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void ProductRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new ProductRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Product>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void ProductRepository_Update()
    {
        // Arrange
        string id = "1";
        string brandName = "brandName";
        var dbContext = await UnibeanDBContext();
        var repository = new ProductRepository(dbContext);

        // Act
        var existingAccount = await dbContext.Products.FindAsync(id);
        existingAccount.ProductName = brandName;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Product>();
        Assert.Equal(id, result.Id);
        Assert.Equal(brandName, result.ProductName);
    }
}
