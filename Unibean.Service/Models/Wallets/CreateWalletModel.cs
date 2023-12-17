using Unibean.Service.Validations;

namespace Unibean.Service.Models.Wallets;

public class CreateWalletModel
{
    [ValidCampaign]
    public string CampaignId { get; set; }

    [ValidStudent]
    public string StudentId { get; set; }

    [ValidPartner]
    public string PartnerId { get; set; }

    [ValidWalletType]
    public string TypeId { get; set; }

    public decimal? Balance { get; set; }

    public string Description { get; set; }

    public bool? State { get; set; }
}
