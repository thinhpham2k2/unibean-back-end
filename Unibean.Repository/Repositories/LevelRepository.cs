using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class LevelRepository : ILevelRepository
{
    public Level GetLevelByName(string levelName)
    {
        Level level = new();
        try
        {
            using var db = new UnibeanDBContext();
            level = db.Levels
            .Where(l => l.LevelName.Equals(levelName)
            && l.Status.Equals(true))
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return level;
    }
}
