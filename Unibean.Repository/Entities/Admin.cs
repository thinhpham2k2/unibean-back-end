using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;


[Table("tbl_admin")]
public class Admin
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [MaxLength(50)]
    [Column("user_name")]
    public string UserName { get; set; }

    [MaxLength(255)]
    [Column("password")]
    public string Password { get; set; }

    [MaxLength(255)]
    [Column("full_name")]
    public string FullName { get; set; }

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

    [Column("date_created")]
    public DateTime? DateCreated { get; set; }

    [Column("date_updated")]
    public DateTime? DateUpdated { get; set; }

    [Column("state", TypeName = "bit(1)")]
    public bool? State { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }

    public virtual ICollection<Request> Requests { get; set; }
}
