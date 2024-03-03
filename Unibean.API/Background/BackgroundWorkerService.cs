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
            await Task.Delay(300000, stoppingToken);
        }
    }
}
