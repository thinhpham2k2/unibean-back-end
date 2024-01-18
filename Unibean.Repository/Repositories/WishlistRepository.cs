using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class WishlistRepository : IWishlistRepository
{
    public Wishlist Add(Wishlist creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
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
            using var db = new UnibeanDBContext();
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
        (List<string> studentIds, List<string> brandIds, string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Wishlist> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Wishlists
                .Where(t => (EF.Functions.Like(t.Student.FullName, "%" + search + "%")
                || EF.Functions.Like(t.Brand.BrandName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (studentIds.Count == 0 || studentIds.Contains(t.StudentId))
                && (brandIds.Count == 0 || brandIds.Contains(t.BrandId))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(s => s.Student)
               .Include(s => s.Brand)
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
            using var db = new UnibeanDBContext();
            wishlist = db.Wishlists
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
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

    public Wishlist GetByStudentAndBrand(string studentId, string brandId)
    {
        Wishlist wishlist = new();
        try
        {
            using var db = new UnibeanDBContext();
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
            using var db = new UnibeanDBContext();
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
