using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class CityRepository : ICityRepository
{
    public City Add(City creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.Cities.Add(creation).Entity;
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
            var city = db.Cities.FirstOrDefault(b => b.Id.Equals(id));
            city.Status = false;
            db.Cities.Update(city);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<City> GetAll(string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<City> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Cities
                .Where(t => (EF.Functions.Like(t.CityName, "%" + search + "%")
                || EF.Functions.Like(t.FileName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && t.Status.Equals(true))
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .ToList();

            pagedResult = new PagedResultModel<City>
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

    public City GetById(string id)
    {
        City city = new();
        try
        {
            using var db = new UnibeanDBContext();
            city = db.Cities
            .Where(s => s.Id.Equals(id) && s.Status.Equals(true))
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return city;
    }

    public City Update(City update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.Cities.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
