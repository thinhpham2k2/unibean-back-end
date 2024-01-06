using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class CampusRepository : ICampusRepository
{
    public Campus Add(Campus creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.Campuses.Add(creation).Entity;
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
            var campus = db.Campuses.FirstOrDefault(b => b.Id.Equals(id));
            campus.Status = false;
            db.Campuses.Update(campus);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Campus> GetAll(List<string> universityIds, List<string> areaIds, string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Campus> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Campuses
                .Where(t => (EF.Functions.Like(t.CampusName, "%" + search + "%")
                || EF.Functions.Like(t.University.UniversityName, "%" + search + "%")
                || EF.Functions.Like(t.Area.AreaName, "%" + search + "%")
                || EF.Functions.Like(t.Address, "%" + search + "%")
                || EF.Functions.Like(t.Phone, "%" + search + "%")
                || EF.Functions.Like(t.Email, "%" + search + "%")
                || EF.Functions.Like(t.FileName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (universityIds.Count == 0 || universityIds.Contains(t.UniversityId))
                && (areaIds.Count == 0 || areaIds.Contains(t.AreaId))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(s => s.University)
               .Include(s => s.Area)
               .ToList();

            pagedResult = new PagedResultModel<Campus>
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

    public Campus GetById(string id)
    {
        Campus campus = new();
        try
        {
            using var db = new UnibeanDBContext();
            campus = db.Campuses
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.University)
            .Include(s => s.Area)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return campus;
    }

    public Campus Update(Campus update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.Campuses.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
