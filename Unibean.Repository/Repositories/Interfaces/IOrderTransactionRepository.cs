using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IOrderTransactionRepository
{
    OrderTransaction Add(OrderTransaction creation);

    List<OrderTransaction> GetAll
        (List<string> walletIds, List<string> orderIds, string search);

    OrderTransaction GetById(string id);

    decimal IncomeOfRedBean(DateOnly date);

    decimal IncomeOfRedBean(string stationId, DateOnly date);

    decimal OutcomeOfRedBean(string stationId, DateOnly date);
}
