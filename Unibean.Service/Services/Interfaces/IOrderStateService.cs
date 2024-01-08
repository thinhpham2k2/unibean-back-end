using Unibean.Service.Models.Campuses;
using Unibean.Service.Models.OrderStates;

namespace Unibean.Service.Services.Interfaces;

public interface IOrderStateService
{
    OrderStateModel Add(string id, CreateOrderStateModel creation);
}
