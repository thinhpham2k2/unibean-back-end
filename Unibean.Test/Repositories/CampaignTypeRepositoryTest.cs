using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class CampaignTypeRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.CampaignTypes.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.CampaignTypes.Add(
                new CampaignType()
                {
                    Id = i.ToString(),
                    TypeName = "typeName" + i,
                    Image = "image" + i,
                    FileName = "fileName" + i,
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
    public async void CampaignTypeRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignTypeRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CampaignType>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void CampaignTypeRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignTypeRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.CampaignTypes.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void CampaignTypeRepository_GetAll()
    {
        // Arrange
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignTypeRepository(dbContext);

        // Act
        var result = repository.GetAll(state, propertySort, isAsc,
            search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<CampaignType>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void CampaignTypeRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignTypeRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CampaignType>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void CampaignTypeRepository_Update()
    {
        // Arrange
        string id = "1";
        string typeName = "typeName";
        var dbContext = await UnibeanDBContext();
        var repository = new CampaignTypeRepository(dbContext);

        // Act
        var existingAccount = await dbContext.CampaignTypes.FindAsync(id);
        existingAccount.TypeName = typeName;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CampaignType>();
        Assert.Equal(id, result.Id);
        Assert.Equal(typeName, result.TypeName);
    }
}
