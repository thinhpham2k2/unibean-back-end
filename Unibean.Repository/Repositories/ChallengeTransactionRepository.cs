﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class ChallengeTransactionRepository : IChallengeTransactionRepository
{
    private readonly UnibeanDBContext unibeanDB;

    public ChallengeTransactionRepository(UnibeanDBContext unibeanDB)
    {
        this.unibeanDB = unibeanDB;
    }

    public ChallengeTransaction Add(ChallengeTransaction creation)
    {
        try
        {
            var db = unibeanDB;
            creation = db.ChallengeTransactions.Add(creation).Entity;

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

    public List<ChallengeTransaction> GetAll
        (List<string> walletIds, List<string> challengeIds, string search)
    {
        List<ChallengeTransaction> result;
        try
        {
            var db = unibeanDB;
            result = db.ChallengeTransactions
                .Where(t => (EF.Functions.Like(t.Challenge.Challenge.ChallengeName, "%" + search + "%")
                || EF.Functions.Like("Thử thách", "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (walletIds.Count == 0 || walletIds.Contains(t.WalletId))
                && (challengeIds.Count == 0 || challengeIds.Contains(t.ChallengeId))
                && (bool)t.Status)
                .Include(s => s.Wallet)
                .Include(s => s.Challenge)
                    .ThenInclude(c => c.Challenge).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }

    public ChallengeTransaction GetById(string id)
    {
        ChallengeTransaction challengeTransaction = new();
        try
        {
            var db = unibeanDB;
            challengeTransaction = db.ChallengeTransactions
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Wallet)
            .Include(s => s.Challenge)
                .ThenInclude(c => c.Challenge)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return challengeTransaction;
    }
}
