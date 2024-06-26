﻿using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IOrderStateRepository
{
    OrderState Add(OrderState creation);

    OrderState AddAbort(OrderState creation);

    void Delete(string id);

    PagedResultModel<OrderState> GetAll
        (List<string> orderIds, List<State> stateIds, string propertySort,
        bool isAsc, string search, int page, int limit);

    OrderState GetById(string id);

    OrderState Update(OrderState update);
}
