using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IRequestTransactionRepository
{
    RequestTransaction Add(RequestTransaction creation);

    List<RequestTransaction> GetAll
        (List<string> walletIds, List<string> requestIds,
        List<WalletType> walletTypeIds, string search);

    RequestTransaction GetById(string id);

    decimal IncomeOfGreenBean(string brandId, DateOnly date);
}
