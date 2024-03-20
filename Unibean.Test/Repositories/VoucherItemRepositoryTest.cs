using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;
using Unibean.Repository.Repositories.Interfaces;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.Test.Repositories;

public class VoucherItemRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.VoucherItems.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.VoucherItems.Add(
                new VoucherItem()
                {
                    Id = i.ToString(),
                    VoucherId = "1",
                    CampaignDetailId = i > 3 ? i.ToString() : null,
                    VoucherCode = "voucherCode" + i,
                    Index = i,
                    IsLocked = i > 3,
                    IsBought = i > 6,
                    IsUsed = i > 8,
                    ValidOn = DateOnly.FromDateTime(DateTime.Now),
                    ExpireOn = DateOnly.FromDateTime(DateTime.Now),
                    DateCreated = DateTime.Now,
                    DateIssued = DateTime.Now,
                    State = true,
                    Status = true,
                    Activities = new List<Activity>()
                    {
                        new()
                        {
                            Id = i.ToString(),
                            Type = Type.Use,
                            DateCreated = DateTime.Now,
                        }
                    }
                });
                await databaseContext.SaveChangesAsync();
            }
        }
        return databaseContext;
    }

    [Fact]
    public async void VoucherItemRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new VoucherItemRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<VoucherItem>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void VoucherItemRepository_AddList()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new VoucherItemRepository(dbContext);

        // Act & Assert
        repository.AddList(new List<VoucherItem>()
        {
            new()
            {
                Id= id
            }
        });
    }

    [Fact]
    public async void VoucherItemRepository_CheckVoucherCode_ReturnFalse()
    {
        // Arrange
        string code = "voucherCode";
        var dbContext = await UnibeanDBContext();
        var repository = new VoucherItemRepository(dbContext);

        // Act & Assert
        Assert.False(repository.CheckVoucherCode(code));
    }

    [Fact]
    public async void VoucherItemRepository_CheckVoucherCode_ReturnTrue()
    {
        // Arrange
        string code = "voucherCode1";
        var dbContext = await UnibeanDBContext();
        var repository = new VoucherItemRepository(dbContext);

        // Act & Assert
        Assert.True(repository.CheckVoucherCode(code));
    }

    [Fact]
    public async void VoucherItemRepository_CountVoucherItemToday()
    {
        // Arrange
        string brandId = "1";
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        var dbContext = await UnibeanDBContext();
        var repository = new VoucherItemRepository(dbContext);

        // Act
        var result = repository.CountVoucherItemToday(brandId, date);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public async void VoucherItemRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new VoucherItemRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.VoucherItems.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void VoucherItemRepository_GetAll()
    {
        // Arrange
        List<string> campaignIds = new();
        List<string> campaignDetailIds = new();
        List<string> voucherIds = new();
        List<string> brandIds = new();
        List<string> typeIds = new();
        List<string> studentIds = new();
        bool? isLocked = null;
        bool? isBought = null;
        bool? isUsed = null;
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new VoucherItemRepository(dbContext);

        // Act
        var result = repository.GetAll(campaignIds, campaignDetailIds, voucherIds,
            brandIds, typeIds, studentIds, isLocked, isBought, isUsed, state,
            propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<VoucherItem>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void VoucherItemRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new VoucherItemRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<VoucherItem>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void VoucherItemRepository_GetByVoucherCode()
    {
        // Arrange
        string id = "1";
        string voucherCode = "voucherCode" + id;
        var dbContext = await UnibeanDBContext();
        var repository = new VoucherItemRepository(dbContext);

        // Act
        var result = repository.GetByVoucherCode(voucherCode);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<VoucherItem>();
        Assert.Equal(id, result.Id);
        Assert.Equal(voucherCode, result.VoucherCode);
    }

    [Fact]
    public async void VoucherItemRepository_GetIndex()
    {
        // Arrange
        string voucherId = "1";
        int quantity = 3;
        int fromIndex = 0;
        var dbContext = await UnibeanDBContext();
        var repository = new VoucherItemRepository(dbContext);

        // Act
        var result = repository.GetIndex(voucherId, quantity, fromIndex);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ItemIndex>();
        Assert.Equal(1, result.FromIndex);
        Assert.Equal(3, result.ToIndex);
    }

    [Fact]
    public async void VoucherItemRepository_GetMaxIndex()
    {
        // Arrange
        string voucherId = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new VoucherItemRepository(dbContext);

        // Act & Assert
        Assert.Equal(10, repository.GetMaxIndex(voucherId));
    }

    [Fact]
    public async void VoucherItemRepository_Update()
    {
        // Arrange
        string id = "1";
        string voucherCode = "voucherCode";
        var dbContext = await UnibeanDBContext();
        var repository = new VoucherItemRepository(dbContext);

        // Act
        var existingAccount = await dbContext.VoucherItems.FindAsync(id);
        existingAccount.VoucherCode = voucherCode;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<VoucherItem>();
        Assert.Equal(id, result.Id);
        Assert.Equal(voucherCode, result.VoucherCode);
    }

    [Fact]
    public async void VoucherItemRepository_UpdateList()
    {
        // Arrange
        string voucherId = "1";
        string campaignDetailId = "1";
        int quantity = 3;
        DateOnly StartOn = DateOnly.FromDateTime(DateTime.Now);
        DateOnly EndOn = DateOnly.FromDateTime(DateTime.Now);
        ItemIndex index = new();
        var dbContext = await UnibeanDBContext();
        var repository = new VoucherItemRepository(dbContext);

        // Act & Assert
        repository.UpdateList(voucherId, campaignDetailId, quantity,
            StartOn, EndOn, index);
    }
}
