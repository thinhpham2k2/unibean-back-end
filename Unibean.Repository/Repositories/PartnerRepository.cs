using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Unibean.Repository.Repositories;

public class PartnerRepository : IPartnerRepository
{
    public Partner Add(Partner creation)
    {
        try
        {
            using (var db = new UnibeanDBContext())
            {
                creation = db.Partners.Add(creation).Entity;
                db.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return creation;
    }

    public bool CheckEmailDuplicate(string email)
    {
        Partner partner = new Partner();
        try
        {
            using (var db = new UnibeanDBContext())
            {
                partner = db.Partners
                    .Where(p => p.Email.Equals(email)).FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return partner == null;
    }

    public bool CheckUsernameDuplicate(string userName)
    {
        Partner partner = new Partner();
        try
        {
            using (var db = new UnibeanDBContext())
            {
                partner = db.Partners
                    .Where(p => p.UserName.Equals(userName)).FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return partner == null;
    }

    public void Delete(string id)
    {
        try
        {
            using (var db = new UnibeanDBContext())
            {
                var partner = db.Partners.FirstOrDefault(b => b.Id.Equals(id));
                partner.Status = false;
                db.Partners.Update(partner);
                db.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Partner> GetAll(string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Partner> pagedResult = new PagedResultModel<Partner>();
        try
        {
            using (var db = new UnibeanDBContext())
            {
                var query = db.Partners
                    .Where(p => (EF.Functions.Like(p.Id, "%" + search + "%")
                    || EF.Functions.Like(p.BrandName, "%" + search + "%")
                    || EF.Functions.Like(p.Acronym, "%" + search + "%")
                    || EF.Functions.Like(p.UserName, "%" + search + "%")
                    || EF.Functions.Like(p.Address, "%" + search + "%")
                    || EF.Functions.Like(p.LogoFileName, "%" + search + "%")
                    || EF.Functions.Like(p.CoverFileName, "%" + search + "%")
                    || EF.Functions.Like(p.Email, "%" + search + "%")
                    || EF.Functions.Like(p.Phone, "%" + search + "%")
                    || EF.Functions.Like(p.Description, "%" + search + "%"))
                    && p.Status.Equals(true))
                    .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

                var result = query
                   .Skip((page - 1) * limit)
                   .Take(limit)
                   .ToList();

                pagedResult = new PagedResultModel<Partner>
                {
                    CurrentPage = page,
                    PageSize = limit,
                    PageCount = (int)Math.Ceiling((double)query.Count() / limit),
                    Result = result,
                    RowCount = result.Count(),
                    TotalCount = query.Count()
                };
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return pagedResult;
    }

    public Partner GetById(string id)
    {
        Partner partner = new Partner();
        try
        {
            using (var db = new UnibeanDBContext())
            {
                partner = db.Partners
                .Where(s => s.Id.Equals(id) && s.Status.Equals(true))
                .Include(s => s.Wishlists.Where(w => w.Status.Equals(true)))
                .Include(s => s.Campaigns.Where(c => c.Status.Equals(true)))
                .ThenInclude(c => c.Type)
                .Include(s => s.Stores.Where(s => s.Status.Equals(true)))
                .ThenInclude(s => s.Area)
                .FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return partner;
    }

    public Partner GetByUserNameAndPassword(string userName, string password)
    {
        Partner partner = new Partner();
        try
        {
            using (var db = new UnibeanDBContext())
            {
                partner = db.Partners.Where(a => a.UserName.Equals(userName)
                && a.Status.Equals(true)).FirstOrDefault();
            }
            if (partner != null)
            {
                if (!BCryptNet.Verify(password, partner.Password))
                {
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return partner;
    }

    public Partner Update(Partner update)
    {
        try
        {
            using (var db = new UnibeanDBContext())
            {
                update = db.Partners.Update(update).Entity;
                db.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
