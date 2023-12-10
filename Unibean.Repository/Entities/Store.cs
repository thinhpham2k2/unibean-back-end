using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

[Table("tbl_store")]
public class Store
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("partner_id", TypeName = "char(26)")]
    public string PartnerId { get; set; }

    public Partner Partner { get; set; }

    [Column("area_id", TypeName = "char(26)")]
    public string AreaId { get; set; }

    public Area Area { get; set; }

    [MaxLength(255)]
    [Column("store_name")]
    public string StoreName { get; set; }

    [MaxLength(50)]
    [Column("user_name")]
    public string UserName { get; set; }

    [MaxLength(255)]
    [Column("password")]
    public string Password { get; set; }

    [Column("image", TypeName = "text")]
    public string Image { get; set; }

    [EmailAddress]
    [MaxLength(320)]
    [Column("email")]
    public string Email { get; set; }

    [Column("address", TypeName = "text")]
    public string Address { get; set; }

    [Column("phone", TypeName = "char(20)")]
    public string Phone { get; set; }

    [Column("opening_hours")]
    public TimeOnly? OpeningHours { get; set; }

    [Column("closing_hours")]
    public TimeOnly? ClosingHours { get; set; }

    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    [Column("date_created")]
    public DateTime? DateCreated { get; set; }

    [Column("date_updated")]
    public DateTime? DateUpdated { get; set; }

    [Column("state", TypeName = "bit(1)")]
    public bool? State { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }

    public virtual ICollection<Activity> Activities { get; set; }

    public virtual ICollection<CampaignStore> CampaignStores { get; set; }
}
