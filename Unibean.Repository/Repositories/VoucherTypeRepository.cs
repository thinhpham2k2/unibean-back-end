using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class VoucherTypeRepository : IVoucherTypeRepository
{
    private readonly UnibeanDBContext unibeanDB;

    public VoucherTypeRepository(UnibeanDBContext unibeanDB)
    {
        this.unibeanDB = unibeanDB;
    }

    public VoucherType Add(VoucherType creation)
    {
        try
        {
            var db = unibeanDB;
            creation = db.VoucherTypes.Add(creation).Entity;
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
            var type = db.VoucherTypes.FirstOrDefault(b => b.Id.Equals(id));
            type.Status = false;
            db.VoucherTypes.Update(type);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<VoucherType> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<VoucherType> pagedResult = new();
        try
        {
            var db = unibeanDB;
            var query = db.VoucherTypes
                .Where(t => (EF.Functions.Like(t.TypeName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (state == null || state.Equals(t.State))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .ToList();

            pagedResult = new PagedResultModel<VoucherType>
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

    public VoucherType GetById(string id)
    {
        VoucherType type = new();
        try
        {
            var db = unibeanDB;
            type = db.VoucherTypes
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Vouchers.Where(v => (bool)v.Status))
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return type;
    }

    public VoucherType Update(VoucherType update)
    {
        try
        {
            var db = unibeanDB;
            update = db.VoucherTypes.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
