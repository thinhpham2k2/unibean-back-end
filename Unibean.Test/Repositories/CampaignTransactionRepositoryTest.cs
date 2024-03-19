using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class CampaignTransactionRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.CampaignTransactions.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.CampaignTransactions.Add(
                new CampaignTransaction()
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
                    CampaignId = i.ToString(),
                    Campaign = new()
                    {
                        Id = i.ToString(),
                        BrandId = i.ToString(),
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
    public async void CampaignTransactionRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignTransactionRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id,
            WalletId = "1",
            Amount = 100,
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CampaignTransaction>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void CampaignTransactionRepository_GetAll()
    {
        // Arrange
        List<string> walletIds = new();
        List<string> campaignIds = new();
        List<WalletType> walletTypeIds = new();
        string search = "";
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignTransactionRepository(dbContext);

        // Act
        var result = repository.GetAll(walletIds, campaignIds,
            walletTypeIds, search);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<CampaignTransaction>>();
        result.Count.Should().BeInRange(0, 10);
    }

    [Fact]
    public async void CampaignTransactionRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignTransactionRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CampaignTransaction>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void CampaignTransactionRepository_IncomeOfGreenBean()
    {
        // Arrange
        string brandId = "1";
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignTransactionRepository(dbContext);

        // Act
        var result = repository.IncomeOfGreenBean(brandId, date);

        // Assert
        result.Should().Be(100);
    }

    [Fact]
    public async void CampaignTransactionRepository_OutcomeOfGreenBean()
    {
        // Arrange
        string brandId = "1";
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignTransactionRepository(dbContext);

        // Act
        var result = repository.OutcomeOfGreenBean(brandId, date);

        // Assert
        result.Should().Be(0);
    }
}
