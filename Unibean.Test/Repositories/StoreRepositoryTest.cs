using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class StoreRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Stores.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.Stores.Add(
                new Store()
                {
                    Id = i.ToString(),
                    BrandId = i.ToString(),
                    AreaId = i.ToString(),
                    AccountId = i.ToString(),
                    StoreName = "campusName" + i,
                    OpeningHours = TimeOnly.MinValue,
                    ClosingHours = TimeOnly.MaxValue,
                    File = "file" + i,
                    FileName = "fileName" + i,
                    Address = "address" + i,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Description = "description" + i,
                    State = true,
                    Status = true,
                    CampaignStores = new List<CampaignStore>()
                    {
                        new()
                        {
                            Id = i.ToString(),
                            CampaignId = i.ToString(),
                            Campaign = new()
                            {
                                Id = i.ToString(),
                            }
                        }
                    }
                });
                await databaseContext.SaveChangesAsync();
            }
        }
        return databaseContext;
    }

    [Fact]
    public async void StoreRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new StoreRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Store>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void StoreRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new StoreRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.Stores.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void StoreRepository_GetAll()
    {
        // Arrange
        List<string> universityIds = new();
        List<string> areaIds = new();
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new StoreRepository(dbContext);

        // Act
        var result = repository.GetAll(universityIds, areaIds,
            state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<Store>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void StoreRepository_GetAllByCampaign()
    {
        // Arrange
        List<string> campaignIds = new()
        {
            "1"
        };
        List<string> universityIds = new();
        List<string> areaIds = new();
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new StoreRepository(dbContext);

        // Act
        var result = repository.GetAllByCampaign(campaignIds, universityIds,
            areaIds, state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<Store>>();
        Assert.Equal(0, result.RowCount);
    }

    [Fact]
    public async void StoreRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new StoreRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Store>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void StoreRepository_Update()
    {
        // Arrange
        string id = "1";
        string campusName = "campusName";
        var dbContext = await UnibeanDBContext();
        var repository = new StoreRepository(dbContext);

        // Act
        var existingAccount = await dbContext.Stores.FindAsync(id);
        existingAccount.StoreName = campusName;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Store>();
        Assert.Equal(id, result.Id);
        Assert.Equal(campusName, result.StoreName);
    }
}
