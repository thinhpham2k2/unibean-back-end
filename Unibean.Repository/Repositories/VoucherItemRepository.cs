using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Type = Unibean.Repository.Entities.Type;

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

    public void AddList(IEnumerable<VoucherItem> creations)
    {
        try
        {
            using var db = new UnibeanDBContext();
            db.VoucherItems.AddRange(creations);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public bool CheckVoucherCode(string code)
    {
        VoucherItem voucher = new();
        try
        {
            using var db = new UnibeanDBContext();
            voucher = db.VoucherItems
            .Where(s => s.VoucherCode.Equals(code))
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return voucher != null;
    }

    public long CountVoucherItemToday(string brandId, DateOnly date)
    {
        long count = 0;
        try
        {
            using var db = new UnibeanDBContext();
            count = db.VoucherItems.Where(c => (bool)c.Status 
            && (bool)c.IsLocked && (bool)c.IsBought && (bool)c.IsUsed
            && c.Voucher.BrandId.Equals(brandId)
            && DateOnly.FromDateTime(c.Activities.Where(a => a.Type.Equals(Type.Use))
            .FirstOrDefault().DateCreated.Value).Equals(date)).Count();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return count;
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
        (List<string> campaignIds, List<string> campaignDetailIds, List<string> voucherIds, List<string> brandIds,
        List<string> typeIds, List<string> studentIds, bool? isLocked, bool? isBought, bool? isUsed, bool? state,
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
                && (campaignDetailIds.Count == 0 || campaignDetailIds.Contains(t.CampaignDetailId))
                && (voucherIds.Count == 0 || voucherIds.Contains(t.VoucherId))
                && (brandIds.Count == 0 || brandIds.Contains(t.CampaignDetail.Campaign.BrandId))
                && (typeIds.Count == 0 || typeIds.Contains(t.Voucher.TypeId))
                && (studentIds.Count == 0 || studentIds.Contains(t.Activities.FirstOrDefault(a
                    => (bool)a.Status).StudentId))
                && (isLocked == null || isLocked.Equals(t.IsLocked))
                && (isBought == null || isBought.Equals(t.IsBought))
                && (isUsed == null || isUsed.Equals(t.IsUsed))
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
            .Include(s => s.CampaignDetail.Campaign)
                .ThenInclude(c => c.CampaignActivities.Where(a => (bool)a.Status))
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

    public VoucherItem GetByVoucherCode(string code)
    {
        VoucherItem voucher = new();
        try
        {
            using var db = new UnibeanDBContext();
            voucher = db.VoucherItems
            .Where(s => s.VoucherCode.Equals(code) && (bool)s.Status)
            .Include(s => s.CampaignDetail)
                .ThenInclude(c => c.Campaign)
                    .ThenInclude(v => v.Type)
            .Include(s => s.CampaignDetail.Campaign)
                .ThenInclude(c => c.CampaignStores.Where(a => (bool)a.Status))
            .Include(s => s.CampaignDetail.Campaign)
                .ThenInclude(c => c.CampaignMajors.Where(a => (bool)a.Status))
            .Include(s => s.CampaignDetail.Campaign)
                .ThenInclude(c => c.CampaignCampuses.Where(a => (bool)a.Status))
            .Include(s => s.CampaignDetail.Campaign)
                .ThenInclude(c => c.CampaignActivities.Where(a => (bool)a.Status))
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

    public ItemIndex GetIndex
        (string voucherId, int quantity, int fromIndex)
    {

        try
        {
            using var db = new UnibeanDBContext();

            var list = db.VoucherItems.Where(
                i => i.VoucherId.Equals(voucherId)
                && (bool)i.State && (bool)i.Status
                && !(bool)i.IsLocked && !(bool)i.IsBought && !(bool)i.IsUsed
                && (fromIndex.Equals(0) || i.Index >= fromIndex)
                && i.CampaignDetail.Equals(null)).Take(quantity).ToList();

            return new ItemIndex
            {
                FromIndex = list.FirstOrDefault().Index,
                ToIndex = list.LastOrDefault().Index
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public int GetMaxIndex(string voucherId)
    {
        int index;
        try
        {
            using var db = new UnibeanDBContext();
            index = db.VoucherItems
            .Where(s => s.VoucherId.Equals(voucherId) && (bool)s.Status)
            .Max(s => s.Index) ?? 0;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return index;
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

    public void UpdateList
        (string voucherId, string campaignDetailId,
        int quantity, DateOnly StartOn, DateOnly EndOn, ItemIndex index)
    {
        try
        {
            using var db = new UnibeanDBContext();

            var list = db.VoucherItems.Where(
                i => i.VoucherId.Equals(voucherId)
                && (bool)i.State && (bool)i.Status && i.Index >= index.FromIndex
                && i.Index <= index.ToIndex && !(bool)i.IsLocked && !(bool)i.IsBought
                && !(bool)i.IsUsed && i.CampaignDetail.Equals(null)).Take(quantity).ToList()
                .Select(i =>
                {
                    i.CampaignDetailId = campaignDetailId;
                    i.IsLocked = true;
                    i.ValidOn = StartOn;
                    i.ExpireOn = EndOn;
                    i.DateIssued = DateTime.Now;
                    return i;
                });

            db.VoucherItems.UpdateRange(list);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
