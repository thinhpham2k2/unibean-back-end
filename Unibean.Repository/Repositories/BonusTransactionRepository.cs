using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class BonusTransactionRepository : IBonusTransactionRepository
{
    public BonusTransaction Add(BonusTransaction creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.BonusTransactions.Add(creation).Entity;

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

    public List<BonusTransaction> GetAll
        (List<string> walletIds, List<string> bonusIds, string search)
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
                && (bool)a.Status)
                .Include(s => s.Wallet)
                    .ThenInclude(w => w.Type)
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

    public BonusTransaction GetById(string id)
    {
        BonusTransaction bonusTransaction = new();
        try
        {
            using var db = new UnibeanDBContext();
            bonusTransaction = db.BonusTransactions
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Wallet)
                .ThenInclude(w => w.Type)
            .Include(s => s.Bonus)
                .ThenInclude(a => a.Brand)
            .Include(s => s.Bonus)
                .ThenInclude(a => a.Student)
            .Include(s => s.Bonus)
                .ThenInclude(a => a.Store)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return bonusTransaction;
    }
}
