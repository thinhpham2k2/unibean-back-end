using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IStudentChallengeRepository
{
    StudentChallenge Add(StudentChallenge creation);

    void Delete(string id);

    PagedResultModel<StudentChallenge> GetAll
        (List<string> studentIds, List<string> challengeIds, string propertySort, bool isAsc, string search, int page, int limit);

    StudentChallenge GetById(string id);

    StudentChallenge Update(StudentChallenge update);
}
