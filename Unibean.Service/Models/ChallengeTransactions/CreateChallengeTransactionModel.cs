namespace Unibean.Service.Models.ChallengeTransactions;

public class CreateChallengeTransactionModel
{
    public string WalletId { get; set; }

    public string ChallengeId { get; set; }

    public decimal? Amount { get; set; }

    public decimal? Rate { get; set; }

    public string Description { get; set; }

    public bool? State { get; set; }
}
