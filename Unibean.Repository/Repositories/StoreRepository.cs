using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Unibean.Repository.Repositories;

public class StoreRepository : IStoreRepository
{
    public Store GetByUserNameAndPassword(string userName, string password)
    {
        Store store = new Store();
        try
        {
            using (var db = new UnibeanDBContext())
            {
                store = db.Stores.Where(s => s.UserName.Equals(userName)
                && s.Status.Equals(true))
                    .Include(s => s.Partner)
                    .Include(s => s.Area).FirstOrDefault();
            }
            if (store != null)
            {
                if (!BCryptNet.Verify(password, store.Password))
                {
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return store;
    }
}
