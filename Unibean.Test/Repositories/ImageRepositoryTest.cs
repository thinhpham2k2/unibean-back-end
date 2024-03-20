using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class ImageRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Images.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.Images.Add(
                new Image()
                {
                    Id = i.ToString(),
                    ProductId = i.ToString(),
                    Url = "url" + i,
                    FileName = "fileName" + i,
                    IsCover = true,
                    DateCreated = DateTime.Now,
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
    public async void ImageRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new ImageRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Image>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void ImageRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new ImageRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.Images.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void ImageRepository_GetAll()
    {
        // Arrange
        List<string> productIds = new();
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new ImageRepository(dbContext);

        // Act
        var result = repository.GetAll(productIds, state, propertySort,
            isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<Image>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void ImageRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new ImageRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Image>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void ImageRepository_Update()
    {
        // Arrange
        string id = "1";
        string description = "description";
        var dbContext = await UnibeanDBContext();
        var repository = new ImageRepository(dbContext);

        // Act
        var existingAccount = await dbContext.Images.FindAsync(id);
        existingAccount.Description = description;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Image>();
        Assert.Equal(id, result.Id);
        Assert.Equal(description, result.Description);
    }
}
