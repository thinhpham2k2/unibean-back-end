﻿using Unibean.Repository.Paging;
using Unibean.Service.Models.Areas;
using Unibean.Service.Models.Types;

namespace Unibean.Service.Services.Interfaces;

public interface IAreaService
{
    Task<AreaModel> Add(CreateAreaModel creation);

    void Delete(string id);

    PagedResultModel<AreaModel> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    AreaModel GetById(string id);

    Task<AreaModel> Update(string id, UpdateAreaModel update);
}
