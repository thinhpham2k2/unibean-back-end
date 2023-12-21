using Unibean.Service.Models.Campuses;

namespace Unibean.Service.Services.Interfaces;

public interface ICampusService
{
    CampusModel GetById(string id);
}
