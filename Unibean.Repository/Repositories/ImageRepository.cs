using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly UnibeanDBContext unibeanDB;

    public ImageRepository(UnibeanDBContext unibeanDB)
    {
        this.unibeanDB = unibeanDB;
    }

    public Image Add(Image creation)
    {
        try
        {
            var db = unibeanDB;
            creation = db.Images.Add(creation).Entity;
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
            var image = db.Images.FirstOrDefault(b => b.Id.Equals(id));
            image.Status = false;
            db.Images.Update(image);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Image> GetAll
        (List<string> productIds, bool? state, string propertySort,
        bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Image> pagedResult = new();
        try
        {
            var db = unibeanDB;
            var query = db.Images
                .Where(t => (EF.Functions.Like(t.Product.ProductName, "%" + search + "%")
                || EF.Functions.Like(t.FileName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (productIds.Count == 0 || productIds.Contains(t.ProductId))
                && (state == null || state.Equals(t.State))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(s => s.Product)
                    .ThenInclude(p => p.Category)
               .ToList();

            pagedResult = new PagedResultModel<Image>
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

    public Image GetById(string id)
    {
        Image image = new();
        try
        {
            var db = unibeanDB;
            image = db.Images
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Product)
                .ThenInclude(p => p.Category)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return image;
    }

    public Image Update(Image update)
    {
        try
        {
            var db = unibeanDB;
            update = db.Images.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
