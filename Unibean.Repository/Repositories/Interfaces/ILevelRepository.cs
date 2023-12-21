using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface ILevelRepository
{
    Level GetLevelByName(string levelName);
}
