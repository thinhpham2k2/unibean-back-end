using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class PaymentTransactionRepository : IPaymentTransactionRepository
{
    public List<PaymentTransaction> GetAll
        (List<string> walletIds, List<string> paymentIds, string search)
    {
        List<PaymentTransaction> result;
        try
        {
            using var db = new UnibeanDBContext();
            result = db.PaymentTransactions
                .Where(p => (EF.Functions.Like("Nạp đậu (" + p.Amount + " đậu)", "%" + search + "%")
                || EF.Functions.Like(p.Wallet.Type.TypeName, "%" + search + "%")
                || EF.Functions.Like("Thanh toán", "%" + search + "%")
                || EF.Functions.Like(p.Description, "%" + search + "%"))
                && (walletIds.Count == 0 || walletIds.Contains(p.WalletId))
                && (paymentIds.Count == 0 || paymentIds.Contains(p.PaymentId))
                && p.Status.Equals(true))
                .Include(s => s.Wallet)
                    .ThenInclude(w => w.Type)
                .Include(s => s.Payment).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }
}
