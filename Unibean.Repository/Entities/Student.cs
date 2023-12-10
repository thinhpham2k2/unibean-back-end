using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Unibean.Repository.Entities;

[Table("tbl_student")]
public class Student
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("gender_id", TypeName = "char(26)")]
    public string GenderId { get; set; }

    public Gender Gender { get; set; }

    [Column("major_id", TypeName = "char(26)")]
    public string MajorId { get; set; }

    public Major Major { get; set; }

    [Column("campus_id", TypeName = "char(26)")]
    public string CampusId { get; set; }

    public Campus Campus { get; set; }

    [MaxLength(50)]
    [Column("user_name")]
    public string UserName { get; set; }

    [MaxLength(255)]
    [Column("password")]
    public string Password { get; set; }

    [Column("student_card", TypeName = "text")]
    public string StudentCard { get; set; }

    [MaxLength(255)]
    [Column("full_name")]
    public string FullName { get; set; }

    [MaxLength(255)]
    [Column("code")]
    public string Code { get; set; }

    [EmailAddress]
    [MaxLength(320)]
    [Column("email")]
    public string Email { get; set; }

    [Column("date_of_birth")]
    public DateOnly? DateOfBirth { get; set; }

    [Column("phone", TypeName = "char(20)")]
    public string Phone { get; set; }

    [Column("avatar", TypeName = "text")]
    public string Avatar { get; set; }

    [Column("address", TypeName = "text")]
    public string Address { get; set; }

    [Column("date_created")]
    public DateTime? DateCreated { get; set; }

    [Column("date_updated")]
    public DateTime? DateUpdated { get; set; }

    [Column("date_verified")]
    public DateTime? DateVerified { get; set; }

    [Column("is_verify", TypeName = "bit(1)")]
    public bool? IsVerify { get; set; }

    [Column("state", TypeName = "bit(1)")]
    public bool? State { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; }

    //public virtual ICollection<Order> Orders { get; set; }

    //public virtual ICollection<Order> Orders { get; set; }

    //public virtual ICollection<Order> Orders { get; set; }

    //public virtual ICollection<Order> Orders { get; set; }
}
