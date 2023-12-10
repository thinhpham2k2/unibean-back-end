using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

[Table("tbl_voucher_item")]
public class VoucherItem
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("voucher_id", TypeName = "char(26)")]
    public string VoucherId { get; set; }

    public Voucher Voucher { get; set; }

    [Column("campaign_id", TypeName = "char(26)")]
    public string CampaignId { get; set; }

    public Campaign Campaign { get; set; }

    [Column("price", TypeName = "decimal(38,2)")]
    public decimal? Price { get; set; }

    [Column("rate", TypeName = "decimal(38,2)")]
    public decimal? Rate { get; set; }

    [Column("is_bought", TypeName = "bit(1)")]
    public bool? IsBought { get; set; }

    [Column("is_used", TypeName = "bit(1)")]
    public bool? IsUsed { get; set; }

    [Column("valid_on")]
    public DateOnly? ValidOn { get; set; }

    [Column("expire_on")]
    public DateOnly? ExpireOn { get; set; }

    [Column("date_created")]
    public DateTime? DateCreated { get; set; }

    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    [Column("state", TypeName = "bit(1)")]
    public bool? State { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }

    public virtual ICollection<Activity> Activities { get; set; }
}
