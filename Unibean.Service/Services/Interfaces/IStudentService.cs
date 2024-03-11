using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.ChallengeTransactions;
using Unibean.Service.Models.Orders;
using Unibean.Service.Models.StudentChallenges;
using Unibean.Service.Models.Students;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Models.VoucherItems;

namespace Unibean.Service.Services.Interfaces;

public interface IStudentService
{
    Task<StudentExtraModel> Add(CreateStudentModel creation);

    Task<StudentModel> AddGoogle(CreateStudentGoogleModel creation);

    ChallengeTransactionModel ClaimChallenge(string id, string challengeId);

    void Delete(string id);

    PagedResultModel<StudentModel> GetAll
        (List<string> majorIds, List<string> campusIds, List<StudentState> stateIds, bool? isVerify, 
        string propertySort, bool isAsc, string search, int page, int limit);

    StudentExtraModel GetById(string id);

    OrderExtraModel GetOrderByOrderId(string id, string orderId);

    PagedResultModel<StudentChallengeModel> GetChallengeListByStudentId
        (List<ChallengeType> typeIds, string id, bool? isCompleted, bool? state, bool? isClaimed, string propertySort, 
        bool isAsc, string search, int page, int limit);

    PagedResultModel<TransactionModel> GetHistoryTransactionListByStudentId
        (string id, List<TransactionType> typeIds, bool? state, string propertySort, 
        bool isAsc, string search, int page, int limit);

    PagedResultModel<OrderModel> GetOrderListByStudentId
        (List<string> stationIds, List<State> stateIds, string id, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit);

    VoucherItemExtraModel GetVoucherItemByVoucherId(string id, string voucherId);

    PagedResultModel<VoucherItemModel> GetVoucherListByStudentId
        (List<string> campaignIds, List<string> voucherIds, List<string> brandIds, List<string> typeIds,
        string id, bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    List<string> GetWishlistsByStudentId(string id);

    Task<StudentExtraModel> Update(string id, UpdateStudentModel update);

    bool UpdateInviteCode(string id, string code);

    bool UpdateState(string id, StudentState stateId);

    Task<StudentExtraModel> UpdateVerification(string id, UpdateStudentVerifyModel update);
}
