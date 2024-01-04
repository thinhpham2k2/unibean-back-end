using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IOrderTransactionRepository
{
    List<OrderTransaction> GetAll
        (List<string> walletIds, List<string> orderIds, string search);
}
