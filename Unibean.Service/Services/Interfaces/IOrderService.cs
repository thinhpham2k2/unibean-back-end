﻿using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Orders;

namespace Unibean.Service.Services.Interfaces;

public interface IOrderService
{
    OrderModel Add(string id, CreateOrderModel creation);

    PagedResultModel<OrderModel> GetAll
        (List<string> stationIds, List<string> studentIds, List<State> stateIds,
        bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    OrderExtraModel GetById(string id);
}
