using Unibean.Service.Models.Accounts;

namespace Unibean.Service.Services.Interfaces;

public interface IAccountService
{
    AccountModel GetByUserNameAndPassword(string userName, string password);
}
