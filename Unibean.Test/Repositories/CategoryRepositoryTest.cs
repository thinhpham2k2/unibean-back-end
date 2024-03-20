using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class CategoryRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Categories.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.Categories.Add(
                new Category()
                {
                    Id = i.ToString(),
                    CategoryName = "categoryName" + i,
                    Image = "image" + i,
                    FileName = "fileName" + i,
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
    public async void CategoryRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new CategoryRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Category>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void CategoryRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new CategoryRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.Categories.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void CategoryRepository_GetAll()
    {
        // Arrange
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new CategoryRepository(dbContext);

        // Act
        var result = repository.GetAll(state, propertySort, isAsc,
            search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<Category>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void CategoryRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new CategoryRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Category>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void CategoryRepository_Update()
    {
        // Arrange
        string id = "1";
        string categoryName = "categoryName";
        var dbContext = await UnibeanDBContext();
        var repository = new CategoryRepository(dbContext);

        // Act
        var existingAccount = await dbContext.Categories.FindAsync(id);
        existingAccount.CategoryName = categoryName;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Category>();
        Assert.Equal(id, result.Id);
        Assert.Equal(categoryName, result.CategoryName);
    }
}
