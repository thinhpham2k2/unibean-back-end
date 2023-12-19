using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

[Table("tbl_store")]
public class Store
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("brand_id", TypeName = "char(26)")]
    public string BrandId { get; set; }

    public Brand Brand { get; set; }

    [Column("area_id", TypeName = "char(26)")]
    public string AreaId { get; set; }

    public Area Area { get; set; }

    [Column("account_id", TypeName = "char(26)")]
    public string AccountId { get; set; }

    public Account Account { get; set; }

    [MaxLength(255)]
    [Column("store_name")]
    public string StoreName { get; set; }

    [Column("address", TypeName = "text")]
    public string Address { get; set; }

    [Column("opening_hours")]
    public TimeOnly? OpeningHours { get; set; }

    [Column("closing_hours")]
    public TimeOnly? ClosingHours { get; set; }

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

    public virtual ICollection<Activity> Activities { get; set; }

    public virtual ICollection<CampaignStore> CampaignStores { get; set; }
}
