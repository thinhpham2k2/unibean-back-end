using Unibean.Service.Models.Partners;

namespace Unibean.Service.Services.Interfaces;

public interface IPartnerService
{
    PartnerModel GetByUserNameAndPassword(string userName, string password);
}
