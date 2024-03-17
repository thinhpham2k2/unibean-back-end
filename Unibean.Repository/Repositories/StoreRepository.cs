using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class StoreRepository : IStoreRepository
{
    private readonly UnibeanDBContext unibeanDB;

    public StoreRepository(UnibeanDBContext unibeanDB)
    {
        this.unibeanDB = unibeanDB;
    }

    public Store Add(Store creation)
    {
        try
        {
            var db = unibeanDB;
            creation = db.Stores.Add(creation).Entity;
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
            var store = db.Stores.FirstOrDefault(b => b.Id.Equals(id));
            store.Status = false;
            db.Stores.Update(store);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Store> GetAll
        (List<string> brandIds, List<string> areaIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Store> pagedResult = new();
        try
        {
            var db = unibeanDB;
            var query = db.Stores
                .Where(t => (EF.Functions.Like(t.StoreName, "%" + search + "%")
                || EF.Functions.Like(t.Brand.BrandName, "%" + search + "%")
                || EF.Functions.Like(t.Area.AreaName, "%" + search + "%")
                || EF.Functions.Like(t.Address, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (brandIds.Count == 0 || brandIds.Contains(t.BrandId))
                && (areaIds.Count == 0 || areaIds.Contains(t.AreaId))
                && (state == null || state.Equals(t.State))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(s => s.Brand)
               .Include(s => s.Area)
               .Include(s => s.Account)
               .ToList();

            pagedResult = new PagedResultModel<Store>
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

    public PagedResultModel<Store> GetAllByCampaign
        (List<string> campaignIds, List<string> brandIds, List<string> areaIds,
        bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Store> pagedResult = new();
        try
        {
            var db = unibeanDB;
            var query = db.Campaigns
                .Where(c => (campaignIds.Count == 0 || campaignIds.Contains(c.Id))
                && (brandIds.Count == 0 || brandIds.Contains(c.BrandId))
                && (bool)c.Status)
                .SelectMany(c => c.CampaignStores.Where(c => (bool)c.Status).Select(v => v.Store)).Distinct()
                .Where(t => (EF.Functions.Like(t.StoreName, "%" + search + "%")
                || EF.Functions.Like(t.Address, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (areaIds.Count == 0 || areaIds.Contains(t.AreaId))
                && (state == null || state.Equals(t.State)))
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(s => s.Brand)
               .Include(s => s.Area)
               .Include(s => s.Account)
               .ToList();

            pagedResult = new PagedResultModel<Store>
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

    public Store GetById(string id)
    {
        Store store = new();
        try
        {
            var db = unibeanDB;
            store = db.Stores
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Brand)
                .ThenInclude(b => b.Account)
            .Include(s => s.Brand)
                .ThenInclude(b => b.Wallets.Where(w => (bool)w.Status))
            .Include(s => s.Area)
            .Include(s => s.Account)
            .Include(s => s.CampaignStores.Where(c => (bool)c.Status))
                .ThenInclude(s => s.Campaign.CampaignActivities.Where(a => (bool)a.Status))
            .Include(s => s.Activities.Where(a => (bool)a.Status))
            .Include(s => s.Bonuses.Where(b => (bool)b.Status))
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return store;
    }

    public Store Update(Store update)
    {
        try
        {
            var db = unibeanDB;
            update = db.Stores.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
