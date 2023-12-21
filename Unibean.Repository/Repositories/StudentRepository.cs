using Unibean.Repository.Entities;
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
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return creation;
    }

    public Student GetById(string id)
    {
        Student student = new();
        try
        {
            using var db = new UnibeanDBContext();
            student = db.Students
            .Where(s => s.Id.Equals(id) && s.Status.Equals(true))
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return student;
    }
}
