using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class WalletTypeRepository : IWalletTypeRepository
{
    public WalletType Add(WalletType creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.WalletTypes.Add(creation).Entity;
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
            var walletType = db.WalletTypes.FirstOrDefault(b => b.Id.Equals(id));
            walletType.Status = false;
            db.WalletTypes.Update(walletType);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<WalletType> GetAll(string propertySort, bool isAsc, string search, int page, int limit)
    {

        PagedResultModel<WalletType> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.WalletTypes
                .Where(t => (EF.Functions.Like(t.TypeName, "%" + search + "%")
                || EF.Functions.Like(t.FileName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && t.Status.Equals(true))
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .ToList();

            pagedResult = new PagedResultModel<WalletType>
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

    public WalletType GetById(string id)
    {
        WalletType walletType = new();
        try
        {
            using var db = new UnibeanDBContext();
            walletType = db.WalletTypes
            .Where(s => s.Id.Equals(id) && s.Status.Equals(true))
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return walletType;
    }

    public WalletType GetFirst()
    {
        WalletType walletType = new();
        try
        {
            using var db = new UnibeanDBContext();
            walletType = db.WalletTypes
            .Where(s => s.Status.Equals(true))
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return walletType;
    }

    public WalletType GetSecond()
    {
        WalletType walletType = new();
        try
        {
            using var db = new UnibeanDBContext();
            walletType = db.WalletTypes
            .Where(s => s.Status.Equals(true))
            .Skip(1)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return walletType;
    }

    public WalletType Update(WalletType update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.WalletTypes.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
