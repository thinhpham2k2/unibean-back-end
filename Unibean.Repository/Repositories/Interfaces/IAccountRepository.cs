using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IAccountRepository
{
    bool CheckPhoneDuplicate(string phone);

    bool CheckEmailDuplicate(string email);

    bool CheckUsernameDuplicate(string userName);

    Account GetByUserNameAndPassword(string userName, string password);
}
