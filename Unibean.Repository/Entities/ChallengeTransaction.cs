using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

[Table("tbl_challenge_transaction")]
public class ChallengeTransaction
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("wallet_id", TypeName = "char(26)")]
    public string WalletId { get; set; }

    public Wallet Wallet { get; set; }

    [Column("challenge_id", TypeName = "char(26)")]
    public string ChallengeId { get; set; }

    public StudentChallenge Challenge { get; set; }

    [Column("amount", TypeName = "decimal(38,2)")]
    public decimal? Amount { get; set; }

    [Column("rate", TypeName = "decimal(38,2)")]
    public decimal? Rate { get; set; }

    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    [Column("state", TypeName = "bit(1)")]
    public bool? State { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }
}
