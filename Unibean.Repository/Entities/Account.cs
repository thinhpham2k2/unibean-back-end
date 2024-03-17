using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

public enum Role
{
    [Display(Name = "Quản trị viên")]
    [Description("Quản trị viên của hệ thống")]
    Admin = 1,

    [Display(Name = "Nhân viên")]
    [Description("Nhân viên quản lí trạm")]
    Staff = 2,

    [Display(Name = "Thương hiệu")]
    [Description("Quản lí thương hiệu")]
    Brand = 3,

    [Display(Name = "Cửa hàng")]
    [Description("Quản lí chi nhánh cửa hàng")]
    Store = 4,

    [Display(Name = "Sinh viên")]
    [Description("Sinh viên của các trường đại học")]
    Student = 5
}

[Table("tbl_account")]
public class Account
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("role",
        TypeName = "enum('Admin', 'Staff', 'Brand', 'Store', 'Student')")]
    public Role? Role { get; set; }

    [MaxLength(50)]
    [Column("user_name")]
    public string UserName { get; set; }

    [MaxLength(255)]
    [Column("password")]
    public string Password { get; set; }

    [Column("phone", TypeName = "char(20)")]
    public string Phone { get; set; }

    [EmailAddress]
    [MaxLength(320)]
    [Column("email")]
    public string Email { get; set; }

    [Column("avatar", TypeName = "text")]
    public string Avatar { get; set; }

    [Column("file_name", TypeName = "text")]
    public string FileName { get; set; }

    [Column("is_verify", TypeName = "bit(1)")]
    public bool? IsVerify { get; set; }

    [Column("date_created")]
    public DateTime? DateCreated { get; set; }

    [Column("date_updated")]
    public DateTime? DateUpdated { get; set; }

    [Column("date_verified")]
    public DateTime? DateVerified { get; set; }

    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    [Column("state", TypeName = "bit(1)")]
    public bool? State { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }

    public virtual ICollection<Admin> Admins { get; set; }

    public virtual ICollection<Staff> Staffs { get; set; }

    public virtual ICollection<Brand> Brands { get; set; }

    public virtual ICollection<Store> Stores { get; set; }

    public virtual ICollection<Student> Students { get; set; }
}
