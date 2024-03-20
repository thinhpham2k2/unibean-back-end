using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class ChallengeTransactionRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.ChallengeTransactions.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.ChallengeTransactions.Add(
                new ChallengeTransaction()
                {
                    Id = i.ToString(),
                    WalletId = i.ToString(),
                    Wallet = new()
                    {
                        Id = i.ToString(),
                        BrandId = i.ToString(),
                        CampaignId = i.ToString(),
                        Type = WalletType.Green,
                        Status = true,
                    },
                    ChallengeId = i.ToString(),
                    Challenge = new()
                    {
                        Id = i.ToString(),
                        DateCreated = DateTime.Now,
                        Status = true,
                    },
                    Amount = 100,
                    Rate = 1,
                    DateCreated = DateTime.Now,
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
    public async void ChallengeTransactionRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new ChallengeTransactionRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id,
            WalletId = "1",
            Amount = 100,
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ChallengeTransaction>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void ChallengeTransactionRepository_GetAll()
    {
        // Arrange
        List<string> walletIds = new();
        List<string> challengeIds = new();
        string search = "";
        var dbContext = await UnibeanDBContext();
        var repository = new ChallengeTransactionRepository(dbContext);

        // Act
        var result = repository.GetAll(walletIds, challengeIds, search);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<ChallengeTransaction>>();
        result.Count.Should().BeInRange(0, 10);
    }

    [Fact]
    public async void ChallengeTransactionRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new ChallengeTransactionRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ChallengeTransaction>();
        Assert.Equal(id, result.Id);
    }
}
