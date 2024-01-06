using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class RoleRepository : IRoleRepository
{
    public Role Add(Role creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.Roles.Add(creation).Entity;
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
            var role = db.Roles.FirstOrDefault(b => b.Id.Equals(id));
            role.Status = false;
            db.Roles.Update(role);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Role> GetAll(string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Role> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Roles
                .Where(r => (EF.Functions.Like(r.RoleName, "%" + search + "%")
                || EF.Functions.Like(r.FileName, "%" + search + "%")
                || EF.Functions.Like(r.Description, "%" + search + "%"))
                && (bool)r.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .ToList();

            pagedResult = new PagedResultModel<Role>
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

    public Role GetById(string id)
    {
        Role role = new();
        try
        {
            using var db = new UnibeanDBContext();
            role = db.Roles
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return role;
    }

    public Role GetRoleByName(string roleName)
    {
        Role role = new();
        try
        {
            using var db = new UnibeanDBContext();
            role = db.Roles
            .Where(s => s.RoleName.Equals(roleName) && (bool)s.Status)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return role;
    }

    public Role Update(Role update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.Roles.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
