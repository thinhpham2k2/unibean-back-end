using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class StaffRepository : IStaffRepository
{
    private readonly UnibeanDBContext unibeanDB;

    public StaffRepository(UnibeanDBContext unibeanDB)
    {
        this.unibeanDB = unibeanDB;
    }

    public Staff Add(Staff creation)
    {
        try
        {
            var db = unibeanDB;
            creation = db.Staffs.Add(creation).Entity;
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
            var staff = db.Staffs.FirstOrDefault(b => b.Id.Equals(id));
            staff.Status = false;
            db.Staffs.Update(staff);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Staff> GetAll
        (List<string> stationIds, bool? state, string propertySort,
        bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Staff> pagedResult = new();
        try
        {
            var db = unibeanDB;
            var query = db.Staffs
                .Where(p => (EF.Functions.Like(p.Id, "%" + search + "%")
                || EF.Functions.Like(p.FullName, "%" + search + "%")
                || EF.Functions.Like(p.Station.StationName, "%" + search + "%")
                || EF.Functions.Like(p.Account.Email, "%" + search + "%")
                || EF.Functions.Like(p.Account.Description, "%" + search + "%"))
                && (state == null || state.Equals(p.State))
                && (stationIds.Count == 0 || stationIds.Contains(p.StationId))
                && (bool)p.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(b => b.Account)
               .Include(s => s.Station)
               .ToList();

            pagedResult = new PagedResultModel<Staff>
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

    public Staff GetById(string id)
    {
        Staff staff = new();
        try
        {
            var db = unibeanDB;
            staff = db.Staffs
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(b => b.Account)
            .Include(s => s.Station)
                .ThenInclude(s => s.Staffs.Where(s => (bool)s.Status))
            .Include(s => s.Station)
                .ThenInclude(s => s.Orders.Where(s => (bool)s.Status))
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return staff;
    }

    public Staff Update(Staff update)
    {
        try
        {
            var db = unibeanDB;
            update = db.Staffs.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
