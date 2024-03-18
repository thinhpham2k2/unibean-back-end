using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.Test.Repositories;

public class CampaignRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Campaigns.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.Campaigns.Add(
                new Campaign()
                {
                    Id = i.ToString(),
                    BrandId = i.ToString(),
                    Brand = new()
                    {
                        Id = i.ToString(),
                        TotalIncome = 0,
                        TotalSpending = 0,
                        Status = true,
                        Wallets = new List<Wallet>()
                        {
                            new()
                            {
                                Id = Ulid.NewUlid().ToString(),
                                Balance = 1000,
                                Type = WalletType.Green,
                            }
                        }
                    },
                    TypeId = i.ToString(),
                    CampaignName = "campaignName" + i,
                    Image = "image" + i,
                    ImageName = "imageName" + i,
                    File = "file" + i,
                    FileName = "fileName" + i,
                    Condition = "condition" + i,
                    Link = "link" + i,
                    StartOn = DateOnly.FromDateTime(DateTime.Now),
                    EndOn = DateOnly.FromDateTime(DateTime.Now),
                    Duration = 1,
                    TotalIncome = 0,
                    TotalSpending = 0,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Description = "description" + i,
                    Status = true,
                    Wallets = new List<Wallet>()
                    {
                        new()
                        {
                            Id = Ulid.NewUlid().ToString(),
                            Balance = 1000,
                            Type = WalletType.Green,
                        }
                    },
                    CampaignActivities = new List<CampaignActivity>()
                    {
                        new()
                        {
                            Id = i.ToString(),
                            State = CampaignState.Active,
                        }
                    },
                    CampaignDetails = new List<CampaignDetail>()
                    {
                        new()
                        {
                            Id = i.ToString(),
                            VoucherItems = new List<VoucherItem>()
                            {
                                new()
                                {
                                    Id = i.ToString(),
                                    CampaignDetailId = i.ToString(),
                                    IsLocked = true,
                                    IsBought = true,
                                    IsUsed = false,
                                    State = true,
                                    Status = true,
                                    Activities = new List<Activity>()
                                    {
                                        new()
                                        {
                                            Id = i.ToString(),
                                            Type = Type.Buy,
                                            Student = new()
                                            {
                                                Id = i.ToString(),
                                                Account = new() 
                                                {
                                                    Id = i.ToString(),
                                                    Email = "email" + i.ToString()
                                                },
                                                Wallets = new List<Wallet>()
                                                {
                                                    new()
                                                    {
                                                        Id = Ulid.NewUlid().ToString(),
                                                        Balance = 1000,
                                                        Type = WalletType.Green,
                                                    },
                                                    new()
                                                    {
                                                        Id = Ulid.NewUlid().ToString(),
                                                        Balance = 1000,
                                                        Type = WalletType.Red,
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
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
    public async void CampaignRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id,
            BrandId = "1",
            TotalIncome = 100,
            DateCreated = DateTime.Now,
            Status = true,
            CampaignStores = new List<CampaignStore>
            {
                new()
                {
                    Id = id,
                }
            },
            CampaignMajors = new List<CampaignMajor>
            {
                new()
                {
                    Id = id,
                }
            },
            CampaignCampuses = new List<CampaignCampus>
            {
                new()
                {
                    Id = id,
                }
            }
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Campaign>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void CampaignRepository_AllToClosed()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignRepository(dbContext);

        // Act
        var result = repository.AllToClosed(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<string>>();
        Assert.Single(result);
    }

    [Fact]
    public async void CampaignRepository_CountCampaign()
    {
        // Arrange
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignRepository(dbContext);

        // Act
        var result = repository.CountCampaign();

        // Assert
        result.Should().Be(10);
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public async void CampaignRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.Campaigns.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void CampaignRepository_ExpiredToClosed()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignRepository(dbContext);

        // Act & Assert
        Assert.True(repository.ExpiredToClosed(id));
    }

    [Fact]
    public async void CampaignRepository_GetAll()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> typeIds = new();
        List<string> storeIds = new();
        List<string> majorIds = new();
        List<string> campusIds = new();
        List<CampaignState> stateIds = new();
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignRepository(dbContext);

        // Act
        var result = repository.GetAll(brandIds, typeIds, storeIds,
            majorIds, campusIds, stateIds, propertySort, isAsc,
            search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<Campaign>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void CampaignRepository_GetAllEnded()
    {
        // Arrange
        List<CampaignState> stateIds = new();
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignRepository(dbContext);

        // Act
        var result = repository.GetAllEnded(stateIds);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<Campaign>>();
        result.Count.Should().Be(0);
        result.Count.Should().BeInRange(0, 10);
    }

    [Fact]
    public async void CampaignRepository_GetAllExpired()
    {
        // Arrange
        List<CampaignState> stateIds = new();
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignRepository(dbContext);

        // Act
        var result = repository.GetAllExpired(stateIds, date);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<Campaign>>();
        result.Count.Should().Be(0);
        result.Count.Should().BeInRange(0, 10);
    }

    [Fact]
    public async void CampaignRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Campaign>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void CampaignRepository_Update()
    {
        // Arrange
        string id = "1";
        string campaignName = "campaignName";
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignRepository(dbContext);

        // Act
        var existingAccount = await dbContext.Campaigns.FindAsync(id);
        existingAccount.CampaignName = campaignName;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Campaign>();
        Assert.Equal(id, result.Id);
        Assert.Equal(campaignName, result.CampaignName);
    }
}
