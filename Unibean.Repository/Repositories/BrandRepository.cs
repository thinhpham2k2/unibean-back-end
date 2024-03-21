using Microsoft.EntityFrameworkCore;
using MoreLinq;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly UnibeanDBContext unibeanDB;

    public BrandRepository(UnibeanDBContext unibeanDB)
    {
        this.unibeanDB = unibeanDB;
    }

    public Brand Add(Brand creation)
    {
        try
        {
            var db = unibeanDB;
            creation = db.Brands.Add(creation).Entity;

            if (creation != null)
            {
                // Create wallet
                db.Wallets.Add(new Wallet
                {
                    Id = Ulid.NewUlid().ToString(),
                    BrandId = creation.Id,
                    Type = WalletType.Green,
                    Balance = 0,
                    DateCreated = creation.DateCreated,
                    DateUpdated = creation.DateUpdated,
                    Description = WalletType.Green.GetEnumDescription(),
                    State = true,
                    Status = true,
                });
            }
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return creation;
    }

    public bool CheckBrandId(string id)
    {
        Brand brand = new();
        try
        {
            var db = unibeanDB;
            brand = db.Brands
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return brand != null;
    }

    public long CountBrand()
    {
        long count = 0;
        try
        {
            var db = unibeanDB;
            count = db.Brands.Where(c => (bool)c.Status).Count();
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
            var db = unibeanDB;
            var brand = db.Brands.FirstOrDefault(b => b.Id.Equals(id));
            brand.Status = false;
            db.Brands.Update(brand);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Brand> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Brand> pagedResult = new();
        try
        {
            var db = unibeanDB;
            var query = db.Brands
                .Where(p => (EF.Functions.Like(p.Id, "%" + search + "%")
                || EF.Functions.Like(p.BrandName, "%" + search + "%")
                || EF.Functions.Like(p.Acronym, "%" + search + "%")
                || EF.Functions.Like(p.Address, "%" + search + "%")
                || EF.Functions.Like(p.CoverFileName, "%" + search + "%")
                || EF.Functions.Like(p.Description, "%" + search + "%"))
                && (state == null || state.Equals(p.State))
                && (bool)p.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(b => b.Account)
               .Include(s => s.Wishlists.Where(w => (bool)w.Status))
               .Include(s => s.Wallets.Where(w => (bool)w.Status))
               .ToList();

            pagedResult = new PagedResultModel<Brand>
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

    public Brand GetById(string id)
    {
        Brand brand = new();
        try
        {
            var db = unibeanDB;
            brand = db.Brands
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Account)
            .Include(s => s.Wishlists.Where(w => (bool)w.Status))
            .Include(s => s.Wallets.Where(w => (bool)w.Status))
            .Include(s => s.Campaigns.Where(c => (bool)c.Status))
                .ThenInclude(c => c.Type)
            .Include(s => s.Campaigns.Where(c => (bool)c.Status))
                .ThenInclude(c => c.CampaignActivities.Where(c => (bool)c.Status))
            .Include(s => s.Stores.Where(s => (bool)s.Status))
                .ThenInclude(s => s.Area)
            .Include(s => s.Vouchers.Where(s => (bool)s.Status))
                .ThenInclude(s => s.Type)
            .Include(s => s.Vouchers.Where(s => (bool)s.Status))
                .ThenInclude(s => s.VoucherItems.Where(i => (bool)i.Status))
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return brand;
    }

    public List<Brand> GetRanking(int limit)
    {
        List<Brand> result = new();
        try
        {
            var db = unibeanDB;
            result.AddRange(db.Brands.Where(
                b => (bool)b.Status).OrderByDescending(
                b => b.TotalSpending).Take(limit).Include(b => b.Account));
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }

    public Brand Update(Brand update)
    {
        try
        {
            var db = unibeanDB;
            update = db.Brands.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
