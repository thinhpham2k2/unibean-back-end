using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Repository.Entities;

[Table("tbl_voucher")]
public class Voucher
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("brand_id", TypeName = "char(26)")]
    public string BrandId { get; set; }

    public Brand Brand { get; set; }

    [Column("type_id", TypeName = "char(26)")]
    public string TypeId { get; set; }

    public VoucherType Type { get; set; }

    [MaxLength(255)]
    [Column("voucher_name")]
    public string VoucherName { get; set; }

    [Column("price", TypeName = "decimal(38,2)")]
    public decimal? Price { get; set; }

    [Column("rate", TypeName = "decimal(38,2)")]
    public decimal? Rate { get; set; }

    [Column("condition", TypeName = "text")]
    public string Condition { get; set; }

    [Column("image", TypeName = "text")]
    public string Image { get; set; }

    [Column("image_name", TypeName = "text")]
    public string ImageName { get; set; }

    [Column("file", TypeName = "text")]
    public string File { get; set; }

    [Column("file_name", TypeName = "text")]
    public string FileName { get; set; }

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

    public virtual ICollection<CampaignDetail> CampaignDetails { get; set; }
}
