using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class ProductRepository : IProductRepository
{
    public Product Add(Product creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.Products.Add(creation).Entity;
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
        (List<string> categoryIds, List<string> levelIds, string propertySort,
        bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Product> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Products
                .Where(t => (EF.Functions.Like(t.ProductName, "%" + search + "%")
                || EF.Functions.Like(t.Category.CategoryName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (categoryIds.Count == 0 || categoryIds.Contains(t.CategoryId))
                && (levelIds.Count == 0 || levelIds.Contains(t.LevelId))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(s => s.Category)
               .Include(s => s.Level)
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
            using var db = new UnibeanDBContext();
            product = db.Products
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Category)
            .Include(s => s.Level)
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

    public Product Update(Product update)
    {
        try
        {
            using var db = new UnibeanDBContext();
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
