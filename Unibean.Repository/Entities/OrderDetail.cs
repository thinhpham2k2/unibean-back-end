using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

[Table("tbl_order_detail")]
public class OrderDetail
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("product_id", TypeName = "char(26)")]
    public string ProductId { get; set; }

    public Product Product { get; set; }

    [Column("order_id", TypeName = "char(26)")]
    public string OrderId { get; set; }

    public Order Order { get; set; }

    [Column("price", TypeName = "decimal(38,2)")]
    public decimal? Price { get; set; }

    [Column("quantity")]
    public int? Quantity { get; set; }

    [Column("amount", TypeName = "decimal(38,2)")]
    public decimal? Amount { get; set; }

    [Column("state", TypeName = "bit(1)")]
    public bool? State { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }
}
