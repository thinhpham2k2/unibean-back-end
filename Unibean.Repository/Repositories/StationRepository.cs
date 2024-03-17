using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class StationRepository : IStationRepository
{
    private readonly UnibeanDBContext unibeanDB;

    public StationRepository(UnibeanDBContext unibeanDB)
    {
        this.unibeanDB = unibeanDB;
    }

    public Station Add(Station creation)
    {
        try
        {
            var db = unibeanDB;
            creation = db.Stations.Add(creation).Entity;
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
            var db = unibeanDB;
            var station = db.Stations.FirstOrDefault(b => b.Id.Equals(id));
            station.Status = false;
            db.Stations.Update(station);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Station> GetAll
        (List<StationState> stateIds, string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Station> pagedResult = new();
        try
        {
            var db = unibeanDB;
            var query = db.Stations
                .Where(t => (EF.Functions.Like(t.StationName, "%" + search + "%")
                || EF.Functions.Like(t.Address, "%" + search + "%")
                || EF.Functions.Like(t.Phone, "%" + search + "%")
                || EF.Functions.Like(t.Email, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (stateIds.Count == 0 || stateIds.Contains(t.State.Value))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .ToList();

            pagedResult = new PagedResultModel<Station>
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

    public Station GetById(string id)
    {
        Station station = new();
        try
        {
            var db = unibeanDB;
            station = db.Stations
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Staffs.Where(s => (bool)s.Status))
            .Include(s => s.Orders.Where(o => (bool)o.Status))
                .ThenInclude(o => o.OrderStates.Where(o => (bool)o.Status))
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return station;
    }

    public Station Update(Station update)
    {
        try
        {
            var db = unibeanDB;
            update = db.Stations.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
