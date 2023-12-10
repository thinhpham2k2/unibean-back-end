using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

[Table("tbl_wallet")]
public class Wallet
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("campaign_id", TypeName = "char(26)")]
    public string CampaignId { get; set; }

    public Campaign Campaign { get; set; }

    [Column("student_id", TypeName = "char(26)")]
    public string StudentId { get; set; }

    public Student Student { get; set; }

    [Column("partner_id", TypeName = "char(26)")]
    public string PartnerId { get; set; }

    public Partner Partner { get; set; }

    [Column("type_id", TypeName = "char(26)")]
    public string TypeId { get; set; }

    public WalletType Type { get; set; }

    [Column("balance", TypeName = "decimal(38,2)")]
    public decimal? Balance { get; set; }

    [Column("date_created")]
    public DateTime? DateCreated { get; set; }

    [Column("date_updated")]
    public DateTime? DateUpdated { get; set; }

    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    [Column("state", TypeName = "bit(1)")]
    public bool? State { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }

    public virtual ICollection<OrderTransaction> OrderTransactions { get; set; }

    public virtual ICollection<ActivityTransaction> ActivityTransactions { get; set; }

    public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; }

    public virtual ICollection<WalletTransaction> WalletTransactions { get; set; }

    public virtual ICollection<RequestTransaction> RequestTransactions { get; set; }
}
