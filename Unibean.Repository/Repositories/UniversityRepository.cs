using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class UniversityRepository : IUniversityRepository
{
    private readonly UnibeanDBContext unibeanDB;

    public UniversityRepository(UnibeanDBContext unibeanDB)
    {
        this.unibeanDB = unibeanDB;
    }

    public University Add(University creation)
    {
        try
        {
            var db = unibeanDB;
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
            var db = unibeanDB;
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

    public PagedResultModel<University> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<University> pagedResult = new();
        try
        {
            var db = unibeanDB;
            var query = db.Universities
                .Where(t => (EF.Functions.Like(t.UniversityName, "%" + search + "%")
                || EF.Functions.Like(t.Phone, "%" + search + "%")
                || EF.Functions.Like(t.Email, "%" + search + "%")
                || EF.Functions.Like(t.Link, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (state == null || state.Equals(t.State))
                && (bool)t.Status)
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
            var db = unibeanDB;
            university = db.Universities
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Campuses.Where(c => (bool)c.Status))
                .ThenInclude(c => c.Students.Where(s => (bool)s.Status))
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
            var db = unibeanDB;
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
