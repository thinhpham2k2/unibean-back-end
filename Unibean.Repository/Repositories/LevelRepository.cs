using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class LevelRepository : ILevelRepository
{
    public Level Add(Level creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.Levels.Add(creation).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return creation;
    }

    public void Delete(string id)
    {
        try
        {
            using var db = new UnibeanDBContext();
            var level = db.Levels.FirstOrDefault(b => b.Id.Equals(id));
            level.Status = false;
            db.Levels.Update(level);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Level> GetAll(string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Level> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Levels
                .Where(t => (EF.Functions.Like(t.LevelName, "%" + search + "%")
                || EF.Functions.Like(t.FileName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .ToList();

            pagedResult = new PagedResultModel<Level>
            {
                CurrentPage = page,
                PageSize = limit,
                PageCount = (int)Math.Ceiling((double)query.Count() / limit),
                Result = result,
                RowCount = result.Count,
                TotalCount = query.Count()
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return pagedResult;
    }

    public Level GetById(string id)
    {
        Level level = new();
        try
        {
            using var db = new UnibeanDBContext();
            level = db.Levels
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return level;
    }

    public Level Update(Level update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.Levels.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
