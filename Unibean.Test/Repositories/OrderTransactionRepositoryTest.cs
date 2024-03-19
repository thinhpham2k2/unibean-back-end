using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class OrderTransactionRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.OrderTransactions.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.OrderTransactions.Add(
                new OrderTransaction()
                {
                    Id = i.ToString(),
                    WalletId = i.ToString(),
                    Wallet = new()
                    {
                        Id = i.ToString(),
                        StudentId = i.ToString(),
                        Type = WalletType.Red,
                        Status = true,
                    },
                    OrderId = i.ToString(),
                    Order = new()
                    {
                        Id = i.ToString(),
                        StudentId = i.ToString(),
                        StationId = i.ToString(),
                        DateCreated = DateTime.Now,
                        Status = true,
                    },
                    Amount = -100,
                    Rate = 1,
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
    public async void OrderTransactionRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new OrderTransactionRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id,
            WalletId = "1",
            Amount = 100,
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OrderTransaction>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void OrderTransactionRepository_GetAll()
    {
        // Arrange
        List<string> walletIds = new();
        List<string> orderIds = new();
        string search = "";
        var dbContext = await UnibeanDBContext();
        var repository = new OrderTransactionRepository(dbContext);

        // Act
        var result = repository.GetAll(walletIds, orderIds, search);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<OrderTransaction>>();
        result.Count.Should().BeInRange(0, 10);
    }

    [Fact]
    public async void OrderTransactionRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new OrderTransactionRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OrderTransaction>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void OrderTransactionRepository_IncomeOfRedBean_1()
    {
        // Arrange
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        var dbContext = await UnibeanDBContext();
        var repository = new OrderTransactionRepository(dbContext);

        // Act
        var result = repository.IncomeOfRedBean(date);

        // Assert
        result.Should().Be(1000);
    }

    [Fact]
    public async void OrderTransactionRepository_IncomeOfRedBean_2()
    {
        // Arrange
        string stationId = "1";
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        var dbContext = await UnibeanDBContext();
        var repository = new OrderTransactionRepository(dbContext);

        // Act
        var result = repository.IncomeOfRedBean(stationId, date);

        // Assert
        result.Should().Be(100);
    }

    [Fact]
    public async void OrderTransactionRepository_OutcomeOfRedBean()
    {
        // Arrange
        string stationId = "1";
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        var dbContext = await UnibeanDBContext();
        var repository = new OrderTransactionRepository(dbContext);

        // Act
        var result = repository.OutcomeOfRedBean(stationId, date);

        // Assert
        result.Should().Be(0);
    }
}
