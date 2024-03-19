using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class OrderStateRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.OrderStates.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.OrderStates.Add(
                new OrderState()
                {
                    Id = i.ToString(),
                    OrderId = "areaName" + i,
                    Order = new()
                    {
                        Id = i.ToString(),
                        Status = true,
                        OrderTransactions = new List<OrderTransaction>()
                        {
                            new()
                            {
                                Id = i.ToString(),
                                WalletId = i.ToString(),
                                Wallet = new()
                                {
                                    Id = i.ToString(),
                                },
                                Status = true,
                            }
                        }
                    },
                    DateCreated = DateTime.Now,
                    Description = "description" + i,
                    State = State.Order,
                    Status = true,
                });
                await databaseContext.SaveChangesAsync();
            }
        }
        return databaseContext;
    }

    [Fact]
    public async void OrderStateRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new OrderStateRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OrderState>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void OrderStateRepository_AddAbort()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new OrderStateRepository(dbContext);

        // Act
        var result = repository.AddAbort(new()
        {
            Id = id,
            OrderId = "1",
            Description = "description" + id,
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OrderState>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void OrderStateRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new OrderStateRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.OrderStates.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void OrderStateRepository_GetAll()
    {
        // Arrange
        List<string> orderIds = new();
        List<State> stateIds = new();
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new OrderStateRepository(dbContext);

        // Act
        var result = repository.GetAll(orderIds, stateIds, propertySort, 
            isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<OrderState>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void OrderStateRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new OrderStateRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OrderState>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void OrderStateRepository_Update()
    {
        // Arrange
        string id = "1";
        string description = "description";
        var dbContext = await UnibeanDBContext();
        var repository = new OrderStateRepository(dbContext);

        // Act
        var existingAccount = await dbContext.OrderStates.FindAsync(id);
        existingAccount.Description = description;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OrderState>();
        Assert.Equal(id, result.Id);
        Assert.Equal(description, result.Description);
    }
}
