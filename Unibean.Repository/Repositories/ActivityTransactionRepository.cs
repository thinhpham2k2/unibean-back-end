using Microsoft.EntityFrameworkCore;
using System.Linq;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.Repository.Repositories;

public class ActivityTransactionRepository : IActivityTransactionRepository
{
    public ActivityTransaction Add(ActivityTransaction creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.ActivityTransactions.Add(creation).Entity;

            if (creation != null)
            {
                // Update wallet balance
                var wallet = db.Wallets.Where(w => (bool)w.Status && w.Id.Equals(creation.WalletId))
                    .FirstOrDefault();
                wallet.Balance += creation.Amount;
                wallet.DateUpdated = DateTime.Now;
                db.Wallets.Update(wallet);
            }
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return creation;
    }

    public List<ActivityTransaction> GetAll
        (List<string> walletIds, List<string> activityIds, string search)
    {
        List<ActivityTransaction> result;
        try
        {
            using var db = new UnibeanDBContext();
            result = db.ActivityTransactions
                .Where(a => (EF.Functions.Like(a.Activity.VoucherItem.Voucher.VoucherName, "%" + search + "%")
                || EF.Functions.Like((string)(object)a.Wallet.Type, "%" + search + "%")
                || EF.Functions.Like((string)(object)a.Activity.Type, "%" + search + "%")
                || EF.Functions.Like(a.Description, "%" + search + "%"))
                && (walletIds.Count == 0 || walletIds.Contains(a.WalletId))
                && (activityIds.Count == 0 || activityIds.Contains(a.ActivityId))
                && (bool)a.Status)
                .Include(s => s.Wallet)
                .Include(s => s.Activity)
                .Include(s => s.Activity)
                    .ThenInclude(a => a.VoucherItem)
                        .ThenInclude(v => v.Voucher).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }

    public ActivityTransaction GetById(string id)
    {
        ActivityTransaction activityTransaction = new();
        try
        {
            using var db = new UnibeanDBContext();
            activityTransaction = db.ActivityTransactions
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Wallet)
            .Include(s => s.Activity)
            .Include(s => s.Activity)
                .ThenInclude(a => a.VoucherItem)
                    .ThenInclude(v => v.Voucher)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return activityTransaction;
    }

    public decimal IncomeOfGreenBean(DateOnly date)
    {
        decimal result = 0;
        try
        {
            using var db = new UnibeanDBContext();
            result = -db.ActivityTransactions
                .Where(o => o.Wallet.Type.Equals(WalletType.Green)
                && o.Activity.Type.Equals(Type.Buy)
                && DateOnly.FromDateTime(o.Activity.DateCreated.Value).Equals(date)
                && (bool)o.Status).Select(o => o.Amount.Value).Sum();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }

    public decimal OutcomeOfGreenBean(string storeId, DateOnly date)
    {
        decimal result = 0;
        try
        {
            using var db = new UnibeanDBContext();
            var store = db.Stores.Where(s => s.Id.Equals(storeId));
            result = store.SelectMany(s => s.Activities
            .Where(a => (bool)a.Status && DateOnly.FromDateTime(a.DateCreated.Value).Equals(date))
            .SelectMany(a => a.ActivityTransactions.Where(t => t.Amount > 0 && (bool)t.Status)
            .Select(t => t.Amount))).Sum().Value;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }
}
