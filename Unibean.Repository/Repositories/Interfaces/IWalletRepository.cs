using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IWalletRepository
{
    Wallet Add(Wallet creation);
}
