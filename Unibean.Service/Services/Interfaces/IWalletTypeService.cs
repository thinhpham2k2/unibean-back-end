using Unibean.Repository.Paging;
using Unibean.Service.Models.WalletTypes;

namespace Unibean.Service.Services.Interfaces;

public interface IWalletTypeService
{
    Task<WalletTypeModel> Add(CreateWalletTypeModel creation);

    void Delete(string id);

    PagedResultModel<WalletTypeModel> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    WalletTypeModel GetById(string id);

    Task<WalletTypeModel> Update(string id, UpdateWalletTypeModel update);
}
