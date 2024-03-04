using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IStudentRepository
{
    Student Add(Student creation);

    long CountStudent();

    bool CheckCodeDuplicate(string code);

    bool CheckInviteCode(string inviteCode);

    void Delete(string id);

    PagedResultModel<Student> GetAll
        (List<string> majorIds, List<string> campusIds, List<StudentState> stateIds, 
        bool? isVerify, string propertySort, bool isAsc, string search, int page, int limit);

    Student GetById(string id);

    Student Update(Student update);
}
