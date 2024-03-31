using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IStudentChallengeRepository
{
    StudentChallenge Add(StudentChallenge creation);

    void Delete(string id);

    List<StudentChallenge> GetAll
        (List<string> studentIds, List<string> challengeIds, List<ChallengeType> typeIds,
        bool? state, string propertySort, bool isAsc, string search);

    StudentChallenge GetById(string id);

    StudentChallenge Update(StudentChallenge update);
}
