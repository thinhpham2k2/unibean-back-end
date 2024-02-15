using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class BonusRepository : IBonusRepository
{
    public Bonus Add(Bonus creation)
    {
        try
        {
            using var db = new UnibeanDBContext();

            // Get green bean wallet student
            var student = db.Students
                    .Where(s => s.Id.Equals(creation.StudentId) && (bool)s.Status)
                    .Include(b => b.Wallets).FirstOrDefault();
            var studentWallet = student.Wallets.Where(w => w.Type.Equals(WalletType.Green)).FirstOrDefault();

            // Get green bean wallet brand
            var brand = db.Brands
                    .Where(s => s.Id.Equals(creation.BrandId) && (bool)s.Status)
                    .Include(b => b.Wallets).FirstOrDefault();
            var brandWallet = brand.Wallets.Where(w => w.Type.Equals(WalletType.Green)).FirstOrDefault();

            creation.BonusTransactions = new List<BonusTransaction>() {
                new BonusTransaction
            {
                Id = Ulid.NewUlid().ToString(),
                BonusId = creation.Id,
                WalletId = studentWallet.Id,
                Amount = creation.Amount,
                Rate = 1,
                Description = creation.Description,
                State = creation.State,
                Status = creation.Status,
            },
                new BonusTransaction
            {
                Id = Ulid.NewUlid().ToString(),
                BonusId = creation.Id,
                WalletId = brandWallet.Id,
                Amount = -creation.Amount,
                Rate = 1,
                Description = creation.Description,
                State = creation.State,
                Status = creation.Status,
            }};

            creation = db.Bonuses.Add(creation).Entity;

            if (creation != null)
            {
                // Update student wallet balance
                student.TotalIncome += creation.Amount;
                studentWallet.Balance += creation.Amount;
                studentWallet.DateUpdated = DateTime.Now;

                // Update brand wallet balance
                brand.TotalSpending += creation.Amount;
                brandWallet.Balance -= creation.Amount;
                brandWallet.DateUpdated = DateTime.Now;

                db.Students.Update(student);
                db.Brands.Update(brand);
                db.Wallets.UpdateRange(studentWallet, brandWallet);
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
            var bonus = db.Bonuses.FirstOrDefault(b => b.Id.Equals(id));
            bonus.Status = false;
            db.Bonuses.Update(bonus);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Bonus> GetAll
        (List<string> brandIds, List<string> storeIds, List<string> studentIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Bonus> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Bonuses
                .Where(t => (EF.Functions.Like(t.Brand.BrandName, "%" + search + "%")
                || EF.Functions.Like(t.Store.StoreName, "%" + search + "%")
                || EF.Functions.Like(t.Store.Address, "%" + search + "%")
                || EF.Functions.Like(t.Student.FullName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (brandIds.Count == 0 || brandIds.Contains(t.BrandId))
                && (storeIds.Count == 0 || storeIds.Contains(t.StoreId))
                && (studentIds.Count == 0 || studentIds.Contains(t.StudentId))
                && (state == null || state.Equals(t.State))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(s => s.BonusTransactions.Where(b => (bool)b.Status))
                    .ThenInclude(a => a.Wallet)
               .Include(s => s.Brand)
               .Include(s => s.Store)
               .Include(s => s.Student)
               .ToList();

            pagedResult = new PagedResultModel<Bonus>
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

    public Bonus GetById(string id)
    {
        Bonus bonus = new();
        try
        {
            using var db = new UnibeanDBContext();
            bonus = db.Bonuses
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.BonusTransactions.Where(b => (bool)b.Status))
                .ThenInclude(a => a.Wallet)
            .Include(s => s.Brand)
                .ThenInclude(b => b.Account)
            .Include(s => s.Store)
                .ThenInclude(s => s.Account)
            .Include(s => s.Student)
                .ThenInclude(s => s.Account)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return bonus;
    }

    public List<Bonus> GetList
        (List<string> brandIds, List<string> storeIds, List<string> studentIds, string search)
    {
        List<Bonus> result;
        try
        {
            using var db = new UnibeanDBContext();
            result = db.Bonuses
                .Where(t => (EF.Functions.Like(t.Brand.BrandName, "%" + search + "%")
                || EF.Functions.Like(t.Store.StoreName, "%" + search + "%")
                || EF.Functions.Like(t.Store.Address, "%" + search + "%")
                || EF.Functions.Like(t.Student.FullName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (brandIds.Count == 0 || brandIds.Contains(t.BrandId))
                && (storeIds.Count == 0 || storeIds.Contains(t.StoreId))
                && (studentIds.Count == 0 || studentIds.Contains(t.StudentId))
                && (bool)t.Status)
               .Include(s => s.BonusTransactions.Where(b => (bool)b.Status && b.Amount > 0))
                    .ThenInclude(a => a.Wallet)
               .Include(s => s.Brand)
               .Include(s => s.Store)
               .Include(s => s.Student).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }

    public Bonus Update(Bonus update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.Bonuses.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
