using Unibean.Service.Models.Stores;

namespace Unibean.Service.Services.Interfaces;

public interface IStoreService
{
    StoreModel GetByUserNameAndPassword(string userName, string password);
}
