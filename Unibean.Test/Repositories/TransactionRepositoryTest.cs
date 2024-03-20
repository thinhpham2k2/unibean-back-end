using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Test.Repositories;

public class TransactionRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        // Activity transactions
        for (int i = 1; i <= 10; i++)
        {
            databaseContext.ActivityTransactions.Add(
            new ActivityTransaction()
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
                ActivityId = i.ToString(),
                Activity = new()
                {
                    Id = i.ToString(),
                    VoucherItemId = i.ToString(),
                    VoucherItem = new()
                    {
                        Id = i.ToString(),
                        Voucher = new()
                        {
                            Id = i.ToString(),
                            VoucherName = "voucherName" + i.ToString(),
                        }
                    },
                    StoreId = i.ToString(),
                    Store = new()
                    {
                        Id = i.ToString(),
                        StoreName = "storeName" + i.ToString(),
                    },
                    StudentId = i.ToString(),
                    Student = new()
                    {
                        Id = i.ToString(),
                        FullName = "fullName" + i.ToString(),
                    },
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

        // Bonus transactions
        for (int i = 11; i <= 20; i++)
        {
            databaseContext.BonusTransactions.Add(
            new BonusTransaction()
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
                BonusId = i.ToString(),
                Bonus = new()
                {
                    Id = i.ToString(),
                    BrandId = i.ToString(),
                    Brand = new()
                    {
                        Id = i.ToString(),
                        BrandName = "brandName" + i.ToString(),
                    },
                    StoreId = i.ToString(),
                    Store = new()
                    {
                        Id = i.ToString(),
                        StoreName = "storeName" + i.ToString(),
                    },
                    StudentId = i.ToString(),
                    Student = new()
                    {
                        Id = i.ToString(),
                        FullName = "fullName" + i.ToString(),
                    },
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

        // Campaign transactions
        for (int i = 21; i <= 30; i++)
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
                    Brand = new()
                    {
                        Id = i.ToString(),
                        BrandName = "brandName" + i.ToString(),
                    },
                    CampaignId = i.ToString(),
                    Type = WalletType.Green,
                    Status = true,
                },
                CampaignId = i.ToString(),
                Campaign = new()
                {
                    Id = i.ToString(),
                    BrandId = i.ToString(),
                    CampaignName = "campaignName" + i.ToString(),
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

        // Challenge transactions
        for (int i = 31; i <= 40; i++)
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
                    Brand = new()
                    {
                        Id = i.ToString(),
                        BrandName = "brandName" + i.ToString(),
                    },
                    CampaignId = i.ToString(),
                    Type = WalletType.Green,
                    Status = true,
                },
                ChallengeId = i.ToString(),
                Challenge = new()
                {
                    Id = i.ToString(),
                    ChallengeId = i.ToString(),
                    Challenge = new()
                    {
                        Id = i.ToString(),
                        ChallengeName = "challengeName" + i.ToString(),
                    },
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

        // Order transactions
        for (int i = 41; i <= 50; i++)
        {
            databaseContext.OrderTransactions.Add(
            new OrderTransaction()
            {
                Id = i.ToString(),
                WalletId = i.ToString(),
                Wallet = new()
                {
                    Id = i.ToString(),
                    BrandId = i.ToString(),
                    Brand = new()
                    {
                        Id = i.ToString(),
                        BrandName = "brandName" + i.ToString(),
                    },
                    CampaignId = i.ToString(),
                    Type = WalletType.Green,
                    Status = true,
                },
                OrderId = i.ToString(),
                Order = new()
                {
                    Id = i.ToString(),
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

        // Request transactions
        for (int i = 51; i <= 60; i++)
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
                    Brand = new()
                    {
                        Id = i.ToString(),
                        BrandName = "brandName" + i.ToString(),
                    },
                    CampaignId = i.ToString(),
                    Type = WalletType.Green,
                    Status = true,
                },
                RequestId = i.ToString(),
                Request = new()
                {
                    Id = i.ToString(),
                    AdminId = i.ToString(),
                    Admin = new()
                    {
                        Id = i.ToString(),
                        FullName = "fullName" + i.ToString(),
                    },
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
        return databaseContext;
    }

    [Fact]
    public async void TransactionRepository_GetAll_ForBrand()
    {
        // Arrange
        List<string> walletIds = new();
        List<TransactionType> typeIds = new();
        string search = "";
        Role role = Role.Brand;
        var dbContext = await UnibeanDBContext();
        var repository = new TransactionRepository(dbContext);

        // Act
        var result = repository.GetAll(walletIds, typeIds, search, role);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<object>>();
        result.Count.Should().BeInRange(0, 30);
        result.Count.Should().Be(30);
    }

    [Fact]
    public async void TransactionRepository_GetAll_ForStudent()
    {
        // Arrange
        List<string> walletIds = new();
        List<TransactionType> typeIds = new();
        string search = "";
        Role role = Role.Student;
        var dbContext = await UnibeanDBContext();
        var repository = new TransactionRepository(dbContext);

        // Act
        var result = repository.GetAll(walletIds, typeIds, search, role);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<object>>();
        result.Count.Should().BeInRange(0, 40);
        result.Count.Should().Be(40);
    }
}
