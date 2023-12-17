using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class WalletRepository : IWalletRepository
{
    public Wallet Add(Wallet creation)
    {
        try
        {
            using (var db = new UnibeanDBContext())
            {
                creation = db.Wallets.Add(creation).Entity;
                db.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return creation;
    }
}
