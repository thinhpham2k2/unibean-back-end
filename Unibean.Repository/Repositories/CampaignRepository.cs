using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.Repository.Repositories;

public class CampaignRepository : ICampaignRepository
{
    private readonly UnibeanDBContext unibeanDB;

    public CampaignRepository(UnibeanDBContext unibeanDB)
    {
        this.unibeanDB = unibeanDB;
    }

    public Campaign Add(Campaign creation)
    {
        try
        {
            var db = unibeanDB;

            // Create campaign activity
            creation.CampaignActivities = new List<CampaignActivity> {
                new() {
                    Id = Ulid.NewUlid().ToString(),
                    CampaignId = creation.Id,
                    State = CampaignState.Pending,
                    DateCreated = creation.DateCreated,
                    Description = CampaignState.Pending.GetEnumDescription(),
                    Status = true,
                }
            };

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
                new() {
                    Id = Ulid.NewUlid().ToString(),
                    CampaignId = creation.Id,
                    Type = WalletType.Green,
                    Balance = creation.TotalIncome,
                    DateCreated = creation.DateCreated,
                    DateUpdated = creation.DateUpdated,
                    Description = WalletType.Green.GetEnumDescription(),
                    State = true,
                    Status = true,
            }};

            // Get brand wallet
            var brand = db.Brands
                    .Where(s => s.Id.Equals(creation.BrandId) && (bool)s.Status)
                    .Include(b => b.Account)
                    .Include(b => b.Wallets).FirstOrDefault();
            var brandGreenWallet = brand.Wallets.Where(w => w.Type.Equals(WalletType.Green)).FirstOrDefault();

            // Cretae campaign transactions
            creation.CampaignTransactions = new List<CampaignTransaction>() {
                new() {
                // Transaction for campaign's green bean
                Id = Ulid.NewUlid().ToString(),
                CampaignId = creation.Id,
                WalletId = creation.Wallets.Where(w => w.Type.Equals(WalletType.Green)).FirstOrDefault().Id,
                Amount = creation.TotalIncome,
                Rate = 1,
                DateCreated = creation.DateCreated,
                State = true,
                Status = creation.Status,
            },
                // Transaction for brand's green bean wallet
                new() {
                Id = Ulid.NewUlid().ToString(),
                CampaignId = creation.Id,
                WalletId = brandGreenWallet.Id,
                Amount = -creation.TotalIncome,
                Rate = 1,
                DateCreated = creation.DateCreated,
                State = true,
                Status = creation.Status,
            }};

            creation = db.Campaigns.Add(creation).Entity;
            creation.Brand = brand;

            if (creation != null)
            {
                // Update brand green wallet balance
                brand.TotalSpending += creation.TotalIncome;
                brandGreenWallet.Balance -= creation.TotalIncome;
                brandGreenWallet.DateUpdated = DateTime.Now;
                db.Wallets.Update(brandGreenWallet);
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

    public List<string> AllToClosed(string id)
    {
        List<string> emails = new();
        try
        {
            var db = unibeanDB;

            // Get campaign wallet
            var campaign = db.Campaigns
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(c => c.Wallets.Where(w => (bool)w.Status))
            .Include(c => c.CampaignDetails.Where(d => (bool)d.Status))
                .ThenInclude(d => d.VoucherItems.Where(i => (bool)i.Status))
                    .ThenInclude(i => i.Activities.Where(a => (bool)a.Status))
                        .ThenInclude(a => a.Student)
                            .ThenInclude(s => s.Wallets.Where(w => (bool)w.Status))
            .Include(c => c.CampaignDetails.Where(d => (bool)d.Status))
                .ThenInclude(d => d.VoucherItems.Where(i => (bool)i.Status))
                    .ThenInclude(i => i.Activities.Where(a => (bool)a.Status))
                        .ThenInclude(a => a.Student)
                            .ThenInclude(s => s.Account)
            .Include(c => c.CampaignActivities.Where(d => (bool)d.Status))
            .FirstOrDefault();
            var campaignWallet = campaign.Wallets.FirstOrDefault();

            // Get campaign wallet
            var brand = db.Brands
            .Where(s => s.Id.Equals(campaign.BrandId) && (bool)s.Status)
            .Include(c => c.Wallets.Where(w => (bool)w.Status))
            .FirstOrDefault();
            var brandWallet = brand.Wallets.FirstOrDefault();

            // Refund For Brand
            // Cretae campaign transactions
            db.CampaignTransactions.AddRange(new List<CampaignTransaction>() {
                new() {
                // Transaction for campaign's green bean
                Id = Ulid.NewUlid().ToString(),
                CampaignId = campaign.Id,
                WalletId = campaignWallet.Id,
                Amount = -campaignWallet.Balance,
                Rate = 1,
                DateCreated = DateTime.Now,
                State = true,
                Status = true,
            },
                // Transaction for brand's green bean wallet
                new() {
                Id = Ulid.NewUlid().ToString(),
                CampaignId = campaign.Id,
                WalletId = brandWallet.Id,
                Amount = campaignWallet.Balance,
                Rate = 1,
                DateCreated = DateTime.Now,
                State = true,
                Status = true,
            }});

            // Update brand green wallet balance
            brandWallet.Balance += campaignWallet.Balance;
            brandWallet.DateUpdated = DateTime.Now;

            // Update campaign green wallet balance
            campaignWallet.Balance = 0;
            campaignWallet.DateUpdated = DateTime.Now;

            List<Wallet> walletList = new() { campaignWallet, brandWallet };

            // Refund For Student
            var itemList = campaign.CampaignDetails.SelectMany(d => d.VoucherItems.Where(
                i => (bool)i.State && (bool)i.Status
                && (bool)i.IsLocked && (bool)i.IsBought
                && !(bool)i.IsUsed && i.CampaignDetailId != null))
                .ToList();

            emails = itemList.Select(i => i.Activities.Where(a => a.Type.Equals(Type.Buy)).FirstOrDefault().Student.Account.Email).ToList();

            List<Activity> activityList = new();

            // Refund voucher item
            foreach (var item in itemList)
            {
                item.State = false;
                var activityId = Ulid.NewUlid().ToString();
                var wallet = item.Activities.Where(
                    a => a.Type.Equals(Type.Buy)).FirstOrDefault().Student.Wallets.Where(
                    w => w.Type.Equals(WalletType.Green)).FirstOrDefault();

                activityList.Add(new()
                {
                    Id = activityId,
                    StudentId = item.Activities.Where(
                        a => a.Type.Equals(Type.Buy)).First().StudentId,
                    VoucherItemId = item.Id,
                    Type = Type.Refund,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Description = Type.Refund.GetEnumDescription(),
                    State = true,
                    Status = true,
                    ActivityTransactions = new List<ActivityTransaction>()
                    {
                        new()
                        {
                            Id = Ulid.NewUlid().ToString(),
                            ActivityId = activityId,
                            WalletId = wallet.Id,
                            Amount = item.CampaignDetail.Price,
                            Rate = 1,
                            Description = Type.Refund.GetEnumDescription(),
                            State = true,
                            Status = true,
                        }
                    }
                });

                var w = walletList.Find(w => w.Id.Equals(wallet.Id));
                if (w != null)
                {
                    w.Balance += item.CampaignDetail.Price;
                }
                else
                {
                    wallet.Balance += item.CampaignDetail.Price;
                    walletList.Add(wallet);
                }
            }

            db.Activities.AddRange(activityList);
            db.VoucherItems.UpdateRange(itemList);
            db.Wallets.UpdateRange(walletList);

            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return emails;
    }

    public long CountCampaign()
    {
        long count = 0;
        try
        {
            var db = unibeanDB;
            count = db.Campaigns.Where(c => (bool)c.Status).Count();
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

    public bool ExpiredToClosed(string id)
    {
        try
        {
            var db = unibeanDB;

            // Get campaign wallet
            var campaign = db.Campaigns
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(c => c.Wallets.Where(w => (bool)w.Status))
            .FirstOrDefault();
            var campaignWallet = campaign.Wallets.FirstOrDefault();

            // Get campaign wallet
            var brand = db.Brands
            .Where(s => s.Id.Equals(campaign.BrandId) && (bool)s.Status)
            .Include(c => c.Wallets.Where(w => (bool)w.Status))
            .FirstOrDefault();
            var brandWallet = brand.Wallets.FirstOrDefault();

            // Refund For Brand
            // Cretae campaign transactions
            db.CampaignTransactions.AddRange(new List<CampaignTransaction>() {
                new() {
                // Transaction for campaign's green bean
                Id = Ulid.NewUlid().ToString(),
                CampaignId = campaign.Id,
                WalletId = campaignWallet.Id,
                Amount = -campaignWallet.Balance,
                Rate = 1,
                DateCreated = DateTime.Now,
                State = true,
                Status = true,
            },
                // Transaction for brand's green bean wallet
                new() {
                Id = Ulid.NewUlid().ToString(),
                CampaignId = campaign.Id,
                WalletId = brandWallet.Id,
                Amount = campaignWallet.Balance,
                Rate = 1,
                DateCreated = DateTime.Now,
                State = true,
                Status = true,
            }});

            // Update brand green wallet balance
            brandWallet.Balance += campaignWallet.Balance;
            brandWallet.DateUpdated = DateTime.Now;

            // Update campaign green wallet balance
            campaignWallet.Balance = 0;
            campaignWallet.DateUpdated = DateTime.Now;

            db.Wallets.UpdateRange(campaignWallet, brandWallet);

            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return true;
    }

    public PagedResultModel<Campaign> GetAll
        (List<string> brandIds, List<string> typeIds, List<string> storeIds,
        List<string> majorIds, List<string> campusIds, List<CampaignState> stateIds,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Campaign> pagedResult = new();
        try
        {
            var db = unibeanDB;
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
                && (stateIds.Count == 0 || stateIds.Contains(t.CampaignActivities.OrderBy(a => a.Id).LastOrDefault().State.Value))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(s => s.Brand)
               .Include(s => s.Type)
               .Include(s => s.Wallets.Where(w => (bool)w.Status))
               .Include(s => s.CampaignActivities.Where(a => (bool)a.Status).OrderBy(a => a.Id))
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

    public List<Campaign> GetAllEnded(List<CampaignState> stateIds)
    {
        List<Campaign> result = new();

        try
        {
            var db = unibeanDB;
            var query = db.Campaigns
                .Where(t =>
                stateIds.Contains(t.CampaignActivities.OrderBy(a => a.Id).LastOrDefault().State.Value)
                && t.TotalSpending >= t.TotalIncome && (bool)t.Status);

            result = query
               .Include(s => s.Brand)
               .Include(s => s.Type)
               .Include(s => s.Wallets.Where(w => (bool)w.Status))
               .Include(s => s.CampaignActivities.Where(a => (bool)a.Status).OrderBy(a => a.Id))
               .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }

    public List<Campaign> GetAllExpired
        (List<CampaignState> stateIds, DateOnly date)
    {
        List<Campaign> result = new();

        try
        {
            var db = unibeanDB;
            var query = db.Campaigns
                .Where(t =>
                stateIds.Contains(t.CampaignActivities.OrderBy(a => a.Id).LastOrDefault().State.Value)
                && t.EndOn < date && (bool)t.Status);

            result = query
               .Include(s => s.Brand)
               .Include(s => s.Type)
               .Include(s => s.Wallets.Where(w => (bool)w.Status))
               .Include(s => s.CampaignActivities.Where(a => (bool)a.Status).OrderBy(a => a.Id))
               .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }

    public Campaign GetById(string id)
    {
        Campaign campaign = new();
        try
        {
            var db = unibeanDB;
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
            .Include(s => s.CampaignTransactions.Where(w => (bool)w.Status))
                .ThenInclude(s => s.Wallet)
            .Include(s => s.CampaignDetails.Where(w => (bool)w.Status))
                .ThenInclude(s => s.VoucherItems)
                    .ThenInclude(v => v.Activities)
            .Include(s => s.CampaignActivities.Where(a => (bool)a.Status).OrderBy(a => a.Id))
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
            var db = unibeanDB;
            if (!update.CampaignActivities.LastOrDefault().State.Equals(CampaignState.Pending))
            {
                db.CampaignActivities.Add(new CampaignActivity
                {
                    Id = Ulid.NewUlid().ToString(),
                    CampaignId = update.Id,
                    State = CampaignState.Pending,
                    DateCreated = DateTime.Now,
                    Description = CampaignState.Pending.GetEnumDescription(),
                    Status = true,
                });
            }
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
