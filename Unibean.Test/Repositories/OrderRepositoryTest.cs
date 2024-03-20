using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class OrderRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Orders.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.Orders.Add(
                new Order()
                {
                    Id = i.ToString(),
                    StudentId = i.ToString(),
                    StationId = i.ToString(),
                    Amount = 100,
                    DateCreated = DateTime.Now,
                    Description = "description" + i,
                    State = true,
                    Status = true,
                    OrderDetails = new List<OrderDetail>()
                    {
                        new()
                        {
                            Id = i.ToString(),
                            OrderId = i.ToString(),
                            ProductId = i.ToString(),
                            Product = new()
                            {
                                Id = i.ToString(),
                                Price = 100,
                                Quantity = 100,
                                Status = true,
                            },
                            Quantity = 2
                        }
                    },
                    OrderStates = new List<OrderState>()
                    {
                        new()
                        {
                            Id = i.ToString(),
                            State = State.Order,
                            OrderId = i.ToString(),
                        }
                    }
                });
                await databaseContext.SaveChangesAsync();
            }
        }
        return databaseContext;
    }

    [Fact]
    public async void OrderRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new OrderRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id,
            OrderDetails = new List<OrderDetail>()
            {
                new()
                {
                    Id = id,
                    OrderId = id,
                    ProductId = "1",
                    Quantity = 2
                }
            },
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Order>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void OrderRepository_CountOrderToday()
    {
        // Arrange
        string stationId = "1";
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        var dbContext = await UnibeanDBContext();
        var repository = new OrderRepository(dbContext);

        // Act
        var result = repository.CountOrderToday(stationId, date);

        // Assert
        result.Should().Be(1);
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public async void OrderRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new OrderRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.Orders.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void OrderRepository_GetAll()
    {
        // Arrange
        List<string> stationIds = new();
        List<string> studentIds = new();
        List<State> stateIds = new();
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new OrderRepository(dbContext);

        // Act
        var result = repository.GetAll(stationIds, studentIds, stateIds,
            state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<Order>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void OrderRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new OrderRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Order>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void OrderRepository_Update()
    {
        // Arrange
        string id = "1";
        string description = "description";
        var dbContext = await UnibeanDBContext();
        var repository = new OrderRepository(dbContext);

        // Act
        var existingAccount = await dbContext.Orders.FindAsync(id);
        existingAccount.Description = description;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Order>();
        Assert.Equal(id, result.Id);
        Assert.Equal(description, result.Description);
    }
}
