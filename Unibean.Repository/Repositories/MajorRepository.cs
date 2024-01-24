using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class MajorRepository : IMajorRepository
{
    public Major Add(Major creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
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
            using var db = new UnibeanDBContext();
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

    public PagedResultModel<Major> GetAll(string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Major> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Majors
                .Where(t => (EF.Functions.Like(t.MajorName, "%" + search + "%")
                || EF.Functions.Like(t.FileName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
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
        (List<string> campaignIds, string propertySort, 
        bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Major> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Campaigns
                .Where(c => (campaignIds.Count == 0 || campaignIds.Contains(c.Id))
                && (bool)c.Status)
                .SelectMany(c => c.CampaignMajors.Where(c => (bool)c.Status).Select(v => v.Major)).Distinct()
                .Where(t => EF.Functions.Like(t.MajorName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
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
            using var db = new UnibeanDBContext();
            major = db.Majors
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
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
            using var db = new UnibeanDBContext();
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
