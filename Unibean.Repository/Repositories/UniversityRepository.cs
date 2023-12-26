using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class UniversityRepository : IUniversityRepository
{
    public University Add(University creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.Universities.Add(creation).Entity;
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
            var university = db.Universities.FirstOrDefault(b => b.Id.Equals(id));
            university.Status = false;
            db.Universities.Update(university);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<University> GetAll(string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<University> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Universities
                .Where(t => (EF.Functions.Like(t.UniversityName, "%" + search + "%")
                || EF.Functions.Like(t.Phone, "%" + search + "%")
                || EF.Functions.Like(t.Email, "%" + search + "%")
                || EF.Functions.Like(t.Link, "%" + search + "%")
                || EF.Functions.Like(t.FileName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && t.Status.Equals(true))
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .ToList();

            pagedResult = new PagedResultModel<University>
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

    public University GetById(string id)
    {
        University university = new();
        try
        {
            using var db = new UnibeanDBContext();
            university = db.Universities
            .Where(s => s.Id.Equals(id) && s.Status.Equals(true))
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return university;
    }

    public University Update(University update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.Universities.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
