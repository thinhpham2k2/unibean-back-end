using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class CampaignDetailRepository : ICampaignDetailRepository
{
    public CampaignDetail Add(CampaignDetail creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.CampaignDetails.Add(creation).Entity;
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
            var detail = db.CampaignDetails.FirstOrDefault(b => b.Id.Equals(id));
            detail.Status = false;
            db.CampaignDetails.Update(detail);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<CampaignDetail> GetAll
        (List<string> campaignIds, List<string> typeIds, bool? state, 
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<CampaignDetail> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.CampaignDetails
                .Where(t => (EF.Functions.Like(t.Voucher.VoucherName, "%" + search + "%")
                || EF.Functions.Like(t.Campaign.CampaignName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (campaignIds.Count == 0 || campaignIds.Contains(t.CampaignId))
                && (typeIds.Count == 0 || typeIds.Contains(t.Voucher.TypeId))
                && (state == null || state.Equals(t.State))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(a => a.Voucher)
                    .ThenInclude(v => v.Type)
               .Include(d => d.Campaign)
               .Include(d => d.VoucherItems.Where(v => (bool)v.Status && (bool)v.State))
               .ToList();

            pagedResult = new PagedResultModel<CampaignDetail>
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

    public PagedResultModel<CampaignDetail> GetAllByStore
        (string storeId, List<string> campaignIds, List<string> typeIds, 
        bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<CampaignDetail> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.CampaignDetails
                .Where(t => (EF.Functions.Like(t.Voucher.VoucherName, "%" + search + "%")
                || EF.Functions.Like(t.Campaign.CampaignName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (t.Campaign.CampaignStores.Select(c => c.StoreId).Contains(storeId))
                && (campaignIds.Count == 0 || campaignIds.Contains(t.CampaignId))
                && (typeIds.Count == 0 || typeIds.Contains(t.Voucher.TypeId))
                && (state == null || state.Equals(t.State))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(a => a.Voucher)
                    .ThenInclude(v => v.Type)
               .Include(d => d.Campaign)
               .Include(d => d.VoucherItems.Where(v => (bool)v.Status && (bool)v.State))
               .ToList();

            pagedResult = new PagedResultModel<CampaignDetail>
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

    public List<string> GetAllVoucherItemByCampaignDetail
        (string id)
    {
        List<string> items = new();
        try
        {
            using var db = new UnibeanDBContext();
            items = db.CampaignDetails
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .SelectMany(d => d.VoucherItems.Where(
                v => (bool)v.Status && (bool)v.State 
                && (bool)v.IsLocked && !(bool)v.IsBought 
                && !(bool)v.IsUsed && v.CampaignDetailId != null).Select(v => v.Id)).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return items;
    }

    public CampaignDetail GetById(string id)
    {
        CampaignDetail detail = new();
        try
        {
            using var db = new UnibeanDBContext();
            detail = db.CampaignDetails
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(a => a.Voucher)
                .ThenInclude(v => v.Type)
            .Include(d => d.Campaign)
            .Include(d => d.VoucherItems.Where(v => (bool)v.Status && (bool)v.State))
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return detail;
    }

    public CampaignDetail Update(CampaignDetail update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.CampaignDetails.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
