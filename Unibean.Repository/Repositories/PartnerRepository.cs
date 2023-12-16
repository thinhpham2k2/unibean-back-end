using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Unibean.Repository.Repositories;

public class PartnerRepository : IPartnerRepository
{
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
}
