using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IAccountRepository
{
    bool CheckPhoneDuplicate(string phone);

    bool CheckEmailDuplicate(string email);

    bool CheckUsernameDuplicate(string userName);

    Account Add(Account creation);

    Account GetById(string id);

    Account GetByEmail(string email);

    Account GetByUserNameAndPassword(string userName, string password);
}
