using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class VoucherRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Vouchers.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.Vouchers.Add(
                new Voucher()
                {
                    Id = i.ToString(),
                    BrandId = i.ToString(),
                    TypeId = i.ToString(),
                    VoucherName = "areaName" + i,
                    Price = 10,
                    Rate = 1,
                    Condition = "condition" + i,
                    Image = "image" + i,
                    ImageName = "imageName" + i,
                    File = "file" + i,
                    FileName = "fileName" + i,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Description = "description" + i,
                    State = true,
                    Status = true,
                    VoucherItems = new List<VoucherItem>()
                    {
                        new()
                        {
                            Id = i.ToString(),
                            CampaignDetail = new()
                            {
                                Id = i.ToString(),
                                CampaignId = i.ToString(),
                            }
                        }
                    }
                });
                await databaseContext.SaveChangesAsync();
            }
        }
        return databaseContext;
    }

    [Fact]
    public async void VoucherRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new VoucherRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Voucher>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void VoucherRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new VoucherRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.Vouchers.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void VoucherRepository_GetAll()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> typeIds = new();
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new VoucherRepository(dbContext);

        // Act
        var result = repository.GetAll(brandIds, typeIds, state,
            propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<Voucher>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void VoucherRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new VoucherRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Voucher>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void VoucherRepository_GetByIdAndCampaign()
    {
        // Arrange
        string id = "1";
        string campaignId = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new VoucherRepository(dbContext);

        // Act
        var result = repository.GetByIdAndCampaign(id, campaignId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Voucher>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void VoucherRepository_Update()
    {
        // Arrange
        string id = "1";
        string areaName = "areaName";
        var dbContext = await UnibeanDBContext();
        var repository = new VoucherRepository(dbContext);

        // Act
        var existingAccount = await dbContext.Vouchers.FindAsync(id);
        existingAccount.VoucherName = areaName;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Voucher>();
        Assert.Equal(id, result.Id);
        Assert.Equal(areaName, result.VoucherName);
    }
}
