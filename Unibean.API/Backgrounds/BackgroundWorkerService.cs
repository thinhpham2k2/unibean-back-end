using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Services.Interfaces;

namespace Unibean.API.Backgrounds;

public class BackgroundWorkerService : BackgroundService
{
    readonly ILogger<BackgroundWorkerService> _logger;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    public BackgroundWorkerService(
        ILogger<BackgroundWorkerService> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        this._serviceScopeFactory = serviceScopeFactory; ;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
            var campaignRepository = scope.ServiceProvider.GetRequiredService<ICampaignRepository>();
            var campaignActivityRepository = scope.ServiceProvider.GetRequiredService<ICampaignActivityRepository>();
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
                    // Send mail for brand
                    emailService.SendEmailCamapaign(CampaignState.Finished, campaign.Brand.Account.Email,
                        campaign.Brand.BrandName, campaign.CampaignName, null);

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
                    // Send mail for brand
                    emailService.SendEmailCamapaign(CampaignState.Closed, campaign.Brand.Account.Email,
                        campaign.Brand.BrandName, campaign.CampaignName, null);

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
                    // Send mail for brand
                    emailService.SendEmailCamapaign(CampaignState.Inactive, campaign.Brand.Account.Email,
                        campaign.Brand.BrandName, campaign.CampaignName, null);

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
