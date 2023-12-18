using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class CampaignTypeRepository : ICampaignTypeRepository
{
    public CampaignType Add(CampaignType creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.CampaignTypes.Add(creation).Entity;
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
            var type = db.CampaignTypes.FirstOrDefault(b => b.Id.Equals(id));
            type.Status = false;
            db.CampaignTypes.Update(type);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<CampaignType> GetAll(string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<CampaignType> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.CampaignTypes
                .Where(t => (EF.Functions.Like(t.TypeName, "%" + search + "%")
                || EF.Functions.Like(t.FileName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && t.Status.Equals(true))
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .ToList();

            pagedResult = new PagedResultModel<CampaignType>
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

    public CampaignType GetById(string id)
    {
        CampaignType type = new();
        try
        {
            using var db = new UnibeanDBContext();
            type = db.CampaignTypes
            .Where(s => s.Id.Equals(id) && s.Status.Equals(true))
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return type;
    }

    public CampaignType Update(CampaignType update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.CampaignTypes.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
