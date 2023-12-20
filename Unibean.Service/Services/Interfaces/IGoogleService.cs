using Unibean.Service.Models.Accounts;
using Unibean.Service.Models.Authens;

namespace Unibean.Service.Services.Interfaces;

public interface IGoogleService
{
    Task<AccountModel> LoginWithGoogle(GoogleTokenModel token, string role);
}
