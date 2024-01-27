using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IStudentRepository
{
    Student Add(Student creation);

    bool CheckInviteCode(string inviteCode);

    bool CheckCodeDuplicate(string code);

    void Delete(string id);

    PagedResultModel<Student> GetAll
        (List<string> majorIds, List<string> campusIds, bool? state, bool? isVerify,
        string propertySort, bool isAsc, string search, int page, int limit);

    Student GetById(string id);

    Student Update(Student update);
}
