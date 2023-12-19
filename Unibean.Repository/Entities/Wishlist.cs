using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

[Table("tbl_wishlist")]
public class Wishlist
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("student_id", TypeName = "char(26)")]
    public string StudentId { get; set; }

    public Student Student { get; set; }

    [Column("brand_id", TypeName = "char(26)")]
    public string BrandId { get; set; }

    public Brand Brand { get; set; }

    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    [Column("state", TypeName = "bit(1)")]
    public bool? States { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }
}
