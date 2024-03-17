using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class CampaignTypeRepository : ICampaignTypeRepository
{
    private readonly UnibeanDBContext unibeanDB;

    public CampaignTypeRepository(UnibeanDBContext unibeanDB)
    {
        this.unibeanDB = unibeanDB;
    }

    public CampaignType Add(CampaignType creation)
    {
        try
        {
            var db = unibeanDB;
            creation = db.CampaignTypes.Add(creation).Entity;
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
            var type = db.CampaignTypes.FirstOrDefault(b => b.Id.Equals(id));
            type.Status = false;
            db.CampaignTypes.Update(type);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<CampaignType> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<CampaignType> pagedResult = new();
        try
        {
            var db = unibeanDB;
            var query = db.CampaignTypes
                .Where(t => (EF.Functions.Like(t.TypeName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (state == null || state.Equals(t.State))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .ToList();

            pagedResult = new PagedResultModel<CampaignType>
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

    public CampaignType GetById(string id)
    {
        CampaignType type = new();
        try
        {
            var db = unibeanDB;
            type = db.CampaignTypes
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(t => t.Campaigns.Where(c => (bool)c.Status))
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return type;
    }

    public CampaignType Update(CampaignType update)
    {
        try
        {
            var db = unibeanDB;
            update = db.CampaignTypes.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
