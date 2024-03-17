using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class MajorRepository : IMajorRepository
{
    private readonly UnibeanDBContext unibeanDB;

    public MajorRepository(UnibeanDBContext unibeanDB)
    {
        this.unibeanDB = unibeanDB;
    }

    public Major Add(Major creation)
    {
        try
        {
            var db = unibeanDB;
            creation = db.Majors.Add(creation).Entity;
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
            var major = db.Majors.FirstOrDefault(b => b.Id.Equals(id));
            major.Status = false;
            db.Majors.Update(major);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Major> GetAll
        (bool? state, string propertySort, bool isAsc,
        string search, int page, int limit)
    {
        PagedResultModel<Major> pagedResult = new();
        try
        {
            var db = unibeanDB;
            var query = db.Majors
                .Where(t => (EF.Functions.Like(t.MajorName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (state == null || state.Equals(t.State))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .ToList();

            pagedResult = new PagedResultModel<Major>
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

    public PagedResultModel<Major> GetAllByCampaign
        (List<string> campaignIds, bool? state, string propertySort,
        bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Major> pagedResult = new();
        try
        {
            var db = unibeanDB;
            var query = db.Campaigns
                .Where(c => (campaignIds.Count == 0 || campaignIds.Contains(c.Id))
                && (bool)c.Status)
                .SelectMany(c => c.CampaignMajors.Where(c => (bool)c.Status).Select(v => v.Major)).Distinct()
                .Where(t => EF.Functions.Like(t.MajorName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%")
                && (state == null || state.Equals(t.State)))
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .ToList();

            pagedResult = new PagedResultModel<Major>
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

    public Major GetById(string id)
    {
        Major major = new();
        try
        {
            var db = unibeanDB;
            major = db.Majors
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(m => m.Students.Where(s => (bool)s.Status))
            .Include(m => m.CampaignMajors.Where(c => (bool)c.Status))
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return major;
    }

    public Major Update(Major update)
    {
        try
        {
            var db = unibeanDB;
            update = db.Majors.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
