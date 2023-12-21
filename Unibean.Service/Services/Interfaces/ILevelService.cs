using Unibean.Repository.Paging;
using Unibean.Service.Models.Levels;

namespace Unibean.Service.Services.Interfaces;

public interface ILevelService
{
    Task<LevelModel> Add(CreateLevelModel creation);

    void Delete(string id);

    PagedResultModel<LevelModel> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    LevelModel GetById(string id);

    LevelModel GetLevelByName(string levelName);

    Task<LevelModel> Update(string id, UpdateLevelModel update);
}
