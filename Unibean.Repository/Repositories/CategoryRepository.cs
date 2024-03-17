using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly UnibeanDBContext unibeanDB;

    public CategoryRepository(UnibeanDBContext unibeanDB)
    {
        this.unibeanDB = unibeanDB;
    }

    public Category Add(Category creation)
    {
        try
        {
            var db = unibeanDB;
            creation = db.Categories.Add(creation).Entity;
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
            var category = db.Categories.FirstOrDefault(b => b.Id.Equals(id));
            category.Status = false;
            db.Categories.Update(category);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Category> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Category> pagedResult = new();
        try
        {
            var db = unibeanDB;
            var query = db.Categories
                .Where(t => (EF.Functions.Like(t.CategoryName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (state == null || state.Equals(t.State))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .ToList();

            pagedResult = new PagedResultModel<Category>
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

    public Category GetById(string id)
    {
        Category category = new();
        try
        {
            var db = unibeanDB;
            category = db.Categories
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(c => c.Products.Where(p => (bool)p.Status))
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return category;
    }

    public Category Update(Category update)
    {
        try
        {
            var db = unibeanDB;
            update = db.Categories.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
