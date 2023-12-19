using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

[Table("tbl_brand")]
public class Brand
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("account_id", TypeName = "char(26)")]
    public string AccountId { get; set; }

    public Account Account { get; set; }

    [MaxLength(255)]
    [Column("brand_name")]
    public string BrandName { get; set; }

    [MaxLength(255)]
    [Column("acronym")]
    public string Acronym { get; set; }

    [Column("address", TypeName = "text")]
    public string Address { get; set; }

    [Column("cover_photo", TypeName = "text")]
    public string CoverPhoto { get; set; }

    [Column("cover_file_name", TypeName = "text")]
    public string CoverFileName { get; set; }

    [Column("link", TypeName = "text")]
    public string Link { get; set; }

    [Column("opening_hours")]
    public TimeOnly? OpeningHours { get; set; }

    [Column("closing_hours")]
    public TimeOnly? ClosingHours { get; set; }

    [Column("total_income", TypeName = "decimal(38,2)")]
    public decimal? TotalIncome { get; set; }

    [Column("total_spending", TypeName = "decimal(38,2)")]
    public decimal? TotalSpending { get; set; }

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

    public virtual ICollection<Voucher> Vouchers { get; set; }

    public virtual ICollection<Store> Stores { get; set; }

    public virtual ICollection<Wallet> Wallets { get; set; }

    public virtual ICollection<Campaign> Campaigns { get; set; }

    public virtual ICollection<Wishlist> Wishlists { get; set; }

    public virtual ICollection<Request> Requests { get; set; }
}
