using Unibean.Service.Models.Students;

namespace Unibean.Service.Services.Interfaces;

public interface IStudentService
{
    Task<StudentModel> AddGoogle(CreateGoogleStudentModel creation);
}
