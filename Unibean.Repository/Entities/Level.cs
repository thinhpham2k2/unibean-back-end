using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

[Table("tbl_level")]
public class Level
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [MaxLength(255)]
    [Column("level_name")]
    public string LevelName { get; set; }

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

    public virtual ICollection<Product> Products { get; set; }

    public virtual ICollection<Student> Students { get; set; }
}
