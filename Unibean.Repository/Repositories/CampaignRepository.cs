using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class CampaignRepository : ICampaignRepository
{
    public Campaign GetById(string id)
    {
        Campaign campaign = new Campaign();
        try
        {
            using (var db = new UnibeanDBContext())
            {
                campaign = db.Campaigns
                .Where(s => s.Id.Equals(id) && s.Status.Equals(true))
                .FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return campaign;
    }
}
