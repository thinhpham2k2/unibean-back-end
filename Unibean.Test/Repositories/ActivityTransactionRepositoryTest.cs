using DocumentFormat.OpenXml.Office2010.Excel;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.Test.Repositories;

public class ActivityTransactionRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.ActivityTransactions.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.ActivityTransactions.Add(
                new ActivityTransaction()
                {
                    Id = i.ToString(),
                    ActivityId = i.ToString(),
                    Activity = new()
                    {
                        Id = i.ToString(),
                        Type = Type.Buy,
                        DateCreated = DateTime.UtcNow,
                    },
                    WalletId = i.ToString(),
                    Wallet = new()
                    {
                        Id = i.ToString(),
                        Type = WalletType.Green,
                        State = true,
                        Status = true,
                    },
                    Amount = -i * 100,
                    Rate = 1,
                    Description = "description",
                    State = true,
                    Status = true,
                });
                await databaseContext.SaveChangesAsync();
            }
        }
        return databaseContext;
    }

    [Fact]
    public async void ActivityTransactionRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new ActivityTransactionRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id,
            WalletId = 1.ToString(),
            Amount = 100,
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ActivityTransaction>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void ActivityTransactionRepository_GetList()
    {
        // Arrange
        List<string> walletIds = new();
        List<string> activityIds = new();
        string search = "";
        var dbContext = await UnibeanDBContext();
        var repository = new ActivityTransactionRepository(dbContext);

        // Act
        var result = repository.GetAll(walletIds, activityIds, search);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<ActivityTransaction>>();
        result.Count.Should().BeInRange(0, 10);
        Assert.Equal(10, result.Count);
    }

    [Fact]
    public async void ActivityTransactionRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new ActivityTransactionRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ActivityTransaction>();
        Assert.Equal(id, result.Id);
        Assert.Equal(id, result.ActivityId);
        Assert.Equal(id, result.WalletId);
    }

    [Fact]
    public async void IncomeOfGreenBean()
    {
        // Arrange
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        var dbContext = await UnibeanDBContext();
        var repository = new ActivityTransactionRepository(dbContext);

        // Act
        var result = repository.IncomeOfGreenBean(date);

        // Assert
        result.Should().Be(5500);
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public async void OutcomeOfGreenBean()
    {
        // Arrange
        string storeId = "1";
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        var dbContext = await UnibeanDBContext();
        var repository = new ActivityTransactionRepository(dbContext);

        // Act
        var result = repository.OutcomeOfGreenBean(storeId, date);

        // Assert
        result.Should().Be(0);
    }
}
