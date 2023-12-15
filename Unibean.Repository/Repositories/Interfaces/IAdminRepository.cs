using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IAdminRepository
{
    Admin GetByUserNameAndPassword(string userName, string password);
}
