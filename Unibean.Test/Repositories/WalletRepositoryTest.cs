using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class WalletRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Wallets.AnyAsync())
        {
            Array values = Enum.GetValues(typeof(WalletType));
            for (int i = 1; i <= 10; i++)
            {
                Random random = new();
                WalletType randomType =
                    (WalletType)values.GetValue(random.Next(values.Length));
                databaseContext.Wallets.Add(
                new Wallet()
                {
                    Id = i.ToString(),
                    CampaignId = i.ToString(),
                    StudentId = i.ToString(),
                    BrandId = i.ToString(),
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
    public async void WalletRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new WalletRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Wallet>();
        Assert.Equal(id, result.Id);
    }
}
