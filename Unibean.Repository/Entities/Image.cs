using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Repository.Entities;

[Table("tbl_image")]
public class Image
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("product_id", TypeName = "char(26)")]
    public string ProductId { get; set; }

    public Product Product { get; set; }

    [Column("url", TypeName = "text")]
    public string Url { get; set; }

    [Column("file_name", TypeName = "text")]
    public string FileName { get; set; }

    [Column("is_cover", TypeName = "bit(1)")]
    public bool? IsCover { get; set; }

    [Column("date_created")]
    public DateTime? DateCreated { get; set; }

    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    [Column("state", TypeName = "bit(1)")]
    public bool? States { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }
}
