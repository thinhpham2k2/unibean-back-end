using Unibean.Service.Models.Partners;

namespace Unibean.Service.Services.Interfaces;

public interface IPartnerService
{
    Task<PartnerExtraModel> Add(CreatePartnerModel creation);

    PartnerModel GetByUserNameAndPassword(string userName, string password);
}
