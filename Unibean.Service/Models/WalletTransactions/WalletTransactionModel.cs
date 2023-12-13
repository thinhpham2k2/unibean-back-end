﻿namespace Unibean.Service.Models.WalletTransactions;

public class WalletTransactionModel
{
    public string Id { get; set; }
    public string WalletId { get; set; }
    public string CampaignId { get; set; }
    public string CampaignName { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Rate { get; set; }
    public string Description { get; set; }
    public bool? States { get; set; }
    public bool? Status { get; set; }
}
