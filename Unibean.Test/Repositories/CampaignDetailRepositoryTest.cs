using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class CampaignDetailRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.CampaignDetails.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.CampaignDetails.Add(
                new CampaignDetail()
                {
                    Id = i.ToString(),
                    VoucherId = i.ToString(),
                    CampaignId = i.ToString(),
                    Campaign = new()
                    {
                        Id = i.ToString(),
                        CampaignStores = new List<CampaignStore>()
                        {
                            new()
                            {
                                Id = i.ToString(),
                                Store = new()
                                {
                                    Id = i.ToString(),
                                }
                            }
                        }
                    },
                    VoucherItems = new List<VoucherItem>()
                    {
                        new()
                        {
                            Id = i.ToString(),
                            CampaignDetailId = i.ToString(),
                            IsLocked = true,
                            IsBought = false,
                            IsUsed = false,
                            State = true,
                            Status = true,
                        }
                    },
                    Price = 10,
                    Rate = 1,
                    Quantity = 10,
                    FromIndex = 1,
                    ToIndex = 10,
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
    public async void CampaignDetailRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignDetailRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CampaignDetail>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void CampaignDetailRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignDetailRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.CampaignDetails.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void CampaignDetailRepository_GetAll()
    {
        // Arrange
        List<string> campaignIds = new();
        List<string> typeIds = new();
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignDetailRepository(dbContext);

        // Act
        var result = repository.GetAll(campaignIds, typeIds, state, 
            propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<CampaignDetail>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void CampaignDetailRepository_GetAllByStore()
    {
        // Arrange
        string storeId = "1";
        List<string> campaignIds = new();
        List<string> typeIds = new();
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignDetailRepository(dbContext);

        // Act
        var result = repository.GetAllByStore(storeId, campaignIds, typeIds, state, 
            propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<CampaignDetail>>();
        Assert.Equal(1, result.RowCount);
    }

    [Fact]
    public async void CampaignDetailRepository_GetAllVoucherItemByCampaignDetail()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignDetailRepository(dbContext);

        // Act
        var result = repository.GetAllVoucherItemByCampaignDetail(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<string>>();
        Assert.Single(result);
    }

    [Fact]
    public async void CampaignDetailRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignDetailRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CampaignDetail>();
        Assert.Equal(id, result.Id);
        Assert.Equal(id, result.VoucherId);
        Assert.Equal(id, result.CampaignId);
    }

    [Fact]
    public async void CampaignDetailRepository_Update()
    {
        // Arrange
        string id = "1";
        string description = "description";
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignDetailRepository(dbContext);

        // Act
        var existingAccount = await dbContext.CampaignDetails.FindAsync(id);
        existingAccount.Description = description;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CampaignDetail>();
        Assert.Equal(id, result.Id);
        Assert.Equal(description, result.Description);
    }
}
