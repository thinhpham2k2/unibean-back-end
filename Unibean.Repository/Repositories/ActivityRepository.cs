using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.Repository.Repositories;

public class ActivityRepository : IActivityRepository
{
    private readonly UnibeanDBContext unibeanDB;

    public ActivityRepository(UnibeanDBContext unibeanDB)
    {
        this.unibeanDB = unibeanDB;
    }

    public Activity Add(Activity creation)
    {
        try
        {
            var db = unibeanDB;

            if (creation.Type.Equals(Type.Buy))
            {
                // Get green bean wallet student
                var student = db.Students
                        .Where(s => s.Id.Equals(creation.StudentId) && (bool)s.Status)
                        .Include(b => b.Wallets).FirstOrDefault();
                var studentWallet = student.Wallets.Where(w => w.Type.Equals(WalletType.Green)).FirstOrDefault();

                // Create Activity Transaction List
                var amount = creation.VoucherItem.CampaignDetail.Price;
                creation.ActivityTransactions = new List<ActivityTransaction> {
                    new() {
                        Id = Ulid.NewUlid().ToString(),
                        ActivityId = creation.Id,
                        WalletId = studentWallet.Id,
                        Amount = -amount,
                        Rate = 1,
                        Description = creation.Description,
                        State = creation.State,
                        Status = creation.Status,
                    }};

                creation.VoucherItem.IsBought = true;
                db.VoucherItems.Update(creation.VoucherItem);
                creation = db.Activities.Add(creation).Entity;

                if (creation != null)
                {
                    // Update student wallet balance
                    student.TotalSpending += amount;
                    studentWallet.Balance -= amount;
                    studentWallet.DateUpdated = DateTime.Now;

                    db.Students.Update(student);
                    db.Wallets.Update(studentWallet);
                }
            }
            else if (creation.Type.Equals(Type.Use))
            {
                // Get red bean wallet student
                var student = db.Students
                        .Where(s => s.Id.Equals(creation.StudentId) && (bool)s.Status)
                        .Include(b => b.Wallets).FirstOrDefault();
                var studentWallet = student.Wallets.Where(w => w.Type.Equals(WalletType.Red)).FirstOrDefault();

                // Get green bean wallet campaign
                var campaign = db.Campaigns
                        .Where(s => s.Id.Equals(creation.VoucherItem.CampaignDetail.CampaignId) && (bool)s.Status)
                        .Include(b => b.Wallets).FirstOrDefault();
                var campaignWallet = campaign.Wallets.Where(w => w.Type.Equals(WalletType.Green)).FirstOrDefault();
                var amount = creation.VoucherItem.CampaignDetail.Price * creation.VoucherItem.CampaignDetail.Rate;

                // Create Activity Transaction List
                creation.ActivityTransactions = new List<ActivityTransaction> {
                    new() {
                        Id = Ulid.NewUlid().ToString(),
                        ActivityId = creation.Id,
                        WalletId = campaignWallet.Id,
                        Amount = -amount,
                        Rate = creation.VoucherItem.CampaignDetail.Rate,
                        Description = creation.Description,
                        State = creation.State,
                        Status = creation.Status,
                    },
                    new() {
                        Id = Ulid.NewUlid().ToString(),
                        ActivityId = creation.Id,
                        WalletId = studentWallet.Id,
                        Amount = amount,
                        Rate = creation.VoucherItem.CampaignDetail.Rate,
                        Description = creation.Description,
                        State = creation.State,
                        Status = creation.Status,
                    }};
                creation.VoucherItem.IsUsed = true;
                db.VoucherItems.Update(creation.VoucherItem);
                creation = db.Activities.Add(creation).Entity;

                if (creation != null)
                {
                    // Update student wallet balance
                    studentWallet.Balance += amount;
                    studentWallet.DateUpdated = DateTime.Now;

                    // Update brand wallet balance
                    campaign.TotalSpending += amount;
                    campaignWallet.Balance -= amount;
                    campaignWallet.DateUpdated = DateTime.Now;

                    db.Students.Update(student);
                    db.Campaigns.Update(campaign);
                    db.Wallets.UpdateRange(studentWallet, campaignWallet);
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

    public long CountParticipantToday(string storeId, DateOnly date)
    {
        long count = 0;
        try
        {
            var db = unibeanDB;
            count = db.Activities.Where(c => (bool)c.Status
            && c.StoreId.Equals(storeId)
            && DateOnly.FromDateTime(c.DateCreated.Value).Equals(date)).Select(a => a.StudentId)
            .Distinct().Count();
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
            var activity = db.Activities.FirstOrDefault(b => b.Id.Equals(id));
            activity.Status = false;
            db.Activities.Update(activity);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Activity> GetAll
        (List<string> brandIds, List<string> storeIds, List<string> studentIds, List<string> campaignIds,
        List<string> campaignDetailIds, List<string> voucherIds, List<string> voucherItemIds, List<Type> typeIds,
        bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Activity> pagedResult = new();
        try
        {
            var db = unibeanDB;
            var query = db.Activities
                .Where(t => (EF.Functions.Like(t.Store.StoreName, "%" + search + "%")
                || EF.Functions.Like(t.Store.Address, "%" + search + "%")
                || EF.Functions.Like(t.Student.FullName, "%" + search + "%")
                || EF.Functions.Like(t.VoucherItem.Voucher.VoucherName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (brandIds.Count == 0 || brandIds.Contains(t.Store.BrandId))
                && (storeIds.Count == 0 || storeIds.Contains(t.StoreId))
                && (studentIds.Count == 0 || studentIds.Contains(t.StudentId))
                && (campaignIds.Count == 0 || campaignIds.Contains(t.VoucherItem.CampaignDetail.CampaignId))
                && (campaignDetailIds.Count == 0 || campaignDetailIds.Contains(t.VoucherItem.CampaignDetailId))
                && (voucherIds.Count == 0 || voucherIds.Contains(t.VoucherItem.VoucherId))
                && (voucherItemIds.Count == 0 || voucherItemIds.Contains(t.VoucherItemId))
                && (typeIds.Count == 0 || typeIds.Contains(t.Type.Value))
                && (state == null || state.Equals(t.State))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(s => s.ActivityTransactions.Where(a => (bool)a.Status))
                    .ThenInclude(a => a.Wallet)
               .Include(s => s.Store)
               .Include(s => s.Student)
               .Include(s => s.VoucherItem)
                    .ThenInclude(v => v.Voucher)
               .Include(s => s.VoucherItem)
                    .ThenInclude(v => v.CampaignDetail)
               .ToList();

            pagedResult = new PagedResultModel<Activity>
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

    public Activity GetById(string id)
    {
        Activity activity = new();
        try
        {
            var db = unibeanDB;
            activity = db.Activities
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.ActivityTransactions.Where(a => (bool)a.Status))
                .ThenInclude(a => a.Wallet)
            .Include(s => s.Student)
                .ThenInclude(s => s.Account)
            .Include(s => s.Store)
                .ThenInclude(s => s.Account)
            .Include(s => s.VoucherItem)
                .ThenInclude(v => v.Voucher)
                    .ThenInclude(v => v.Type)
            .Include(s => s.VoucherItem)
                .ThenInclude(v => v.Voucher)
                    .ThenInclude(v => v.Brand)
                        .ThenInclude(b => b.Account)
            .Include(s => s.VoucherItem)
                .ThenInclude(v => v.CampaignDetail)
                    .ThenInclude(d => d.Campaign)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return activity;
    }

    public List<Activity> GetList
        (List<string> storeIds, List<string> studentIds, List<string> voucherIds, string search)
    {
        List<Activity> result;
        try
        {
            var db = unibeanDB;
            result = db.Activities
                .Where(t => (EF.Functions.Like(t.Store.StoreName, "%" + search + "%")
                || EF.Functions.Like(t.Store.Address, "%" + search + "%")
                || EF.Functions.Like(t.Student.FullName, "%" + search + "%")
                || EF.Functions.Like(t.VoucherItem.Voucher.VoucherName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (storeIds.Count == 0 || storeIds.Contains(t.StoreId))
                && (studentIds.Count == 0 || studentIds.Contains(t.StudentId))
                && (voucherIds.Count == 0 || voucherIds.Contains(t.VoucherItemId))
                && (bool)t.Status && t.Type.Equals(Type.Use))
               .Include(s => s.ActivityTransactions.Where(a => (bool)a.Status && a.Amount > 0))
                    .ThenInclude(a => a.Wallet)
               .Include(s => s.Store)
               .Include(s => s.Student)
               .Include(s => s.VoucherItem)
                    .ThenInclude(v => v.Voucher).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }

    public Activity Update(Activity update)
    {
        try
        {
            var db = unibeanDB;
            update = db.Activities.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
