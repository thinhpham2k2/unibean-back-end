using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface ICampaignRepository
{
    Campaign GetById(string id);
}
