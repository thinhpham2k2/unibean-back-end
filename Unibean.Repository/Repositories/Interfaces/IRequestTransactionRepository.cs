using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IRequestTransactionRepository
{
    RequestTransaction Add(RequestTransaction creation);

    List<RequestTransaction> GetAll
        (List<string> walletIds, List<string> requestIds, string search);

    RequestTransaction GetById(string id);
}
