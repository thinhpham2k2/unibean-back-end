using Unibean.Service.Models.Students;

namespace Unibean.Service.Services.Interfaces;

public interface IStudentService
{
    StudentModel GetByUserNameAndPassword(string userName, string password);
}
