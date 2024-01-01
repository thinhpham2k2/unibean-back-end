using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
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

    public PagedResultModel<ChallengeTransaction> GetAll
        (List<string> walletIds, List<string> challengeIds, string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<ChallengeTransaction> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.ChallengeTransactions
                .Where(t => (EF.Functions.Like(t.Description, "%" + search + "%"))
                && (walletIds.Count == 0 || walletIds.Contains(t.WalletId))
                && (challengeIds.Count == 0 || challengeIds.Contains(t.ChallengeId))
                && t.Status.Equals(true))
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(s => s.Wallet)
               .Include(s => s.Challenge)
               .ToList();

            pagedResult = new PagedResultModel<ChallengeTransaction>
            {
                CurrentPage = page,
                PageSize = limit,
                PageCount = (int)Math.Ceiling((double)query.Count() / limit),
                Result = result,
                RowCount = result.Count,
                TotalCount = query.Count()
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return pagedResult;
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
            .Include(s => s.Challenge)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return challengeTransaction;
    }
}
