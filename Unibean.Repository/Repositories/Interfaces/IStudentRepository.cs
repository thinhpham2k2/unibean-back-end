using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IStudentRepository
{
    Student Add(Student creation);

    Student GetById(string id);
}
