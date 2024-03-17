using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

[Table("tbl_campaign_detail")]
public class CampaignDetail
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

    [Column("quantity")]
    public int? Quantity { get; set; }

    [Column("from_index")]
    public int? FromIndex { get; set; }

    [Column("to_index")]
    public int? ToIndex { get; set; }

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

    public virtual ICollection<VoucherItem> VoucherItems { get; set; }
}
