using Unibean.Service.Models.Levels;

namespace Unibean.Service.Services.Interfaces;

public interface ILevelService
{
    LevelModel GetLevelByName(string levelName);
}
