using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IPaymentTransactionRepository
{
    List<PaymentTransaction> GetAll
        (List<string> walletIds, List<string> paymentIds, string search);
}
