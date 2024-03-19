using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.Test.Repositories;

public class ActivityRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Activities.AnyAsync())
        {
            Array values = Enum.GetValues(typeof(Type));
            for (int i = 1; i <= 10; i++)
            {
                Random random = new();
                Type randomType = (Type)values.GetValue(random.Next(values.Length));
                databaseContext.Activities.Add(
                new Activity()
                {
                    Id = i.ToString(),
                    StoreId = i.ToString(),
                    StudentId = i.ToString(),
                    Student = new()
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
                            },
                            new()
                            {
                                Id = Ulid.NewUlid().ToString(),
                                Balance = 1000,
                                Type = WalletType.Red,
                            }
                        }
                    },
                    VoucherItemId = i.ToString(),
                    VoucherItem = new()
                    {
                        Id = i.ToString(),
                        CampaignDetail = new()
                        {
                            Id = Ulid.NewUlid().ToString(),
                            Price = 100,
                            Rate = 1,
                            CampaignId = Ulid.NewUlid().ToString(),
                            Campaign = new()
                            {
                                Id = Ulid.NewUlid().ToString(),
                                Status = true,
                                Wallets = new List<Wallet>()
                                {
                                    new()
                                    {
                                        Id = Ulid.NewUlid().ToString(),
                                        Type = WalletType.Green,
                                        Balance = 1000,
                                    }
                                }
                            },
                        }
                    },
                    Type = randomType,
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
    public async void ActivityRepository_Add_Buy()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new ActivityRepository(dbContext);

        // Act
        var existingItem = await dbContext.VoucherItems.FindAsync(1.ToString());
        var result = repository.Add(new()
        {
            Id = id,
            StudentId = 1.ToString(),
            Type = Type.Buy,
            VoucherItem = existingItem
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Activity>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void ActivityRepository_Add_Use()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new ActivityRepository(dbContext);

        // Act
        var existingItem = await dbContext.VoucherItems.FindAsync(1.ToString());
        var result = repository.Add(new()
        {
            Id = id,
            StudentId = 1.ToString(),
            Type = Type.Use,
            VoucherItem = existingItem
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Activity>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void ActivityRepository_CountParticipantToday()
    {
        // Arrange
        string storeId = "1";
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        var dbContext = await UnibeanDBContext();
        var repository = new ActivityRepository(dbContext);

        // Act
        var result = repository.CountParticipantToday(storeId, date);

        // Assert
        result.Should().Be(1);
        result.Should().BeInRange(1, 10);
    }

    [Fact]
    public async void ActivityRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new ActivityRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.Activities.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void ActivityRepository_GetAll()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> storeIds = new();
        List<string> studentIds = new();
        List<string> campaignIds = new();
        List<string> campaignDetailIds = new();
        List<string> voucherIds = new();
        List<string> voucherItemIds = new();
        List<Type> typeIds = new();
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new ActivityRepository(dbContext);

        // Act
        var result = repository.GetAll(brandIds, storeIds, studentIds,
            campaignIds, campaignDetailIds, voucherIds, voucherItemIds,
            typeIds, state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<Activity>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void ActivityRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new ActivityRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Activity>();
        Assert.Equal(id, result.Id);
        Assert.Equal(id, result.StoreId);
        Assert.Equal(id, result.StudentId);
    }

    [Fact]
    public async void ActivityRepository_GetList()
    {
        // Arrange
        List<string> storeIds = new();
        List<string> studentIds = new();
        List<string> voucherIds = new();
        string search = "";
        var dbContext = await UnibeanDBContext();
        var repository = new ActivityRepository(dbContext);

        // Act
        var result = repository.GetList(storeIds, studentIds,
            voucherIds, search);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<Activity>>();
        result.Count.Should().BeInRange(0, 10);
    }

    [Fact]
    public async void ActivityRepository_Update()
    {
        // Arrange
        string id = "1";
        string description = "description";
        var dbContext = await UnibeanDBContext();
        var repository = new ActivityRepository(dbContext);

        // Act
        var existingAccount = await dbContext.Activities.FindAsync(id);
        existingAccount.Description = description;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Activity>();
        Assert.Equal(id, result.Id);
        Assert.Equal(description, result.Description);
    }
}
