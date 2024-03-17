using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class CampaignTransactionRepository : ICampaignTransactionRepository
{
    private readonly UnibeanDBContext unibeanDB;

    public CampaignTransactionRepository(UnibeanDBContext unibeanDB)
    {
        this.unibeanDB = unibeanDB;
    }

    public CampaignTransaction Add(CampaignTransaction creation)
    {
        try
        {
            var db = unibeanDB;
            creation = db.CampaignTransactions.Add(creation).Entity;

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

    public List<CampaignTransaction> GetAll
        (List<string> walletIds, List<string> campaignIds,
        List<WalletType> walletTypeIds, string search)
    {
        List<CampaignTransaction> result;
        try
        {
            var db = unibeanDB;
            result = db.CampaignTransactions
                .Where(t => (EF.Functions.Like(t.Campaign.CampaignName, "%" + search + "%")
                || EF.Functions.Like(t.Wallet.Brand.BrandName, "%" + search + "%")
                || EF.Functions.Like(t.Amount > 0 ? "Hoàn trả đậu" : "Tạo chiến dịch", "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (walletIds.Count == 0 || walletIds.Contains(t.WalletId))
                && (campaignIds.Count == 0 || campaignIds.Contains(t.CampaignId))
                && (walletTypeIds.Count == 0 || walletTypeIds.Contains(t.Wallet.Type.Value))
                && (bool)t.Status)
                .Include(s => s.Campaign)
                    .ThenInclude(c => c.Type)
                .Include(s => s.Wallet)
                    .ThenInclude(c => c.Brand).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }

    public CampaignTransaction GetById(string id)
    {
        CampaignTransaction walletTransaction = new();
        try
        {
            var db = unibeanDB;
            walletTransaction = db.CampaignTransactions
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Campaign)
                .ThenInclude(c => c.Type)
            .Include(s => s.Wallet)
                .ThenInclude(c => c.Brand)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return walletTransaction;
    }

    public decimal IncomeOfGreenBean(string brandId, DateOnly date)
    {
        decimal result = 0;
        try
        {
            var db = unibeanDB;
            result = db.CampaignTransactions
                .Where(o => o.Wallet.Type.Equals(WalletType.Green)
                && o.Wallet.BrandId.Equals(brandId) && o.Amount > 0
                && DateOnly.FromDateTime(o.DateCreated.Value).Equals(date)
                && (bool)o.Status).Select(o => o.Amount.Value).Sum();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }

    public decimal OutcomeOfGreenBean(string brandId, DateOnly date)
    {
        decimal result = 0;
        try
        {
            var db = unibeanDB;
            result = -db.CampaignTransactions
                .Where(o => o.Wallet.Type.Equals(WalletType.Green)
                && o.Wallet.BrandId.Equals(brandId) && o.Amount < 0
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
