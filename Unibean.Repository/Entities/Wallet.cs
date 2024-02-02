﻿using System.ComponentModel.DataAnnotations;
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

    [Column("brand_id", TypeName = "char(26)")]
    public string BrandId { get; set; }

    public Brand Brand { get; set; }

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

    public virtual ICollection<BonusTransaction> BonusTransactions { get; set; }

    public virtual ICollection<CampaignTransaction> WalletTransactions { get; set; }

    public virtual ICollection<RequestTransaction> RequestTransactions { get; set; }

    public virtual ICollection<ChallengeTransaction> ChallengeTransactions { get; set; }
}
