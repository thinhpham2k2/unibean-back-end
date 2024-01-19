using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class RequestRepository : IRequestRepository
{
    public Request Add(Request creation)
    {
        try
        {
            using var db = new UnibeanDBContext();

            // Get green bean wallet brand
            var wallet = db.Brands
                    .Where(s => s.Id.Equals(creation.BrandId) && (bool)s.Status)
                    .Include(b => b.Wallets).FirstOrDefault().Wallets.FirstOrDefault();

            // Create request transactions
            creation.RequestTransactions = new List<RequestTransaction>() {
                new RequestTransaction
            {
                Id = Ulid.NewUlid().ToString(),
                RequestId = creation.Id,
                WalletId = wallet.Id,
                Amount = creation.Amount,
                Rate = 1,
                Description = creation.Description,
                State = creation.State,
                Status = creation.Status,
            }};

            creation = db.Requests.Add(creation).Entity;

            if (creation != null)
            {
                // Update wallet balance
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

    public void Delete(string id)
    {
        try
        {
            using var db = new UnibeanDBContext();
            var request = db.Requests.FirstOrDefault(b => b.Id.Equals(id));
            request.Status = false;
            db.Requests.Update(request);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Request> GetAll
        (List<string> brandIds, List<string> adminIds, string propertySort,
        bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Request> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Requests
                .Where(t => (EF.Functions.Like(t.Brand.BrandName, "%" + search + "%")
                || EF.Functions.Like(t.Brand.Acronym, "%" + search + "%")
                || EF.Functions.Like(t.Admin.FullName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (brandIds.Count == 0 || brandIds.Contains(t.BrandId))
                && (adminIds.Count == 0 || adminIds.Contains(t.AdminId))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(s => s.Brand)
               .Include(s => s.Admin)
               .Include(s => s.RequestTransactions.Where(r => (bool)r.Status))
                    .ThenInclude(r => r.Wallet)
               .ToList();

            pagedResult = new PagedResultModel<Request>
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

    public Request GetById(string id)
    {
        Request request = new();
        try
        {
            using var db = new UnibeanDBContext();
            request = db.Requests
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Brand)
                .ThenInclude(b => b.Account)
            .Include(s => s.Admin)
                .ThenInclude(a => a.Account)
            .Include(s => s.RequestTransactions.Where(r => (bool)r.Status))
                .ThenInclude(r => r.Wallet)
                    .ThenInclude(w => w.Type)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return request;
    }

    public Request Update(Request update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.Requests.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
