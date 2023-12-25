using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class DistrictRepository : IDistrictRepository
{
    public District Add(District creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.Districts.Add(creation).Entity;
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
            var district = db.Districts.FirstOrDefault(b => b.Id.Equals(id));
            district.Status = false;
            db.Districts.Update(district);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<District> GetAll(string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<District> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Districts
                .Where(t => (EF.Functions.Like(t.DistrictName, "%" + search + "%")
                || EF.Functions.Like(t.FileName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && t.Status.Equals(true))
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(d => d.City)
               .ToList();

            pagedResult = new PagedResultModel<District>
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

    public District GetById(string id)
    {
        District district = new();
        try
        {
            using var db = new UnibeanDBContext();
            district = db.Districts
            .Where(s => s.Id.Equals(id) && s.Status.Equals(true))
            .Include(d => d.City)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return district;
    }

    public District Update(District update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.Districts.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
