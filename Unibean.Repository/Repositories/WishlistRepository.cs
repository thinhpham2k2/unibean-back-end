using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class WishlistRepository : IWishlistRepository
{
    private readonly UnibeanDBContext unibeanDB;

    public WishlistRepository(UnibeanDBContext unibeanDB)
    {
        this.unibeanDB = unibeanDB;
    }

    public Wishlist Add(Wishlist creation)
    {
        try
        {
            var db = unibeanDB;
            creation = db.Wishlists.Add(creation).Entity;
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
            var db = unibeanDB;
            var wishlist = db.Wishlists.FirstOrDefault(b => b.Id.Equals(id));
            wishlist.Status = false;
            db.Wishlists.Update(wishlist);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Wishlist> GetAll
        (List<string> studentIds, List<string> brandIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Wishlist> pagedResult = new();
        try
        {
            var db = unibeanDB;
            var query = db.Wishlists
                .Where(t => (EF.Functions.Like(t.Student.FullName, "%" + search + "%")
                || EF.Functions.Like(t.Brand.BrandName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (studentIds.Count == 0 || studentIds.Contains(t.StudentId))
                && (brandIds.Count == 0 || brandIds.Contains(t.BrandId))
                && (state == null || state.Equals(t.State))
                && (bool)t.Brand.Status
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(s => s.Student)
                    .ThenInclude(s => s.Account)
               .Include(s => s.Brand)
                    .ThenInclude(s => s.Account)
               .ToList();

            pagedResult = new PagedResultModel<Wishlist>
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

    public Wishlist GetById(string id)
    {
        Wishlist wishlist = new();
        try
        {
            var db = unibeanDB;
            wishlist = db.Wishlists
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Student)
                .ThenInclude(s => s.Account)
            .Include(s => s.Brand)
                .ThenInclude(s => s.Account)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return wishlist;
    }

    public Wishlist GetByStudentAndBrand(string studentId, string brandId)
    {
        Wishlist wishlist = new();
        try
        {
            var db = unibeanDB;
            wishlist = db.Wishlists
            .Where(s => s.StudentId.Equals(studentId)
            && s.BrandId.Equals(brandId))
            .Include(s => s.Student)
            .Include(s => s.Brand)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return wishlist;
    }

    public Wishlist Update(Wishlist update)
    {
        try
        {
            var db = unibeanDB;
            update = db.Wishlists.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
