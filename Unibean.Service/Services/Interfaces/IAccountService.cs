using Unibean.Service.Models.Accounts;

namespace Unibean.Service.Services.Interfaces;

public interface IAccountService
{
    AccountModel AddGoogle(CreateGoogleAccountModel creation);

    AccountModel GetByEmail(string email);

    AccountModel GetByUserNameAndPassword(string userName, string password);
}
