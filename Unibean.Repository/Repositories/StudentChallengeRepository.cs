using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class StudentChallengeRepository : IStudentChallengeRepository
{
    private readonly UnibeanDBContext unibeanDB;

    public StudentChallengeRepository(UnibeanDBContext unibeanDB)
    {
        this.unibeanDB = unibeanDB;
    }

    public StudentChallenge Add(StudentChallenge creation)
    {
        try
        {
            var db = unibeanDB;
            creation = db.StudentChallenges.Add(creation).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return creation;
    }

    public void Delete(string id)
    {
        try
        {
            var db = unibeanDB;
            var studentChallenge = db.StudentChallenges
                .FirstOrDefault(b => b.Id.Equals(id));
            studentChallenge.Status = false;
            db.StudentChallenges.Update(studentChallenge);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<StudentChallenge> GetAll
        (List<string> studentIds, List<string> challengeIds, List<ChallengeType> typeIds,
        bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<StudentChallenge> pagedResult = new();
        try
        {
            var db = unibeanDB;
            var query = db.StudentChallenges
                .Where(t => (EF.Functions.Like(t.Challenge.ChallengeName, "%" + search + "%")
                || EF.Functions.Like(t.Student.FullName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (studentIds.Count == 0 || studentIds.Contains(t.StudentId))
                && (challengeIds.Count == 0 || challengeIds.Contains(t.ChallengeId))
                && (typeIds.Count == 0 || typeIds.Contains(t.Challenge.Type.Value))
                && (state == null || state.Equals(t.State))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(s => s.Student)
               .Include(s => s.Challenge)
               .Include(s => s.ChallengeTransactions.Where(c => (bool)c.Status))
               .ToList();

            pagedResult = new PagedResultModel<StudentChallenge>
            {
                CurrentPage = page,
                PageSize = limit,
                PageCount = (int)Math.Ceiling((double)query.Count() / limit),
                Result = result,
                RowCount = result.Count,
                TotalCount = query.Count()
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return pagedResult;
    }

    public StudentChallenge GetById(string id)
    {
        StudentChallenge studentChallenge = new();
        try
        {
            var db = unibeanDB;
            studentChallenge = db.StudentChallenges
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Student)
            .Include(s => s.Challenge)
            .Include(s => s.ChallengeTransactions.Where(c => (bool)c.Status))
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return studentChallenge;
    }

    public StudentChallenge Update(StudentChallenge update)
    {
        try
        {
            var db = unibeanDB;
            update = db.StudentChallenges.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
