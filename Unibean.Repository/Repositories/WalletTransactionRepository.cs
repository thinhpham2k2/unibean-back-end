﻿using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class WalletTransactionRepository : IWalletTransactionRepository
{
    public WalletTransaction Add(WalletTransaction creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.WalletTransactions.Add(creation).Entity;

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

    public List<WalletTransaction> GetAll
        (List<string> walletIds, List<string> campaignIds, List<string> walletTypeIds, string search)
    {
        List<WalletTransaction> result;
        try
        {
            using var db = new UnibeanDBContext();
            result = db.WalletTransactions
                .Where(t => (EF.Functions.Like(t.Campaign.CampaignName, "%" + search + "%")
                || EF.Functions.Like(t.Wallet.Brand.BrandName, "%" + search + "%")
                || EF.Functions.Like(t.Amount > 0 ? "Hoàn trả đậu" : "Tạo chiến dịch", "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (walletIds.Count == 0 || walletIds.Contains(t.WalletId))
                && (campaignIds.Count == 0 || campaignIds.Contains(t.CampaignId))
                && (walletTypeIds.Count == 0 || walletTypeIds.Contains(t.Wallet.TypeId))
                && (bool)t.Status)
                .Include(s => s.Wallet)
                    .ThenInclude(w => w.Type)
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

    public WalletTransaction GetById(string id)
    {
        WalletTransaction walletTransaction = new();
        try
        {
            using var db = new UnibeanDBContext();
            walletTransaction = db.WalletTransactions
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Wallet)
                .ThenInclude(w => w.Type)
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
}
