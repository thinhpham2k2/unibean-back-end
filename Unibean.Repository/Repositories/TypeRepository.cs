using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.Repository.Repositories;

public class TypeRepository : ITypeRepository
{
    public Type Add(Type creation)
    {
        try
        {
            using (var db = new UnibeanDBContext())
            {
                creation = db.Types.Add(creation).Entity;
                db.SaveChanges();
            }
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
            using (var db = new UnibeanDBContext())
            {
                var type = db.Types.FirstOrDefault(b => b.Id.Equals(id));
                type.Status = false;
                db.Types.Update(type);
                db.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Type> GetAll(string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Type> pagedResult = new PagedResultModel<Type>();
        try
        {
            using (var db = new UnibeanDBContext())
            {
                var query = db.Types
                    .Where(s => (EF.Functions.Like(s.TypeName, "%" + search + "%")
                    || EF.Functions.Like(s.Image, "%" + search + "%")
                    || EF.Functions.Like(s.Description, "%" + search + "%"))
                    && s.Status.Equals(true))
                    .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

                var result = query
                   .Skip((page - 1) * limit)
                   .Take(limit)
                   .ToList();

                pagedResult = new PagedResultModel<Type>
                {
                    CurrentPage = page,
                    PageSize = limit,
                    PageCount = (int)Math.Ceiling((double)query.Count() / limit),
                    Result = result,
                    RowCount = result.Count(),
                    TotalCount = query.Count()
                };
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return pagedResult;
    }

    public Type GetById(string id)
    {
        Type type = new Type();
        try
        {
            using (var db = new UnibeanDBContext())
            {
                type = db.Types
                .Where(s => s.Id.Equals(id) && s.Status.Equals(true))
                .FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return type;
    }

    public Type Update(Type update)
    {
        try
        {
            using (var db = new UnibeanDBContext())
            {
                update = db.Types.Update(update).Entity;
                db.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
