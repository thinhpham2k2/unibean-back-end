using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly UnibeanDBContext unibeanDB;

    public ProductRepository(UnibeanDBContext unibeanDB)
    {
        this.unibeanDB = unibeanDB;
    }

    public Product Add(Product creation)
    {
        try
        {
            var db = unibeanDB;
            creation = db.Products.Add(creation).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return creation;
    }

    public long CountProduct()
    {
        long count = 0;
        try
        {
            var db = unibeanDB;
            count = db.Products.Where(c => (bool)c.Status).Count();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return count;
    }

    public void Delete(string id)
    {
        try
        {
            var db = unibeanDB;
            var product = db.Products.FirstOrDefault(b => b.Id.Equals(id));
            product.Status = false;
            db.Products.Update(product);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Product> GetAll
        (List<string> categoryIds, bool? state, string propertySort,
        bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Product> pagedResult = new();
        try
        {
            var db = unibeanDB;
            var query = db.Products
                .Where(t => (EF.Functions.Like(t.ProductName, "%" + search + "%")
                || EF.Functions.Like(t.Category.CategoryName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (categoryIds.Count == 0 || categoryIds.Contains(t.CategoryId))
                && (state == null || state.Equals(t.State))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(s => s.Category)
               .Include(s => s.Images.Where(i => (bool)i.Status))
               .ToList();

            pagedResult = new PagedResultModel<Product>
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

    public Product GetById(string id)
    {
        Product product = new();
        try
        {
            var db = unibeanDB;
            product = db.Products
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Category)
            .Include(s => s.Images.Where(i => (bool)i.Status))
            .Include(s => s.OrderDetails.Where(d => (bool)d.Status))
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return product;
    }

    public List<ProductRanking> GetRankingByStation(string stationId, int limit)
    {
        List<ProductRanking> result = new();
        try
        {
            var db = unibeanDB;
            result.AddRange(db.Products.Where(
                s => s.OrderDetails.Any(a => (bool)a.Status
                & a.Order.StationId.Equals(stationId))
                & (bool)s.Status)
                .Include(s => s.Images)
                .Include(s => s.OrderDetails.Where(a => (bool)a.Status
                    & a.Order.StationId.Equals(stationId)))
                .ToList()
                .Select((s, index) => new ProductRanking()
                {
                    Name = s.ProductName,
                    Image = s.Images.FirstOrDefault(i => (bool)i.IsCover & (bool)i.Status).Url,
                    Total = s.OrderDetails.Select(a => a.Quantity).Sum(),
                }).OrderByDescending(
                a => a.Total).Take(limit));
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }

    public Product Update(Product update)
    {
        try
        {
            var db = unibeanDB;
            update = db.Products.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
