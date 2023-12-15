using Unibean.Service.Models.Admins;

namespace Unibean.Service.Services.Interfaces;

public interface IAdminService
{
    AdminModel GetByUserNameAndPassword(string userName, string password);
}
