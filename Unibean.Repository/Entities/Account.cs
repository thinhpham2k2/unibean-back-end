﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

[Table("tbl_account")]
public class Account
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("role_id", TypeName = "char(26)")]
    public string RoleId { get; set; }

    public Role Role { get; set; }

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

    public virtual ICollection<Brand> Brands { get; set; }

    public virtual ICollection<Store> Stores { get; set; }

    public virtual ICollection<Student> Students { get; set; }
}
