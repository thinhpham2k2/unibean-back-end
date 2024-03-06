using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class BonusTransactionRepository : IBonusTransactionRepository
{
    public List<BonusTransaction> GetAll
        (List<string> walletIds, List<string> bonusIds, List<WalletType> walletTypeIds, string search)
    {
        List<BonusTransaction> result;
        try
        {
            using var db = new UnibeanDBContext();
            result = db.BonusTransactions
                .Where(a => (EF.Functions.Like(a.Bonus.Brand.BrandName, "%" + search + "%")
                || EF.Functions.Like(a.Bonus.Store.StoreName, "%" + search + "%")
                || EF.Functions.Like(a.Bonus.Student.FullName, "%" + search + "%")
                || EF.Functions.Like(a.Description, "%" + search + "%"))
                && (walletIds.Count == 0 || walletIds.Contains(a.WalletId))
                && (bonusIds.Count == 0 || bonusIds.Contains(a.BonusId))
                && (walletTypeIds.Count == 0 || walletTypeIds.Contains(a.Wallet.Type.Value))
                && (bool)a.Status)
                .Include(s => s.Wallet)
                .Include(s => s.Bonus)
                    .ThenInclude(a => a.Brand)
                .Include(s => s.Bonus)
                    .ThenInclude(a => a.Student)
                .Include(s => s.Bonus)
                    .ThenInclude(a => a.Store).ToList();
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
            result = db.Bonuses
                .Where(o => o.StoreId.Equals(storeId)
                && DateOnly.FromDateTime(o.DateCreated.Value).Equals(date)
                && (bool)o.Status).Select(o => o.Amount.Value).Sum();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }
}
