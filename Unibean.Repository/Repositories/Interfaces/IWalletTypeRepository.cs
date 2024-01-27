using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IWalletTypeRepository
{
    WalletType Add(WalletType creation);

    void Delete(string id);

    PagedResultModel<WalletType> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    WalletType GetById(string id);

    WalletType Update(WalletType update);
}
