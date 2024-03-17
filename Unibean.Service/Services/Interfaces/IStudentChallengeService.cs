using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.StudentChallenges;

namespace Unibean.Service.Services.Interfaces;

public interface IStudentChallengeService
{
    StudentChallengeModel Add(CreateStudentChallengeModel creation);

    void Delete(string id);

    PagedResultModel<StudentChallengeModel> GetAll
        (List<string> studentIds, List<string> challengeIds, List<ChallengeType> typeIds,
        bool? state, string propertySort, bool isAsc, string search, int page, int limit);

    StudentChallengeModel GetById(string id);

    void Update(IEnumerable<StudentChallenge> studentChallenges, decimal amount);
}
