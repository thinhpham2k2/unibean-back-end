using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class WalletRepository : IWalletRepository
{
    private readonly UnibeanDBContext unibeanDB;

    public WalletRepository(UnibeanDBContext unibeanDB)
    {
        this.unibeanDB = unibeanDB;
    }

    public Wallet Add(Wallet creation)
    {
        try
        {
            var db = unibeanDB;
            creation = db.Wallets.Add(creation).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return creation;
    }
}
