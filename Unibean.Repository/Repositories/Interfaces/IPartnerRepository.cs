using Unibean.Repository.Entities;
using Unibean.Repository.Paging;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IPartnerRepository
{
    Partner Add(Partner creation);

    void Delete(string id);

    PagedResultModel<Partner> GetAll
        (string propertySort, bool isAsc, string search, int page, int limit);

    Partner GetById(string id);

    Partner Update(Partner update);

    bool CheckEmailDuplicate(string email);

    bool CheckUsernameDuplicate(string userName);

    Partner GetByUserNameAndPassword(string userName, string password);
}
