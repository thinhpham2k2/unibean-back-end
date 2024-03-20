﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class UniversityRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Universities.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.Universities.Add(
                new University()
                {
                    Id = i.ToString(),
                    UniversityName = "universityName" + i,
                    Phone = "phone" + i,
                    Email = "email" + i,
                    Link = "link" + i,
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
    public async void UniversityRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new UniversityRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<University>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void UniversityRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new UniversityRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.Universities.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void UniversityRepository_GetAll()
    {
        // Arrange
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new UniversityRepository(dbContext);

        // Act
        var result = repository.GetAll(state, propertySort, isAsc,
            search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<University>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void UniversityRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new UniversityRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<University>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void UniversityRepository_Update()
    {
        // Arrange
        string id = "1";
        string areaName = "areaName";
        var dbContext = await UnibeanDBContext();
        var repository = new UniversityRepository(dbContext);

        // Act
        var existingAccount = await dbContext.Universities.FindAsync(id);
        existingAccount.UniversityName = areaName;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<University>();
        Assert.Equal(id, result.Id);
        Assert.Equal(areaName, result.UniversityName);
    }
}
