using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class VoucherItemRepository : IVoucherItemRepository
{
    public VoucherItem Add(VoucherItem creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.VoucherItems.Add(creation).Entity;
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
            var voucher = db.VoucherItems.FirstOrDefault(b => b.Id.Equals(id));
            voucher.Status = false;
            db.VoucherItems.Update(voucher);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<VoucherItem> GetAll
        (List<string> campaignIds, List<string> voucherIds, List<string> brandIds,
        List<string> typeIds, List<string> studentIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<VoucherItem> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.VoucherItems
                .Where(t => (EF.Functions.Like(t.VoucherCode, "%" + search + "%")
                || EF.Functions.Like(t.CampaignDetail.Campaign.CampaignName, "%" + search + "%")
                || EF.Functions.Like(t.CampaignDetail.Campaign.Condition, "%" + search + "%")
                || EF.Functions.Like(t.Voucher.VoucherName, "%" + search + "%")
                || EF.Functions.Like(t.Voucher.Condition, "%" + search + "%")
                || EF.Functions.Like(t.Voucher.Brand.BrandName, "%" + search + "%")
                || EF.Functions.Like(t.CampaignDetail.Description, "%" + search + "%"))
                && (campaignIds.Count == 0 || campaignIds.Contains(t.CampaignDetail.CampaignId))
                && (voucherIds.Count == 0 || voucherIds.Contains(t.VoucherId))
                && (brandIds.Count == 0 || brandIds.Contains(t.Voucher.BrandId))
                && (typeIds.Count == 0 || typeIds.Contains(t.Voucher.TypeId))
                && (studentIds.Count == 0 || studentIds.Contains(t.Activities.FirstOrDefault(a
                    => (bool)a.Status).StudentId))
                && (state == null || state.Equals(t.State))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(s => s.CampaignDetail)
                    .ThenInclude(c => c.Campaign)
                        .ThenInclude(v => v.Type)
               .Include(s => s.Voucher)
                    .ThenInclude(v => v.Type)
               .Include(s => s.Voucher)
                    .ThenInclude(v => v.Brand)
                        .ThenInclude(b => b.Account)
               .Include(s => s.Activities.Where(a => (bool)a.Status))
                    .ThenInclude(a => a.Student)
               .Include(s => s.Activities.Where(a => (bool)a.Status))
                    .ThenInclude(a => a.Store)
               .ToList();

            pagedResult = new PagedResultModel<VoucherItem>
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

    public List<VoucherItem> GetAllByCampaign
        (List<string> campaignIds, List<string> voucherIds, int limit)
     {
        try
        {
            using var db = new UnibeanDBContext();

            var query = db.VoucherItems
                .Where(t => (campaignIds.Count == 0 || campaignIds.Contains(t.CampaignDetail.CampaignId))
                && (voucherIds.Count == 0 || voucherIds.Contains(t.CampaignDetail.CampaignId))
                && !(bool)t.IsBought
                && !(bool)t.IsUsed
                && (bool)t.State
                && (bool)t.Status);

            return query.Take(limit)
                .Include(v => v.Voucher)
                .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public VoucherItem GetById(string id)
    {
        VoucherItem voucher = new();
        try
        {
            using var db = new UnibeanDBContext();
            voucher = db.VoucherItems
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.CampaignDetail)
                .ThenInclude(c => c.Campaign)
                    .ThenInclude(v => v.Type)
            .Include(s => s.CampaignDetail.Campaign)
                .ThenInclude(c => c.CampaignStores.Where(a => (bool)a.Status))
            .Include(s => s.CampaignDetail.Campaign)
                .ThenInclude(c => c.CampaignMajors.Where(a => (bool)a.Status))
            .Include(s => s.CampaignDetail.Campaign)
                .ThenInclude(c => c.CampaignCampuses.Where(a => (bool)a.Status))
            .Include(s => s.Voucher)
                .ThenInclude(v => v.Type)
            .Include(s => s.Voucher)
                .ThenInclude(v => v.Brand)
                    .ThenInclude(b => b.Account)
            .Include(s => s.Activities.Where(a => (bool)a.Status))
                .ThenInclude(a => a.Student)
            .Include(s => s.Activities.Where(a => (bool)a.Status))
                .ThenInclude(a => a.Store)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return voucher;
    }

    public VoucherItem Update(VoucherItem update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.VoucherItems.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
