using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Repository.Entities;

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

    [Column("price")]
    public decimal? Price { get; set; }

    [Column("quantity")]
    public int? Quantity { get; set; }

    [Column("amount")]
    public decimal? Amount { get; set; }

    [Column("state", TypeName = "bit(1)")]
    public bool? States { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }
}
