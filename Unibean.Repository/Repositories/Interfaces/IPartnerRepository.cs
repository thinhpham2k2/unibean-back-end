using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IPartnerRepository
{
    Partner GetByUserNameAndPassword(string userName, string password);
}
