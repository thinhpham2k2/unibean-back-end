using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class ActivityTransactionRepository : IActivityTransactionRepository
{
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
}
