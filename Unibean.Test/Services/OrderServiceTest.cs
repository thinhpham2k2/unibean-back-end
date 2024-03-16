using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Admins;
using Unibean.Service.Models.Orders;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class OrderServiceTest
{
    private readonly IEmailService emailService;

    private readonly IOrderRepository orderRepository;

    private readonly IStudentRepository studentRepository;

    private readonly IOrderTransactionRepository orderTransactionRepository;

    public OrderServiceTest()
    {
        emailService = A.Fake<IEmailService>();
        orderRepository = A.Fake<IOrderRepository>();
        studentRepository = A.Fake<IStudentRepository>();
        orderTransactionRepository = A.Fake<IOrderTransactionRepository>();
    }

    [Fact]
    public void OrderService_Add()
    {
        // Arrange
        string id = "id";
        CreateOrderModel creation = new()
        {
            Amount = 1
        };
        A.CallTo(() => studentRepository.GetById(id)).Returns(new()
        {
            Wallets = new List<Wallet>()
            {
                new()
                {
                    Balance = 1,
                    Type = WalletType.Red
                }
            },
            Account = new()
            {
                Email = "receiver"
            }
        });
        A.CallTo(() => orderRepository.Add(A<Order>.Ignored)).Returns(new()
        {
            Id = id
        });
        var service = new OrderService
            (emailService, orderRepository, studentRepository, orderTransactionRepository);

        // Act
        var result = service.Add(id, creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(OrderModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void OrderService_GetAll()
    {
        // Arrange
        List<string> stationIds = new();
        List<string> studentIds = new();
        List<State> stateIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<Order> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => orderRepository.GetAll(stationIds, studentIds, stateIds, state,
            propertySort, isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new OrderService
            (emailService, orderRepository, studentRepository, orderTransactionRepository);

        // Act
        var result = service.GetAll(stationIds, studentIds, stateIds, state,
            propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<OrderModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void OrderService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => orderRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        var service = new OrderService
            (emailService, orderRepository, studentRepository, orderTransactionRepository);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(OrderExtraModel));
        Assert.Equal(id, result.Id);
    }
}
