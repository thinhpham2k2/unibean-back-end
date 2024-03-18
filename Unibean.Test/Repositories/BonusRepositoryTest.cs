using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class BonusRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Bonuses.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.Bonuses.Add(
                new Bonus()
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
                    Amount = 100,
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
    public async void BonusRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new BonusRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id,
            StudentId = 1.ToString(),
            BrandId = 1.ToString(),
            Amount = 100,
            Description = "description",
            State = true,
            Status = true,
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Bonus>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void BonusRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new BonusRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.Bonuses.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void BonusRepository_GetAll()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> storeIds = new();
        List<string> studentIds = new();
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new BonusRepository(dbContext);

        // Act
        var result = repository.GetAll(brandIds, storeIds, studentIds, 
            state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<Bonus>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void BonusRepository_GetList()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> storeIds = new();
        List<string> studentIds = new();
        string search = "";
        var dbContext = await UnibeanDBContext();
        var repository = new BonusRepository(dbContext);

        // Act
        var result = repository.GetList(brandIds, storeIds,
            studentIds, search);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<Bonus>>();
        result.Count.Should().BeInRange(0, 10);
    }

    [Fact]
    public async void BonusRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new BonusRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Bonus>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void BonusRepository_Update()
    {
        // Arrange
        string id = "1";
        string description = "description";
        var dbContext = await UnibeanDBContext();
        var repository = new BonusRepository(dbContext);

        // Act
        var existingAccount = await dbContext.Bonuses.FindAsync(id);
        existingAccount.Description = description;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Bonus>();
        Assert.Equal(id, result.Id);
        Assert.Equal(description, result.Description);
    }
}
