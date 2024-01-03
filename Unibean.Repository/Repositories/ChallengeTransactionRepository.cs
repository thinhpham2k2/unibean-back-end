using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class ChallengeTransactionRepository : IChallengeTransactionRepository
{
    public ChallengeTransaction Add(ChallengeTransaction creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.ChallengeTransactions.Add(creation).Entity;

            if (creation != null)
            {
                // Update wallet balance
                var wallet = db.Wallets.Where(w => w.Status.Equals(true) && w.Id.Equals(creation.WalletId))
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
            using var db = new UnibeanDBContext();
            result = db.ChallengeTransactions
                .Where(t => (EF.Functions.Like(t.Challenge.Challenge.ChallengeName, "%" + search + "%")
                || EF.Functions.Like(t.Wallet.Type.TypeName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (walletIds.Count == 0 || walletIds.Contains(t.WalletId))
                && (challengeIds.Count == 0 || challengeIds.Contains(t.ChallengeId))
                && t.Status.Equals(true))
                .Include(s => s.Wallet)
                    .ThenInclude(w => w.Type)
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
            using var db = new UnibeanDBContext();
            challengeTransaction = db.ChallengeTransactions
            .Where(s => s.Id.Equals(id) && s.Status.Equals(true))
            .Include(s => s.Wallet)
                .ThenInclude(w => w.Type)
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
