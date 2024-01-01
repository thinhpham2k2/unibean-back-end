using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class BrandRepository : IBrandRepository
{
    public Brand Add(Brand creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.Brands.Add(creation).Entity;

            if(creation != null)
            {
                // Create wallet
                foreach (var type in db.WalletTypes.Where(s => s.Status.Equals(true)))
                {
                    db.Wallets.Add(new Wallet
                    {
                        Id = Ulid.NewUlid().ToString(),
                        BrandId = creation.Id,
                        TypeId = type.Id,
                        Balance = 0,
                        DateCreated = creation.DateCreated,
                        DateUpdated = creation.DateUpdated,
                        Description = type.Description,
                        State = true,
                        Status = true,
                    });
                }
            }
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

    public PagedResultModel<Brand> GetAll(string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Brand> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Brands
                .Where(p => (EF.Functions.Like(p.Id, "%" + search + "%")
                || EF.Functions.Like(p.BrandName, "%" + search + "%")
                || EF.Functions.Like(p.Acronym, "%" + search + "%")
                || EF.Functions.Like(p.Address, "%" + search + "%")
                || EF.Functions.Like(p.CoverFileName, "%" + search + "%")
                || EF.Functions.Like(p.Description, "%" + search + "%"))
                && p.Status.Equals(true))
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(b => b.Account)
               .Include(s => s.Wishlists.Where(w => w.Status.Equals(true)))
               .Include(s => s.Wallets.Where(w => w.Status.Equals(true)))
                   .ThenInclude(w => w.Type)
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
            using var db = new UnibeanDBContext();
            brand = db.Brands
            .Where(s => s.Id.Equals(id) && s.Status.Equals(true))
            .Include(s => s.Account)
            .Include(s => s.Wishlists.Where(w => w.Status.Equals(true)))
            .Include(s => s.Wallets.Where(w => w.Status.Equals(true)))
                .ThenInclude(w => w.Type)
            .Include(s => s.Campaigns.Where(c => c.Status.Equals(true)))
                .ThenInclude(c => c.Type)
            .Include(s => s.Stores.Where(s => s.Status.Equals(true)))
                .ThenInclude(s => s.Area)
            .Include(s => s.Vouchers.Where(s => s.Status.Equals(true)))
                .ThenInclude(s => s.Type)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return brand;
    }

    public Brand Update(Brand update)
    {
        try
        {
            using var db = new UnibeanDBContext();
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
