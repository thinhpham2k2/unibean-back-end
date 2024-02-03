using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class OrderTransactionRepository : IOrderTransactionRepository
{
    public OrderTransaction Add(OrderTransaction creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.OrderTransactions.Add(creation).Entity;

            if (creation != null)
            {
                // Update wallet balance
                var wallet = db.Wallets.Where(w => (bool)w.Status && w.Id.Equals(creation.WalletId))
                    .FirstOrDefault();
                wallet.Balance += creation.Amount;
                wallet.DateUpdated = DateTime.Now;
                db.Wallets.Update(wallet);
            }
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return creation;
    }

    public List<OrderTransaction> GetAll
        (List<string> walletIds, List<string> orderIds, string search)
    {
        List<OrderTransaction> result;
        try
        {
            using var db = new UnibeanDBContext();
            result = db.OrderTransactions
                .Where(o => (EF.Functions.Like("Tạo đơn hàng (" + o.Amount + " đậu)", "%" + search + "%")
                || EF.Functions.Like((string)(object)o.Wallet.Type, "%" + search + "%")
                || EF.Functions.Like("Đổi quà", "%" + search + "%")
                || EF.Functions.Like(o.Description, "%" + search + "%"))
                && (walletIds.Count == 0 || walletIds.Contains(o.WalletId))
                && (orderIds.Count == 0 || orderIds.Contains(o.OrderId))
                && (bool)o.Status)
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

    public OrderTransaction GetById(string id)
    {
        OrderTransaction orderTransaction = new();
        try
        {
            using var db = new UnibeanDBContext();
            orderTransaction = db.OrderTransactions
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Wallet)
                .ThenInclude(w => w.Type)
            .Include(s => s.Order)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return orderTransaction;
    }
}
