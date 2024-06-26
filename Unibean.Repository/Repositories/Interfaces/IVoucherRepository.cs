﻿using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IVoucherRepository
{
    Voucher Add(Voucher creation);

    void Delete(string id);

    PagedResultModel<Voucher> GetAll
        (List<string> brandIds, List<string> typeIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    Voucher GetById(string id);

    Voucher GetByIdAndCampaign(string id, string campaignId);

    Voucher Update(Voucher update);
}
