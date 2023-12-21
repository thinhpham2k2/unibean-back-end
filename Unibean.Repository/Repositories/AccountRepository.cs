using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Unibean.Repository.Repositories;

public class AccountRepository : IAccountRepository
{
    public Account Add(Account creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.Accounts.Add(creation).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return creation;
    }

    public bool CheckEmailDuplicate(string email)
    {
        Account account = new();
        try
        {
            using var db = new UnibeanDBContext();
            account = db.Accounts
                .Where(a => a.Email.Equals(email)).FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return account == null;
    }

    public bool CheckPhoneDuplicate(string phone)
    {
        Account account = new();
        try
        {
            using var db = new UnibeanDBContext();
            account = db.Accounts
                .Where(a => a.Phone.Equals(phone)).FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return account == null;
    }

    public bool CheckUsernameDuplicate(string userName)
    {
        Account account = new();
        try
        {
            using var db = new UnibeanDBContext();
            account = db.Accounts
                .Where(p => p.UserName.Equals(userName)).FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return account == null;
    }

    public Account GetByEmail(string email)
    {
        Account account = new();
        try
        {
            using var db = new UnibeanDBContext();
            account = db.Accounts.Where(a => a.Email.Equals(email)
            && a.Status.Equals(true))
                .Include(a => a.Role)
                .Include(a => a.Admins)
                .Include(a => a.Brands)
                .Include(a => a.Stores)
                .Include(a => a.Students)
                .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return account;
    }

    public Account GetById(string id)
    {
        Account account = new();
        try
        {
            using var db = new UnibeanDBContext();
            account = db.Accounts
            .Where(a => a.Id.Equals(id) && a.Status.Equals(true))
            .Include(a => a.Role)
            .Include(a => a.Admins)
            .Include(a => a.Brands)
            .Include(a => a.Stores)
            .Include(a => a.Students)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return account;
    }

    public Account GetByUserNameAndPassword(string userName, string password)
    {
        Account account = new();
        try
        {
            using (var db = new UnibeanDBContext())
            {
                account = db.Accounts.Where(a => a.UserName.Equals(userName)
                && a.Status.Equals(true))
                    .Include(a => a.Role)
                    .Include(a => a.Admins)
                    .Include(a => a.Brands)
                    .Include(a => a.Stores)
                    .Include(a => a.Students)
                    .FirstOrDefault();
            }
            if (account != null)
            {
                if (!BCryptNet.Verify(password, account.Password))
                {
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return account;
    }

    public Account Update(Account update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.Accounts.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
