using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class TransactionRepository : ITransactionRepository
{
    public List<object> GetAll
        (List<string> walletIds, List<TransactionType> typeIds, string search, Role role)
    {
        List<object> result = new();
        try
        {
            using var db = new UnibeanDBContext();

            switch (role)
            {
                case Role.Brand:
                    // Bonus transactions
                    result.AddRange(db.BonusTransactions
                        .Where(a => (EF.Functions.Like(a.Bonus.Brand.BrandName, "%" + search + "%")
                        || EF.Functions.Like(a.Bonus.Store.StoreName, "%" + search + "%")
                        || EF.Functions.Like(a.Bonus.Student.FullName, "%" + search + "%")
                        || EF.Functions.Like(a.Description, "%" + search + "%"))
                        && (walletIds.Count == 0 || walletIds.Contains(a.WalletId))
                        && (bool)a.Status)
                        .Include(s => s.Wallet)
                        .Include(s => s.Bonus)
                            .ThenInclude(a => a.Brand)
                        .Include(s => s.Bonus)
                            .ThenInclude(a => a.Student)
                        .Include(s => s.Bonus)
                            .ThenInclude(a => a.Store).ToList());

                    // Campaign transactions
                    result.AddRange(db.CampaignTransactions
                        .Where(t => (EF.Functions.Like(t.Campaign.CampaignName, "%" + search + "%")
                        || EF.Functions.Like(t.Wallet.Brand.BrandName, "%" + search + "%")
                        || EF.Functions.Like(t.Amount > 0 ? "Hoàn trả đậu" : "Tạo chiến dịch", "%" + search + "%")
                        || EF.Functions.Like(t.Description, "%" + search + "%"))
                        && (walletIds.Count == 0 || walletIds.Contains(t.WalletId))
                        && (bool)t.Status)
                        .Include(s => s.Campaign)
                            .ThenInclude(c => c.Type)
                        .Include(s => s.Wallet)
                            .ThenInclude(c => c.Brand).ToList());

                    // Request transactions
                    result.AddRange(db.RequestTransactions
                        .Where(t => (EF.Functions.Like(t.Request.Admin.FullName, "%" + search + "%")
                        || EF.Functions.Like((string)(object)t.Wallet.Type, "%" + search + "%")
                        || EF.Functions.Like(t.Wallet.Brand.BrandName, "%" + search + "%")
                        || EF.Functions.Like("Nạp đậu", "%" + search + "%")
                        || EF.Functions.Like(t.Description, "%" + search + "%"))
                        && (walletIds.Count == 0 || walletIds.Contains(t.WalletId))
                        && (bool)t.Status)
                        .Include(s => s.Request)
                            .ThenInclude(w => w.Admin)
                        .Include(s => s.Wallet)
                            .ThenInclude(c => c.Brand).ToList());
                    break;
                case Role.Student:

                    if (typeIds.Contains(TransactionType.ChallengeTransaction) || typeIds.Count == 0)
                    {
                        // Challenge transactions
                        result.AddRange(db.ChallengeTransactions
                            .Where(t => (EF.Functions.Like(t.Challenge.Challenge.ChallengeName, "%" + search + "%")
                            || EF.Functions.Like((string)(object)t.Wallet.Type, "%" + search + "%")
                            || EF.Functions.Like("Thử thách", "%" + search + "%")
                            || EF.Functions.Like(t.Description, "%" + search + "%"))
                            && (walletIds.Count == 0 || walletIds.Contains(t.WalletId))
                            && (bool)t.Status)
                            .Include(s => s.Wallet)
                            .Include(s => s.Challenge)
                                .ThenInclude(c => c.Challenge).ToList());
                    }

                    if (typeIds.Contains(TransactionType.OrderTransaction) || typeIds.Count == 0)
                    {
                        // Order transactions
                        result.AddRange(db.OrderTransactions
                        .Where(o => (EF.Functions.Like("Tạo đơn hàng (" + o.Amount + " đậu)", "%" + search + "%")
                        || EF.Functions.Like((string)(object)o.Wallet.Type, "%" + search + "%")
                        || EF.Functions.Like("Đổi quà", "%" + search + "%")
                        || EF.Functions.Like(o.Description, "%" + search + "%"))
                        && (walletIds.Count == 0 || walletIds.Contains(o.WalletId))
                        && (bool)o.Status)
                        .Include(s => s.Wallet)
                        .Include(s => s.Order).ToList());
                    }

                    if (typeIds.Contains(TransactionType.BonusTransaction) || typeIds.Count == 0)
                    {
                        // Bonus transactions
                        result.AddRange(db.BonusTransactions
                        .Where(a => (EF.Functions.Like(a.Bonus.Brand.BrandName, "%" + search + "%")
                        || EF.Functions.Like(a.Bonus.Store.StoreName, "%" + search + "%")
                        || EF.Functions.Like(a.Bonus.Student.FullName, "%" + search + "%")
                        || EF.Functions.Like(a.Description, "%" + search + "%"))
                        && (walletIds.Count == 0 || walletIds.Contains(a.WalletId))
                        && (bool)a.Status)
                        .Include(s => s.Wallet)
                        .Include(s => s.Bonus)
                            .ThenInclude(a => a.Brand)
                        .Include(s => s.Bonus)
                            .ThenInclude(a => a.Student)
                        .Include(s => s.Bonus)
                            .ThenInclude(a => a.Store).ToList());
                    }

                    if (typeIds.Contains(TransactionType.ActivityTransaction) || typeIds.Count == 0)
                    {
                        // Activity transactions
                        result.AddRange(db.ActivityTransactions
                        .Where(a => (EF.Functions.Like(a.Activity.VoucherItem.Voucher.VoucherName, "%" + search + "%")
                        || EF.Functions.Like((string)(object)a.Wallet.Type, "%" + search + "%")
                        || EF.Functions.Like((string)(object)a.Activity.Type, "%" + search + "%")
                        || EF.Functions.Like(a.Description, "%" + search + "%"))
                        && (walletIds.Count == 0 || walletIds.Contains(a.WalletId))
                        && (bool)a.Status)
                        .Include(s => s.Wallet)
                        .Include(s => s.Activity)
                        .Include(s => s.Activity)
                            .ThenInclude(a => a.VoucherItem)
                                .ThenInclude(v => v.Voucher).ToList());
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }
}
