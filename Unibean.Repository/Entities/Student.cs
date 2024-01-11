using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Repository.Entities;

public enum Gender
{
    Nữ = 1, Nam = 2
}

[Table("tbl_student")]
public class Student
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("major_id", TypeName = "char(26)")]
    public string MajorId { get; set; }

    public Major Major { get; set; }

    [Column("campus_id", TypeName = "char(26)")]
    public string CampusId { get; set; }

    public Campus Campus { get; set; }

    [Column("account_id", TypeName = "char(26)")]
    public string AccountId { get; set; }

    public Account Account { get; set; }

    [Column("student_card_front", TypeName = "text")]
    public string StudentCardFront { get; set; }

    [Column("file_name_front", TypeName = "text")]
    public string FileNameFront { get; set; }

    [Column("student_card_back", TypeName = "text")]
    public string StudentCardBack { get; set; }

    [Column("file_name_back", TypeName = "text")]
    public string FileNameBack { get; set; }

    [MaxLength(255)]
    [Column("full_name")]
    public string FullName { get; set; }

    [MaxLength(255)]
    [Column("code")]
    public string Code { get; set; }

    [Column("gender", TypeName = "enum('Nữ', 'Nam')")]
    public Gender? Gender { get; set; }

    [Column("date_of_birth")]
    public DateOnly? DateOfBirth { get; set; }

    [Column("address", TypeName = "text")]
    public string Address { get; set; }

    [Column("total_income", TypeName = "decimal(38,2)")]
    public decimal? TotalIncome { get; set; }

    [Column("total_spending", TypeName = "decimal(38,2)")]
    public decimal? TotalSpending { get; set; }

    [Column("date_created")]
    public DateTime? DateCreated { get; set; }

    [Column("date_updated")]
    public DateTime? DateUpdated { get; set; }

    [Column("state", TypeName = "bit(1)")]
    public bool? State { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; }

    public virtual ICollection<Activity> Activities { get; set; }

    public virtual ICollection<Wallet> Wallets { get; set; }

    public virtual ICollection<Bonus> Bonuses { get; set; }

    public virtual ICollection<Wishlist> Wishlists { get; set; }

    public virtual ICollection<Invitation> Inviters { get; set; }

    public virtual ICollection<Invitation> Invitees { get; set; }

    public virtual ICollection<StudentChallenge> StudentChallenges { get; set; }
}
