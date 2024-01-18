using Unibean.Repository.Paging;
using Unibean.Service.Models.Orders;
using Unibean.Service.Models.StudentChallenges;
using Unibean.Service.Models.Students;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Models.VoucherItems;

namespace Unibean.Service.Services.Interfaces;

public interface IStudentService
{
    Task<StudentModel> Add(CreateStudentModel creation);

    Task<StudentModel> AddGoogle(CreateStudentGoogleModel creation);

    void Delete(string id);

    PagedResultModel<StudentModel> GetAll
        (List<string> majorIds, List<string> campusIds, bool? isVerify, 
        string propertySort, bool isAsc, string search, int page, int limit);

    StudentExtraModel GetById(string id);

    OrderExtraModel GetOrderByOrderId(string id, string orderId);

    PagedResultModel<StudentChallengeModel> GetChallengeListByStudentId
        (string id, bool? isCompleted, bool? isClaimed, string propertySort, bool isAsc, string search, int page, int limit);

    PagedResultModel<TransactionModel> GetHistoryTransactionListByStudentId
        (string id, string propertySort, bool isAsc, string search, int page, int limit);

    PagedResultModel<OrderModel> GetOrderListByStudentId
        (List<string> stationIds, List<string> stateIds, string id, string propertySort, bool isAsc, string search, int page, int limit);

    PagedResultModel<VoucherItemModel> GetVoucherListByStudentId
        (List<string> campaignIds, List<string> voucherIds, List<string> brandIds, List<string> typeIds,
        string id, string propertySort, bool isAsc, string search, int page, int limit);

    List<string> GetWishlistsByStudentId(string id);

    Task<StudentExtraModel> Update(string id, UpdateStudentModel update);
}
