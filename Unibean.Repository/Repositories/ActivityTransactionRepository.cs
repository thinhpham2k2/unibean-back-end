using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;

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
                || EF.Functions.Like(a.Wallet.Type.TypeName, "%" + search + "%")
                || EF.Functions.Like((string)(object)a.Activity.Type, "%" + search + "%")
                || EF.Functions.Like(a.Description, "%" + search + "%"))
                && (walletIds.Count == 0 || walletIds.Contains(a.WalletId))
                && (activityIds.Count == 0 || activityIds.Contains(a.ActivityId))
                && (bool)a.Status)
                .Include(s => s.Wallet)
                    .ThenInclude(w => w.Type)
                .Include(s => s.Activity)
                    .ThenInclude(a => a.Type)
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
                .ThenInclude(w => w.Type)
            .Include(s => s.Activity)
                .ThenInclude(a => a.Type)
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
}
