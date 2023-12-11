﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

[Table("tbl_campaign")]
public class Campaign
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("partner_id", TypeName = "char(26)")]
    public string PartnerId { get; set; }

    public Partner Partner { get; set; }

    [Column("type_id", TypeName = "char(26)")]
    public string TypeId { get; set; }

    public CampaignType Type { get; set; }

    [MaxLength(255)]
    [Column("campaign_name")]
    public string CampaignName { get; set; }

    [Column("image", TypeName = "text")]
    public string Image { get; set; }

    [Column("condition", TypeName = "text")]
    public string Condition { get; set; }

    [Column("link", TypeName = "text")]
    public string Link { get; set; }

    [Column("start_on")]
    public DateOnly? StartOn { get; set; }

    [Column("end_on")]
    public DateOnly? EndOn { get; set; }

    [Column("duration")]
    public int? Duration { get; set; }

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

    public virtual ICollection<CampaignCampus> CampaignCampuses { get; set; }

    public virtual ICollection<CampaignGender> CampaignGenders { get; set; }

    public virtual ICollection<CampaignMajor> CampaignMajors { get; set; }

    public virtual ICollection<CampaignStore> CampaignStores { get; set; }

    public virtual ICollection<VoucherItem> VoucherItems { get; set; }

    public virtual ICollection<Wallet> Wallets { get; set; }

    public virtual ICollection<WalletTransaction> WalletTransactions { get; set; }
}