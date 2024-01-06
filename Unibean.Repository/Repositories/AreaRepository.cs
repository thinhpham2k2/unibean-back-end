﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class AreaRepository : IAreaRepository
{
    public Area Add(Area creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.Areas.Add(creation).Entity;
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
            var area = db.Areas.FirstOrDefault(b => b.Id.Equals(id));
            area.Status = false;
            db.Areas.Update(area);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Area> GetAll(List<string> districtIds, string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Area> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Areas
                .Where(t => (EF.Functions.Like(t.AreaName, "%" + search + "%")
                || EF.Functions.Like(t.FileName, "%" + search + "%")
                || EF.Functions.Like(t.District.DistrictName, "%" + search + "%")
                || EF.Functions.Like(t.District.City.CityName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (districtIds.Count == 0 || districtIds.Contains(t.DistrictId))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(a => a.District)
               .ThenInclude(d => d.City)
               .ToList();

            pagedResult = new PagedResultModel<Area>
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

    public Area GetById(string id)
    {
        Area area = new();
        try
        {
            using var db = new UnibeanDBContext();
            area = db.Areas
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(a => a.District)
            .ThenInclude(d => d.City)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return area;
    }

    public Area Update(Area update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.Areas.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
