using Microsoft.IdentityModel.Tokens;
using System.Linq;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.OrderStates;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class OrderStateService : IOrderStateService
{
    private readonly IOrderRepository orderRepository;

    private readonly IOrderStateRepository orderStateRepository;

    public OrderStateService(IOrderRepository orderRepository,
        IOrderStateRepository orderStateRepository)
    {
        this.orderRepository = orderRepository;
        this.orderStateRepository = orderStateRepository;
    }

    public string Add(string id, CreateOrderStateModel creation)
    {
        Order entity = orderRepository.GetById(id);
        if (entity != null)
        {
            List<State> stateIds = entity.OrderStates.Select(s => (State)s.State).ToList();
            if (!stateIds.IsNullOrEmpty())
            {
                if (creation.State > (int)stateIds.Max() || creation.State.Equals(6))
                {
                    if (creation.State.Equals(6))
                    {
                        return "Hủy đơn hàng thành công";
                    }
                    else
                    {
                        stateIds = Enum.GetValues(typeof(State)).Cast<State>().Where(
                            s => s > stateIds.Max() && (int)s <= creation.State).ToList();

                        stateIds.ForEach(s =>
                        {
                            orderStateRepository.Add(new OrderState
                            {
                                Id = Ulid.NewUlid().ToString(),
                                OrderId = id,
                                State = s,
                                DateCreated = DateTime.Now,
                                Description = creation.Description,
                                Status = true,
                            });
                        });

                        return "Đã tạo thành công";
                    }
                }
                else
                {
                    throw new InvalidParameterException
                        ("Trạng thái không hợp lệ vì đơn hàng đã trải qua trạng thái này");
                }
            }
            else
            {
                if (creation.State.Equals(6))
                {
                    return "Hủy đơn hàng thành công";
                }
                else
                {
                    stateIds = Enum.GetValues(typeof(State)).Cast<State>().Where(
                                s => (int)s <= creation.State).ToList();
                    stateIds.ForEach(s =>
                    {
                        orderStateRepository.Add(new OrderState
                        {
                            Id = Ulid.NewUlid().ToString(),
                            OrderId = id,
                            State = s,
                            DateCreated = DateTime.Now,
                            Description = creation.Description,
                            Status = true,
                        });
                    });

                    return "Đã tạo thành công";
                }
            }
        }
        throw new InvalidParameterException("Đơn hàng không hợp lệ");
    }
}
