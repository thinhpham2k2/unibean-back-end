using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class VoucherTypeRepository : IVoucherTypeRepository
{
    public VoucherType Add(VoucherType creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
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
            using var db = new UnibeanDBContext();
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
            using var db = new UnibeanDBContext();
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
            using var db = new UnibeanDBContext();
            type = db.VoucherTypes
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
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
            using var db = new UnibeanDBContext();
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
