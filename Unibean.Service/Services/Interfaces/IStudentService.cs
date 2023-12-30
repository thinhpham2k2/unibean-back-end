using Unibean.Repository.Paging;
using Unibean.Service.Models.Students;

namespace Unibean.Service.Services.Interfaces;

public interface IStudentService
{
    Task<StudentModel> AddGoogle(CreateStudentGoogleModel creation);

    PagedResultModel<StudentModel> GetAll
        (List<string> levelIds, List<string> genderIds, List<string> majorIds, List<string> campusIds,
        bool? isVerify, string propertySort, bool isAsc, string search, int page, int limit);
}
