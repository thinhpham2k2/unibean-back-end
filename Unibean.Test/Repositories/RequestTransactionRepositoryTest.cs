using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class RequestTransactionRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.RequestTransactions.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.RequestTransactions.Add(
                new RequestTransaction()
                {
                    Id = i.ToString(),
                    WalletId = i.ToString(),
                    Wallet = new()
                    {
                        Id = i.ToString(),
                        BrandId = i.ToString(),
                        Type = WalletType.Green,
                        Status = true,
                    },
                    RequestId = i.ToString(),
                    Request = new()
                    {
                        Id = i.ToString(),
                        BrandId = i.ToString(),
                        DateCreated = DateTime.Now,
                        Status = true,
                    },
                    Amount = 100,
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
    public async void RequestTransactionRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new RequestTransactionRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id,
            WalletId = "1",
            Amount = 100,
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<RequestTransaction>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void RequestTransactionRepository_GetAll()
    {
        // Arrange
        List<string> walletIds = new();
        List<string> requestIds = new();
        List<WalletType> walletTypeIds = new();
        string search = "";
        var dbContext = await UnibeanDBContext();
        var repository = new RequestTransactionRepository(dbContext);

        // Act
        var result = repository.GetAll(walletIds, requestIds,
            walletTypeIds, search);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<RequestTransaction>>();
        result.Count.Should().BeInRange(0, 10);
    }

    [Fact]
    public async void RequestTransactionRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new RequestTransactionRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<RequestTransaction>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void RequestTransactionRepository_IncomeOfGreenBean()
    {
        // Arrange
        string brandId = "1";
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        var dbContext = await UnibeanDBContext();
        var repository = new RequestTransactionRepository(dbContext);

        // Act
        var result = repository.IncomeOfGreenBean(brandId, date);

        // Assert
        result.Should().Be(100);
    }
}
