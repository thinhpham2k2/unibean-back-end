using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public record StudentRanking
{
    public string Name { get; set; }
    public string Image { get; set; }
    public decimal? TotalSpending { get; set; }
}

public interface IStudentRepository
{
    Student Add(Student creation);

    bool CheckStudentId(string id);

    long CountStudent();

    long CountStudentToday(DateOnly date);

    bool CheckCodeDuplicate(string code);

    bool CheckInviteCode(string inviteCode);

    void Delete(string id);

    PagedResultModel<Student> GetAll
        (List<string> majorIds, List<string> campusIds, List<string> universityIds,
        List<StudentState> stateIds, bool? isVerify, string propertySort, bool isAsc,
        string search, int page, int limit);

    Student GetById(string id);

    Student GetByIdForValidation(string id);

    List<Student> GetRanking(int limit);

    List<StudentRanking> GetRankingByBrand(string brandId, int limit);

    List<StudentRanking> GetRankingByStation(string stationId, int limit);

    List<StudentRanking> GetRankingByStore(string storeId, int limit);

    List<string> GetWalletListById(string id);

    Student Update(Student update);
}
