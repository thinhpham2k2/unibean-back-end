using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

[Table("tbl_partner")]
public class Partner
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [MaxLength(255)]
    [Column("brand_name")]
    public string BrandName { get; set; }

    [MaxLength(255)]
    [Column("acronym")]
    public string Acronym { get; set; }

    [MaxLength(50)]
    [Column("user_name")]
    public string UserName { get; set; }

    [MaxLength(255)]
    [Column("password")]
    public string Password { get; set; }

    [Column("address", TypeName = "text")]
    public string Address { get; set; }

    [Column("logo", TypeName = "text")]
    public string Logo { get; set; }

    [Column("cover_photo", TypeName = "text")]
    public string CoverPhoto { get; set; }

    [EmailAddress]
    [MaxLength(320)]
    [Column("email")]
    public string Email { get; set; }

    [Column("phone", TypeName = "char(20)")]
    public string Phone { get; set; }

    [Column("opening_hours")]
    public TimeOnly? OpeningHours { get; set; }

    [Column("closing_hours")]
    public TimeOnly? ClosingHours { get; set; }

    [Column("link", TypeName = "text")]
    public string Link { get; set; }

    [Column("total_income", TypeName = "decimal(38,2)")]
    public decimal? TotalIncome { get; set; }

    [Column("total_spending", TypeName = "decimal(38,2)")]
    public decimal? TotalSpending { get; set; }

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

    public virtual ICollection<Voucher> Vouchers { get; set; }

    public virtual ICollection<Store> Stores { get; set; }

    public virtual ICollection<Wallet> Wallets { get; set; }

    public virtual ICollection<Campaign> Campaigns { get; set; }

    public virtual ICollection<Wishlist> Wishlists { get; set; }

    public virtual ICollection<Request> Requests { get; set; }
}
