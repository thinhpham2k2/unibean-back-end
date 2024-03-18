using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class BonusTransactionRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.BonusTransactions.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.BonusTransactions.Add(
                new BonusTransaction()
                {
                    Id = i.ToString(),
                    WalletId = i.ToString(),
                    BonusId = i.ToString(),
                    Bonus = new()
                    {
                        Id = i.ToString(),
                        StoreId = i.ToString(),
                        Amount = 100,
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
    public async void BonusTransactionRepository_GetList()
    {
        // Arrange
        List<string> walletIds = new();
        List<string> bonusIds = new();
        List<WalletType> walletTypeIds = new();
        string search = "";
        var dbContext = await UnibeanDBContext();
        var repository = new BonusTransactionRepository(dbContext);

        // Act
        var result = repository.GetAll(walletIds, bonusIds,
            walletTypeIds, search);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<BonusTransaction>>();
        result.Count.Should().BeInRange(0, 10);
    }

    [Fact]
    public async void BonusTransactionRepository_OutcomeOfGreenBean()
    {
        // Arrange
        string storeId = "1";
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        var dbContext = await UnibeanDBContext();
        var repository = new BonusTransactionRepository(dbContext);

        // Act
        var result = repository.OutcomeOfGreenBean(storeId, date);

        // Assert
        result.Should().Be(100);
    }
}
