using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class MajorRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Majors.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.Majors.Add(
                new Major()
                {
                    Id = i.ToString(),
                    MajorName = "majorName" + i,
                    Image = "image" + i,
                    FileName = "fileName" + i,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Description = "description" + i,
                    State = true,
                    Status = true,
                    CampaignMajors = new List<CampaignMajor>()
                    {
                        new()
                        {
                            Id = i.ToString(),
                            CampaignId = i.ToString(),
                            Campaign = new()
                            {
                                Id = i.ToString(),
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
    public async void MajorRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new MajorRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Major>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void MajorRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new MajorRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.Majors.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void MajorRepository_GetAll()
    {
        // Arrange
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new MajorRepository(dbContext);

        // Act
        var result = repository.GetAll(state, propertySort,
            isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<Major>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void MajorRepository_GetAllByCampaign()
    {
        // Arrange
        List<string> campaignIds = new()
        {
            "1"
        };
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new MajorRepository(dbContext);

        // Act
        var result = repository.GetAllByCampaign(campaignIds, state,
            propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<Major>>();
        Assert.Equal(0, result.RowCount);
    }

    [Fact]
    public async void MajorRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new MajorRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Major>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void MajorRepository_Update()
    {
        // Arrange
        string id = "1";
        string campusName = "campusName";
        var dbContext = await UnibeanDBContext();
        var repository = new MajorRepository(dbContext);

        // Act
        var existingAccount = await dbContext.Majors.FindAsync(id);
        existingAccount.MajorName = campusName;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Major>();
        Assert.Equal(id, result.Id);
        Assert.Equal(campusName, result.MajorName);
    }
}
