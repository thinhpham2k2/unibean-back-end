using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IStudentRepository
{
    Student GetById(string id);
}
