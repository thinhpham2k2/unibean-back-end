﻿using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IStationRepository
{
    Station Add(Station creation);

    void Delete(string id);

    PagedResultModel<Station> GetAll
        (List<StationState> stateIds, string propertySort, bool isAsc, string search, int page, int limit);

    Station GetById(string id);

    Station Update(Station update);
}
