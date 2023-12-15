using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Unibean.Repository.Repositories;

public class AdminRepository : IAdminRepository
{
    public Admin GetByUserNameAndPassword(string userName, string password)
    {
        Admin admin = new Admin();
        try
        {
            using (var db = new UnibeanDBContext())
            {
                admin = db.Admins.Where(a => a.UserName.Equals(userName)
                && a.Status.Equals(true)).FirstOrDefault();
            }
            if (admin != null)
            {
                if (!BCryptNet.Verify(password, admin.Password))
                {
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return admin;
    }
}
