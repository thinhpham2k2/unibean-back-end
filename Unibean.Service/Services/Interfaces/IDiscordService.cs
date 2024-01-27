using Unibean.Service.Models.WebHooks;

namespace Unibean.Service.Services.Interfaces;

public interface IDiscordService
{
    void CreateWebHooks(DiscordWebhookModel webhookModel);
}
