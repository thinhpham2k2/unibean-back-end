using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.OrderStates;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class OrderStateServiceTest
{
    private readonly IEmailService emailService;

    private readonly IOrderRepository orderRepository;

    private readonly IOrderStateRepository orderStateRepository;

    public OrderStateServiceTest()
    {
        emailService = A.Fake<IEmailService>();
        orderRepository = A.Fake<IOrderRepository>();
        orderStateRepository = A.Fake<IOrderStateRepository>();
    }

    [Fact]
    public void OrderStateService_Add()
    {
        // Arrange
        string id = "id";
        CreateOrderStateModel creation = new()
        {
            State = 6
        };
        A.CallTo(() => orderRepository.GetById(id)).Returns(new()
        {
            OrderStates = new List<OrderState>()
            {
                new()
                {
                    State = State.Order
                }
            },
            Student = new()
            {
                Account = new()
                {
                    Email = "receiver"
                }
            }
        });
        A.CallTo(() => orderStateRepository.AddAbort(A<OrderState>.Ignored)).Returns(new()
        {
            Id = id
        });
        var service = new OrderStateService(emailService, orderRepository, orderStateRepository);

        // Act
        var result = service.Add(id, creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(string));
        Assert.Equal("Hủy đơn hàng thành công", result);
    }
}
