using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

public enum ChallengeType
{
    [Display(Name = "Xác minh")]
    [Description("Xác minh tài khoản sinh viên")]
    Verify = 1,

    [Display(Name = "Chào mừng")]
    [Description("Chào mừng đến với ứng dụng")]
    Welcome = 2,

    [Display(Name = "Lan tỏa")]
    [Description("Lan tỏa ứng dụng đến nhiều người")]
    Spread = 3,

    [Display(Name = "Tiêu thụ")]
    [Description("Tiêu thụ đậu cho việc mua khuyến mãi")]
    Consume = 4
}

[Table("tbl_challenge")]
public class Challenge
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("type", TypeName = "enum('Verify', 'Welcome', 'Spread', 'Consume')")]
    public ChallengeType? Type { get; set; }

    [MaxLength(255)]
    [Column("challenge_name")]
    public string ChallengeName { get; set; }

    [Column("amount", TypeName = "decimal(38,2)")]
    public decimal? Amount { get; set; }

    [Column("condition", TypeName = "decimal(38,2)")]
    public decimal? Condition { get; set; }

    [Column("image", TypeName = "text")]
    public string Image { get; set; }

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

    public virtual ICollection<StudentChallenge> StudentChallenges { get; set; }
}
