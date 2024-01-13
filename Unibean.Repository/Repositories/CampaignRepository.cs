using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class CampaignRepository : ICampaignRepository
{
    public Campaign Add(Campaign creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.Campaigns.Add(creation).Entity;
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
            var campaign = db.Campaigns.FirstOrDefault(b => b.Id.Equals(id));
            campaign.Status = false;
            db.Campaigns.Update(campaign);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Campaign> GetAll
        (List<string> brandIds, List<string> typeIds, string propertySort, bool isAsc, 
        string search, int page, int limit)
    {
        PagedResultModel<Campaign> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Campaigns
                .Where(t => (EF.Functions.Like(t.CampaignName, "%" + search + "%")
                || EF.Functions.Like(t.Condition, "%" + search + "%")
                || EF.Functions.Like(t.Brand.BrandName, "%" + search + "%")
                || EF.Functions.Like(t.Type.TypeName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (brandIds.Count == 0 || brandIds.Contains(t.BrandId))
                && (typeIds.Count == 0 || typeIds.Contains(t.TypeId))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(s => s.Brand)
               .Include(s => s.Type)
               .ToList();

            pagedResult = new PagedResultModel<Campaign>
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

    public Campaign GetById(string id)
    {
        Campaign campaign = new();
        try
        {
            using var db = new UnibeanDBContext();
            campaign = db.Campaigns
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Brand)
            .Include(s => s.Type)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return campaign;
    }

    public Campaign Update(Campaign update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.Campaigns.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
