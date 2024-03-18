using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class BrandRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Brands.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.Brands.Add(
                new Brand()
                {
                    Id = i.ToString(),
                    AccountId = i.ToString(),
                    BrandName = "brandName" + i,
                    Acronym = "acronym" + i,
                    Address = "address" + i,
                    CoverPhoto = "coverPhoto" + i,
                    CoverFileName = "coverFileName" + i,
                    Link = "link" + i,
                    OpeningHours = TimeOnly.MinValue,
                    ClosingHours = TimeOnly.MaxValue,
                    TotalIncome = 0,
                    TotalSpending = 0,
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
    public async void BrandRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new BrandRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Brand>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void BrandRepository_CountBrand()
    {
        // Arrange
        var dbContext = await UnibeanDBContext();
        var repository = new BrandRepository(dbContext);

        // Act
        var result = repository.CountBrand();

        // Assert
        result.Should().Be(10);
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public async void BrandRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new BrandRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.Brands.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void BrandRepository_GetAll()
    {
        // Arrange
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new BrandRepository(dbContext);

        // Act
        var result = repository.GetAll(state, propertySort, isAsc,
            search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<Brand>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void BrandRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new BrandRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Brand>();
        Assert.Equal(id, result.Id);
        Assert.Equal(id, result.AccountId);
    }

    [Fact]
    public async void BrandRepository_Update()
    {
        // Arrange
        string id = "1";
        string brandName = "brandName";
        var dbContext = await UnibeanDBContext();
        var repository = new BrandRepository(dbContext);

        // Act
        var existingAccount = await dbContext.Brands.FindAsync(id);
        existingAccount.BrandName = brandName;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Brand>();
        Assert.Equal(id, result.Id);
        Assert.Equal(brandName, result.BrandName);
    }
}
