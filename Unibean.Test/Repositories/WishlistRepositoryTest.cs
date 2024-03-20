using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class WishlistRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Wishlists.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.Wishlists.Add(
                new Wishlist()
                {
                    Id = i.ToString(),
                    StudentId = i.ToString(),
                    BrandId = i.ToString(),
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
    public async void WishlistRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new WishlistRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Wishlist>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void WishlistRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new WishlistRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.Wishlists.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void WishlistRepository_GetAll()
    {
        // Arrange
        List<string> studentIds = new();
        List<string> brandIds = new();
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new WishlistRepository(dbContext);

        // Act
        var result = repository.GetAll(studentIds, brandIds, state,
            propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<Wishlist>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void WishlistRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new WishlistRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Wishlist>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void WishlistRepository_GetByStudentAndBrand()
    {
        // Arrange
        string id = "1";
        string studentId = "1";
        string brandId = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new WishlistRepository(dbContext);

        // Act
        var result = repository.GetByStudentAndBrand(studentId, brandId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Wishlist>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void WishlistRepository_Update()
    {
        // Arrange
        string id = "1";
        string description = "description";
        var dbContext = await UnibeanDBContext();
        var repository = new WishlistRepository(dbContext);

        // Act
        var existingAccount = await dbContext.Wishlists.FindAsync(id);
        existingAccount.Description = description;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Wishlist>();
        Assert.Equal(id, result.Id);
        Assert.Equal(description, result.Description);
    }
}
