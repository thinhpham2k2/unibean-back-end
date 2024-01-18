using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class VoucherRepository : IVoucherRepository
{
    public Voucher Add(Voucher creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.Vouchers.Add(creation).Entity;
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
            var voucher = db.Vouchers.FirstOrDefault(b => b.Id.Equals(id));
            voucher.Status = false;
            db.Vouchers.Update(voucher);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Voucher> GetAll
        (List<string> brandIds, List<string> typeIds, string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Voucher> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Vouchers
                .Where(t => (EF.Functions.Like(t.VoucherName, "%" + search + "%")
                || EF.Functions.Like(t.Brand.BrandName, "%" + search + "%")
                || EF.Functions.Like(t.Type.TypeName, "%" + search + "%")
                || EF.Functions.Like(t.Condition, "%" + search + "%")
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
               .Include(s => s.VoucherItems.Where(v => (bool)v.Status))
               .ToList();

            pagedResult = new PagedResultModel<Voucher>
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

    public PagedResultModel<Voucher> GetAllByStore
        (List<string> storeIds, List<string> campaignIds, List<string> typeIds, 
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Voucher> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();

            var query = db.Stores
                .Where(s => (storeIds.Count == 0 || storeIds.Contains(s.Id))
                && (bool)s.Status)
                .SelectMany(s => s.CampaignStores.Where(c => (bool)c.Status
                && EF.Functions.Like(c.Campaign.CampaignName, "%" + search + "%")
                && (campaignIds.Count == 0 || campaignIds.Contains(c.CampaignId)))
                .SelectMany(c => c.Campaign.VoucherItems.Select(v => v.Voucher))).Distinct()
                .Where(t => (EF.Functions.Like(t.VoucherName, "%" + search + "%")
                || EF.Functions.Like(t.Condition, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (typeIds.Count == 0 || typeIds.Contains(t.TypeId)))
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(s => s.Brand)
               .Include(s => s.Type)
               .Include(s => s.VoucherItems.Where(v => (bool)v.Status))
               .ToList();

            pagedResult = new PagedResultModel<Voucher>
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

    public Voucher GetById(string id)
    {
        Voucher voucher = new();
        try
        {
            using var db = new UnibeanDBContext();
            voucher = db.Vouchers
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Brand)
            .Include(s => s.Type)
            .Include(s => s.VoucherItems.Where(v => (bool)v.Status))
                .ThenInclude(v => v.Campaign)
                    .ThenInclude(c => c.Brand)
            .Include(s => s.VoucherItems.Where(v => (bool)v.Status))
                .ThenInclude(v => v.Campaign)
                    .ThenInclude(c => c.Type)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return voucher;
    }

    public Voucher Update(Voucher update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.Vouchers.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
