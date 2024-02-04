using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq.Dynamic.Core;
using System.Reflection;
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

            // Create campaign stores
            creation.CampaignStores = creation.CampaignStores.Select(c =>
            {
                c.CampaignId = creation.Id;
                return c;
            }).ToList();

            // Create campaign majors
            creation.CampaignMajors = creation.CampaignMajors.Select(c =>
            {
                c.CampaignId = creation.Id;
                return c;
            }).ToList();

            // Create campaign campuses
            creation.CampaignCampuses = creation.CampaignCampuses.Select(c =>
            {
                c.CampaignId = creation.Id;
                return c;
            }).ToList();

            // Create campaign wallet
            creation.Wallets = new List<Wallet>() {
                new Wallet
            {
                    Id = Ulid.NewUlid().ToString(),
                    CampaignId = creation.Id,
                    Type = WalletType.Green,
                    Balance = creation.TotalIncome,
                    DateCreated = creation.DateCreated,
                    DateUpdated = creation.DateUpdated,
                    Description = WalletType.Green.GetType().GetCustomAttribute<DescriptionAttribute>().ToString(),
                    State = true,
                    Status = true,
            }};

            // Get brand wallet
            var brand = db.Brands
                    .Where(s => s.Id.Equals(creation.BrandId) && (bool)s.Status)
                    .Include(b => b.Account)
                    .Include(b => b.Wallets).FirstOrDefault();
            var brandGreenWallet = brand.Wallets.FirstOrDefault();
            var brandRedWallet = brand.Wallets.FirstOrDefault();

            var amount = brandRedWallet.Balance - creation.TotalIncome;

            // Cretae campaign transactions
            creation.CampaignTransactions = new List<CampaignTransaction>() {
                new CampaignTransaction
            {
                // Transaction for campaign's green bean
                Id = Ulid.NewUlid().ToString(),
                CampaignId = creation.Id,
                WalletId = creation.Wallets.FirstOrDefault().Id,
                Amount = creation.TotalIncome,
                Rate = 1,
                DateCreated = creation.DateCreated,
                State = true,
                Status = creation.Status,
            },
                // Transaction for brand's green bean
                new CampaignTransaction
            {
                Id = Ulid.NewUlid().ToString(),
                CampaignId = creation.Id,
                WalletId = brandRedWallet.Id,
                Amount = amount > 0 ? -creation.TotalIncome : -brandRedWallet.Balance,
                Rate = 1,
                DateCreated = creation.DateCreated,
                State = true,
                Status = creation.Status,
            }};

            if (amount < 0)
            {
                // Transaction for brand's green bean
                creation.CampaignTransactions.Add(new CampaignTransaction
                {
                    Id = Ulid.NewUlid().ToString(),
                    CampaignId = creation.Id,
                    WalletId = brandGreenWallet.Id,
                    Amount = amount,
                    Rate = 1,
                    DateCreated = creation.DateCreated,
                    State = true,
                    Status = creation.Status,
                });
            }

            creation = db.Campaigns.Add(creation).Entity;
            creation.Brand = brand;

            if (creation != null)
            {
                // Update brand red wallet balance
                brandRedWallet.Balance -= amount > 0 ? creation.TotalIncome : brandRedWallet.Balance;
                brandRedWallet.DateUpdated = DateTime.Now;
                db.Wallets.Update(brandRedWallet);

                if (amount < 0)
                {
                    // Update brand green wallet balance
                    brand.TotalSpending += -amount;
                    brandGreenWallet.Balance -= -amount;
                    brandGreenWallet.DateUpdated = DateTime.Now;
                    db.Wallets.Update(brandGreenWallet);
                }

                db.Brands.Update(brand);
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
        (List<string> brandIds, List<string> typeIds, List<string> storeIds, 
        List<string> majorIds, List<string> campusIds, List<CampaignState> stateIds, 
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Campaign> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Campaigns
                .Where(t => (EF.Functions.Like(t.CampaignName, "%" + search + "%")
                || EF.Functions.Like(t.Link, "%" + search + "%")
                || EF.Functions.Like(t.Condition, "%" + search + "%")
                || EF.Functions.Like(t.Brand.BrandName, "%" + search + "%")
                || EF.Functions.Like(t.Type.TypeName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (brandIds.Count == 0 || brandIds.Contains(t.BrandId))
                && (typeIds.Count == 0 || typeIds.Contains(t.TypeId))
                && (storeIds.Count == 0 || t.CampaignStores.Select(c => c.StoreId).Any(s => storeIds.Contains(s)))
                && (majorIds.Count == 0 || t.CampaignMajors.Select(c => c.MajorId).Any(s => majorIds.Contains(s)))
                && (campusIds.Count == 0 || t.CampaignCampuses.Select(c => c.CampusId).Any(s => campusIds.Contains(s)))
                && (stateIds.Count == 0 || stateIds.Contains(t.CampaignActivities.LastOrDefault().State.Value))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(s => s.Brand)
               .Include(s => s.Type)
               .Include(s => s.Wallets.Where(w => (bool)w.Status))
                    .ThenInclude(s => s.Type)
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
                .ThenInclude(b => b.Account)
            .Include(s => s.Type)
            .Include(s => s.CampaignStores.Where(s => (bool)s.Status))
                .ThenInclude(s => s.Store)
            .Include(s => s.CampaignMajors.Where(s => (bool)s.Status))
                .ThenInclude(s => s.Major)
            .Include(s => s.CampaignCampuses.Where(s => (bool)s.Status))
                .ThenInclude(s => s.Campus)
            .Include(s => s.Wallets.Where(w => (bool)w.Status))
                .ThenInclude(s => s.Type)
            .Include(s => s.CampaignTransactions.Where(w => (bool)w.Status))
                .ThenInclude(s => s.Wallet)
                    .ThenInclude(w => w.Type)
            .Include(s => s.CampaignDetails.Where(w => (bool)w.Status))
                .ThenInclude(s => s.VoucherItems)
                    .ThenInclude(v => v.Activities)
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
