using Unibean.Repository.Entities;
using Unibean.Service.Models.Accounts;

namespace Unibean.Service.Services.Interfaces;

public interface IAccountService
{
    Task<AccountModel> AddBrand(CreateBrandAccountModel creation);

    AccountModel AddGoogle(CreateGoogleAccountModel creation);

    AccountModel GetByEmail(string email);

    AccountModel GetByUserNameAndPassword(string userName, string password);
}
