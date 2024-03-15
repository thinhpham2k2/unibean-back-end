using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class StudentRepository : IStudentRepository
{
    public Student Add(Student creation)
    {
        try
        {
            using var db = new UnibeanDBContext();

            creation = db.Students.Add(creation).Entity;

            if (creation != null)
            {
                // Create wallet
                db.Wallets.AddRange(new List<Wallet>() {
                    new() {
                    Id = Ulid.NewUlid().ToString(),
                    StudentId = creation.Id,
                    Type = WalletType.Green,
                    Balance = 0,
                    DateCreated = creation.DateCreated,
                    DateUpdated = creation.DateUpdated,
                    Description = WalletType.Green.GetEnumDescription(),
                    State = true,
                    Status = true,
                    },
                    new() {
                    Id = Ulid.NewUlid().ToString(),
                    StudentId = creation.Id,
                    Type = WalletType.Red,
                    Balance = 0,
                    DateCreated = creation.DateCreated,
                    DateUpdated = creation.DateUpdated,
                    Description = WalletType.Red.GetEnumDescription(),
                    State = true,
                    Status = true,
                    }
                });

                // Create student challenge
                foreach (var challenge in db.Challenges.Where(s => (bool)s.Status))
                {
                    db.StudentChallenges.Add(new StudentChallenge
                    {
                        Id = Ulid.NewUlid().ToString(),
                        ChallengeId = challenge.Id,
                        StudentId = creation.Id,
                        Amount = challenge.Amount,
                        Current = 0,
                        Condition = challenge.Condition,
                        IsCompleted = false,
                        DateCreated = creation.DateCreated,
                        DateUpdated = creation.DateUpdated,
                        Description = challenge.Description,
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

    public bool CheckCodeDuplicate(string code)
    {
        Student student = new();
        try
        {
            using var db = new UnibeanDBContext();
            student = db.Students
                .Where(a => a.Code.Equals(code)).FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return student == null;
    }

    public bool CheckInviteCode(string inviteCode)
    {
        Student student = new();
        try
        {
            using var db = new UnibeanDBContext();
            student = db.Students
                .Where(s => s.Id.Equals(inviteCode) && (bool)s.Status 
                && s.State.Equals(StudentState.Active)).FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return student != null;
    }

    public bool CheckStudentId(string id)
    {
        Student student = new();
        try
        {
            using var db = new UnibeanDBContext();
            student = db.Students
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return student != null;
    }

    public long CountStudent()
    {
        long count = 0;
        try
        {
            using var db = new UnibeanDBContext();
            count = db.Students.Where(c => (bool)c.Status).Count();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return count;
    }

    public long CountStudentToday(DateOnly date)
    {
        long count = 0;
        try
        {
            using var db = new UnibeanDBContext();
            count = db.Students.Where(c => (bool)c.Status 
            && DateOnly.FromDateTime(c.DateCreated.Value).Equals(date)).Count();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return count;
    }

    public void Delete(string id)
    {
        try
        {
            using var db = new UnibeanDBContext();
            var student = db.Students.FirstOrDefault(b => b.Id.Equals(id));
            student.Status = false;
            db.Students.Update(student);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Student> GetAll
        (List<string> majorIds, List<string> campusIds, List<StudentState> stateIds,
        bool? isVerify, string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Student> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Students
                .Where(s => (EF.Functions.Like(s.Id, "%" + search + "%")
                || EF.Functions.Like(s.FullName, "%" + search + "%")
                || EF.Functions.Like(s.Code, "%" + search + "%")
                || EF.Functions.Like(s.Address, "%" + search + "%")
                || EF.Functions.Like((string)(object)s.Gender, "%" + search + "%")
                || EF.Functions.Like(s.Major.MajorName, "%" + search + "%")
                || EF.Functions.Like(s.Campus.CampusName, "%" + search + "%"))
                && (majorIds.Count == 0 || majorIds.Contains(s.MajorId))
                && (campusIds.Count == 0 || campusIds.Contains(s.CampusId))
                && (stateIds.Count == 0 || stateIds.Contains(s.State.Value))
                && (isVerify == null || isVerify.Equals(s.Account.IsVerify))
                && (bool)s.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(b => b.Major)
               .Include(b => b.Campus)
                   .ThenInclude(c => c.University)
               .Include(b => b.Account)
               .Include(s => s.Wallets.Where(w => (bool)w.Status))
               .ToList();

            pagedResult = new PagedResultModel<Student>
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

    public Student GetById(string id)
    {
        Student student = new();
        try
        {
            using var db = new UnibeanDBContext();
            student = db.Students
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(b => b.Major)
            .Include(b => b.Campus)
                .ThenInclude(c => c.University)
            .Include(b => b.Account)
            .Include(s => s.Activities.Where(a => (bool)a.Status))
            .Include(s => s.Wallets.Where(w => (bool)w.Status))
            .Include(s => s.Wishlists.Where(w => (bool)w.Status))
            .Include(s => s.Inviters.Where(i => (bool)i.Status))
            .Include(s => s.Invitees.Where(i => (bool)i.Status))
                .ThenInclude(i => i.Inviter)
            .Include(s => s.StudentChallenges.Where(s => (bool)s.Status))
                .ThenInclude(s => s.Challenge)
            .Include(s => s.Orders.Where(s => (bool)s.Status))
                .ThenInclude(s => s.OrderStates.Where(s => (bool)s.Status))
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return student;
    }

    public List<string> GetWalletListById(string id)
    {
        List<string> list = new();
        try
        {
            using var db = new UnibeanDBContext();
            list.AddRange(db.Students
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Wallets).FirstOrDefault()
            .Wallets.Select(w => w.Id).ToList());
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return list;
    }

    public Student Update(Student update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.Students.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
