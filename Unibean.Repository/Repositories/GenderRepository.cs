using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class GenderRepository : IGenderRepository
{
    public Gender Add(Gender creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.Genders.Add(creation).Entity;
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
            var gender = db.Genders.FirstOrDefault(b => b.Id.Equals(id));
            gender.Status = false;
            db.Genders.Update(gender);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Gender> GetAll(string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Gender> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Genders
                .Where(t => (EF.Functions.Like(t.GenderName, "%" + search + "%")
                || EF.Functions.Like(t.FileName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .ToList();

            pagedResult = new PagedResultModel<Gender>
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

    public Gender GetById(string id)
    {
        Gender gender = new();
        try
        {
            using var db = new UnibeanDBContext();
            gender = db.Genders
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return gender;
    }

    public Gender Update(Gender update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.Genders.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
