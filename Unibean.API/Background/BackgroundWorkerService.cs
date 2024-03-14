using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.API.Background;

public class BackgroundWorkerService : BackgroundService
{
    readonly ILogger<BackgroundWorkerService> _logger;

    private readonly ICampaignRepository campaignRepository;

    private readonly ICampaignActivityRepository campaignActivityRepository;

    public BackgroundWorkerService(
        ILogger<BackgroundWorkerService> logger,
        ICampaignRepository campaignRepository,
        ICampaignActivityRepository campaignActivityRepository)
    {
        _logger = logger;
        this.campaignRepository = campaignRepository;
        this.campaignActivityRepository = campaignActivityRepository;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTime.UtcNow);

            List<Campaign> campaigns = campaignRepository
                .GetAllExpired(new() {
                    CampaignState.Pending,
                    CampaignState.Rejected,
                    CampaignState.Active,
                    CampaignState.Inactive },
                DateOnly.FromDateTime(DateTime.Now));

            if (campaigns.Count > 0)
            {
                foreach (Campaign campaign in campaigns)
                {
                    campaignActivityRepository.Add(new CampaignActivity
                    {
                        Id = Ulid.NewUlid().ToString(),
                        CampaignId = campaign.Id,
                        State = CampaignState.Finished,
                        DateCreated = DateTime.Now,
                        Description = CampaignState.Finished.GetEnumDescription(),
                        Status = true,
                    });
                }
            }

            campaigns = campaignRepository.GetAllExpired(new() { CampaignState.Finished },
                DateOnly.FromDateTime(DateTime.Now));

            if (campaigns.Count > 0)
            {
                foreach (Campaign campaign in campaigns)
                {
                    campaignRepository.ExpiredToClosed(campaign.Id);
                    campaignActivityRepository.Add(new CampaignActivity
                    {
                        Id = Ulid.NewUlid().ToString(),
                        CampaignId = campaign.Id,
                        State = CampaignState.Closed,
                        DateCreated = DateTime.Now,
                        Description = CampaignState.Closed.GetEnumDescription(),
                        Status = true,
                    });
                }
            }

            campaigns = campaignRepository.GetAllEnded(new() { CampaignState.Active });

            if (campaigns.Count > 0)
            {
                foreach (Campaign campaign in campaigns)
                {
                    campaignRepository.ExpiredToClosed(campaign.Id);
                    campaignActivityRepository.Add(new CampaignActivity
                    {
                        Id = Ulid.NewUlid().ToString(),
                        CampaignId = campaign.Id,
                        State = CampaignState.Inactive,
                        DateCreated = DateTime.Now,
                        Description = CampaignState.Inactive.GetEnumDescription(),
                        Status = true,
                    });
                }
            }

            await Task.Delay(300000, stoppingToken);
        }
    }
}
