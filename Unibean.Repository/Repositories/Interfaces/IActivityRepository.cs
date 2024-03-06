﻿using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IActivityRepository
{
    Activity Add(Activity creation);

    long CountParticipantToday(string storeId, DateOnly date);

    void Delete(string id);

    PagedResultModel<Activity> GetAll
        (List<string> storeIds, List<string> studentIds, List<string> voucherIds,
        bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    List<Activity> GetList
        (List<string> storeIds, List<string> studentIds, List<string> voucherIds, string search);

    Activity GetById(string id);

    Activity Update(Activity update);
}
