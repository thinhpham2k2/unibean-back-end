using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Repository.Entities;

public class Product
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("category_id", TypeName = "char(26)")]
    public string CategoryId { get; set; }

    public Category Category { get; set; }

    [MaxLength(255)]
    [Column("product_name")]
    public string ProductName { get; set; }

    [Column("price")]
    public decimal? Price { get; set; }

    [Column("weight")]
    public decimal? Weight { get; set; }

    [Column("quantity")]
    public int? Quantity { get; set; }

    [Column("date_created")]
    public DateTime? DateCreated { get; set; }

    [Column("date_updated")]
    public DateTime? DateUpdated { get; set; }

    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    [Column("state", TypeName = "bit(1)")]
    public bool? States { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }

    public virtual ICollection<Image> Images { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
}
