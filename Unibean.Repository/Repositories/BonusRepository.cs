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
            creation = db.Bonuses.Add(creation).Entity;
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
        (List<string> brandIds, List<string> storeIds, List<string> studentIds, 
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
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(s => s.BonusTransactions.Where(b => (bool)b.Status))
                    .ThenInclude(a => a.Wallet)
                        .ThenInclude(w => w.Type)
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
                    .ThenInclude(w => w.Type)
            .Include(s => s.Brand)
            .Include(s => s.Store)
            .Include(s => s.Student)
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
                        .ThenInclude(w => w.Type)
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
