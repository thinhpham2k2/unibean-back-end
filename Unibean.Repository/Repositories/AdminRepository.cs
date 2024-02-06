using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class AdminRepository : IAdminRepository
{
    public Admin Add(Admin creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.Admins.Add(creation).Entity;
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
            var admin = db.Admins.FirstOrDefault(b => b.Id.Equals(id));
            admin.Status = false;
            db.Admins.Update(admin);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Admin> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Admin> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Admins
                .Where(p => (EF.Functions.Like(p.Id, "%" + search + "%")
                || EF.Functions.Like(p.FullName, "%" + search + "%")
                || EF.Functions.Like(p.Account.Email, "%" + search + "%")
                || EF.Functions.Like(p.Account.Description, "%" + search + "%"))
                && (state == null || state.Equals(p.State))
                && (bool)p.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(b => b.Account)
               .Include(s => s.Requests.Where(r => (bool)r.Status))
                   .ThenInclude(w => w.RequestTransactions)
               .ToList();

            pagedResult = new PagedResultModel<Admin>
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

    public Admin GetById(string id)
    {
        Admin admin = new();
        try
        {
            using var db = new UnibeanDBContext();
            admin = db.Admins
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(b => b.Account)
            .Include(s => s.Requests.Where(r => (bool)r.Status))
                .ThenInclude(w => w.RequestTransactions)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return admin;
    }

    public Admin Update(Admin update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.Admins.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
