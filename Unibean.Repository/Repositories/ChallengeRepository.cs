using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class ChallengeRepository : IChallengeRepository
{
    public Challenge Add(Challenge creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.Challenges.Add(creation).Entity;

            if (creation != null)
            {
                // Create student challenge
                foreach (var student in db.Students.Where(
                    s => (bool)s.Status).Include(s => s.StudentChallenges.Where(
                        s => (bool)s.Status)).ThenInclude(s => s.Challenge))
                {
                    var current = student.StudentChallenges.Where(
                        s => s.Challenge.Type.Value.Equals(creation.Type.Value)).OrderBy(s => s.Id)
                        .LastOrDefault()?.Current;
                    
                    db.StudentChallenges.Add(new StudentChallenge
                    {
                        Id = Ulid.NewUlid().ToString(),
                        ChallengeId = creation.Id,
                        StudentId = student.Id,
                        Amount = creation.Amount,
                        Current = current ?? 0,
                        Condition = creation.Condition,
                        IsCompleted = current >= creation.Condition,
                        DateCreated = creation.DateCreated,
                        DateUpdated = creation.DateUpdated,
                        Description = creation.Description,
                        State = true,
                        Status = true,
                    });
                }
            }
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
            using var db = new UnibeanDBContext();
            var challenge = db.Challenges.FirstOrDefault(b => b.Id.Equals(id));
            challenge.Status = false;
            db.Challenges.Update(challenge);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Challenge> GetAll
        (List<ChallengeType> typeIds, bool? state, string propertySort, 
        bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Challenge> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Challenges
                .Where(t => (EF.Functions.Like(t.ChallengeName, "%" + search + "%")
                || EF.Functions.Like((string)(object)t.Type, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (typeIds.Count == 0 || typeIds.Contains(t.Type.Value))
                && (state == null || state.Equals(t.State))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(c => c.StudentChallenges.Where(s => (bool)s.Status))
               .ToList();

            pagedResult = new PagedResultModel<Challenge>
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

    public Challenge GetById(string id)
    {
        Challenge challenge = new();
        try
        {
            using var db = new UnibeanDBContext();
            challenge = db.Challenges
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(c => c.StudentChallenges.Where(s => (bool)s.Status))
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return challenge;
    }

    public Challenge Update(Challenge update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.Challenges.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
