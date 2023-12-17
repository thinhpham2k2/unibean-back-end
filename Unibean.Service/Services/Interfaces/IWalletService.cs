using Unibean.Service.Models.Wallets;

namespace Unibean.Service.Services.Interfaces;

public interface IWalletService
{
    WalletModel Add(CreateWalletModel creation);
}
