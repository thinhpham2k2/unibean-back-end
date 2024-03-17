using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class RequestTransactionRepository : IRequestTransactionRepository
{
    private readonly UnibeanDBContext unibeanDB;

    public RequestTransactionRepository(UnibeanDBContext unibeanDB)
    {
        this.unibeanDB = unibeanDB;
    }

    public RequestTransaction Add(RequestTransaction creation)
    {
        try
        {
            var db = unibeanDB;
            creation = db.RequestTransactions.Add(creation).Entity;

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

    public List<RequestTransaction> GetAll
        (List<string> walletIds, List<string> requestIds, List<WalletType> walletTypeIds, string search)
    {
        List<RequestTransaction> result;
        try
        {
            var db = unibeanDB;
            result = db.RequestTransactions
                .Where(t => (EF.Functions.Like(t.Request.Admin.FullName, "%" + search + "%")
                || EF.Functions.Like((string)(object)t.Wallet.Type, "%" + search + "%")
                || EF.Functions.Like(t.Wallet.Brand.BrandName, "%" + search + "%")
                || EF.Functions.Like("Nạp đậu", "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (walletIds.Count == 0 || walletIds.Contains(t.WalletId))
                && (requestIds.Count == 0 || requestIds.Contains(t.RequestId))
                && (walletTypeIds.Count == 0 || walletTypeIds.Contains(t.Wallet.Type.Value))
                && (bool)t.Status)
                .Include(s => s.Request)
                    .ThenInclude(w => w.Admin)
                .Include(s => s.Wallet)
                    .ThenInclude(c => c.Brand).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }

    public RequestTransaction GetById(string id)
    {
        RequestTransaction requestTransaction = new();
        try
        {
            var db = unibeanDB;
            requestTransaction = db.RequestTransactions
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Request)
                .ThenInclude(w => w.Admin)
            .Include(s => s.Wallet)
                .ThenInclude(c => c.Brand)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return requestTransaction;
    }

    public decimal IncomeOfGreenBean(string brandId, DateOnly date)
    {
        decimal result = 0;
        try
        {
            var db = unibeanDB;
            result = db.RequestTransactions
                .Where(o => o.Wallet.Type.Equals(WalletType.Green)
                && o.Request.BrandId.Equals(brandId)
                && DateOnly.FromDateTime(o.Request.DateCreated.Value).Equals(date)
                && (bool)o.Status).Select(o => o.Amount.Value).Sum();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }
}
