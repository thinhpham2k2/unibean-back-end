using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Unibean.Repository.Repositories;

public class StudentRepository : IStudentRepository
{
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

    public Student GetByUserNameAndPassword(string userName, string password)
    {
        Student student = new();
        try
        {
            using (var db = new UnibeanDBContext())
            {
                student = db.Students.Where(s => s.UserName.Equals(userName)
                && s.Status.Equals(true))
                    .Include(s => s.Level)
                    .Include(s => s.Gender)
                    .Include(s => s.Major)
                    .Include(s => s.Campus).FirstOrDefault();
            }
            if (student != null)
            {
                if (!BCryptNet.Verify(password, student.Password))
                {
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return student;
    }
}
