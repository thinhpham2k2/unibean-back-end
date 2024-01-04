using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class OrderTransactionRepository : IOrderTransactionRepository
{
    public List<OrderTransaction> GetAll
        (List<string> walletIds, List<string> orderIds, string search)
    {
        List<OrderTransaction> result;
        try
        {
            using var db = new UnibeanDBContext();
            result = db.OrderTransactions
                .Where(o => (EF.Functions.Like("Tạo đơn hàng (" + o.Amount + " đậu)", "%" + search + "%")
                || EF.Functions.Like(o.Wallet.Type.TypeName, "%" + search + "%")
                || EF.Functions.Like("Đổi quà", "%" + search + "%")
                || EF.Functions.Like(o.Description, "%" + search + "%"))
                && (walletIds.Count == 0 || walletIds.Contains(o.WalletId))
                && (orderIds.Count == 0 || orderIds.Contains(o.OrderId))
                && o.Status.Equals(true))
                .Include(s => s.Wallet)
                    .ThenInclude(w => w.Type)
                .Include(s => s.Order).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }
}
