using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Transactions;

namespace Unibean.Service.Services.Interfaces;

public interface ITransactionService
{
    PagedResultModel<TransactionModel> GetAll
        (List<string> walletIds, List<TransactionType> typeIds, bool? state, 
        string propertySort, bool isAsc, string search, int page, int limit, Role role);
}
