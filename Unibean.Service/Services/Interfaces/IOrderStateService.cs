using Unibean.Service.Models.Campuses;
using Unibean.Service.Models.OrderStates;

namespace Unibean.Service.Services.Interfaces;

public interface IOrderStateService
{
    string Add(string id, CreateOrderStateModel creation);
}
