using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class CampaignActivityRepository : ICampaignActivityRepository
{
    private readonly UnibeanDBContext unibeanDB;

    public CampaignActivityRepository(UnibeanDBContext unibeanDB)
    {
        this.unibeanDB = unibeanDB;
    }

    public CampaignActivity Add(CampaignActivity creation)
    {
        try
        {
            var db = unibeanDB;
            creation = db.CampaignActivities.Add(creation).Entity;
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
            var activity = db.CampaignActivities.FirstOrDefault(b => b.Id.Equals(id));
            activity.Status = false;
            db.CampaignActivities.Update(activity);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<CampaignActivity> GetAll
        (List<string> campaignIds, List<CampaignState> stateIds,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<CampaignActivity> pagedResult = new();
        try
        {
            var db = unibeanDB;
            var query = db.CampaignActivities
                .Where(t => (EF.Functions.Like((string)(object)t.State, "%" + search + "%")
                || EF.Functions.Like(t.Campaign.CampaignName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (campaignIds.Count == 0 || campaignIds.Contains(t.CampaignId))
                && (stateIds.Count == 0 || stateIds.Contains(t.State.Value))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(d => d.Campaign)
                    .ThenInclude(c => c.Type)
               .Include(d => d.Campaign)
                    .ThenInclude(c => c.Brand)
               .ToList();

            pagedResult = new PagedResultModel<CampaignActivity>
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

    public CampaignActivity GetById(string id)
    {
        CampaignActivity activity = new();
        try
        {
            var db = unibeanDB;
            activity = db.CampaignActivities
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(d => d.Campaign)
                .ThenInclude(c => c.Type)
            .Include(d => d.Campaign)
                .ThenInclude(c => c.Brand)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return activity;
    }

    public CampaignActivity Update(CampaignActivity update)
    {
        try
        {
            var db = unibeanDB;
            update = db.CampaignActivities.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
