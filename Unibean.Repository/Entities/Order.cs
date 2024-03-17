using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

[Table("tbl_order")]
public class Order
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("student_id", TypeName = "char(26)")]
    public string StudentId { get; set; }

    public Student Student { get; set; }

    [Column("station_id", TypeName = "char(26)")]
    public string StationId { get; set; }

    public Station Station { get; set; }

    [Column("amount", TypeName = "decimal(38,2)")]
    public decimal? Amount { get; set; }

    [Column("date_created")]
    public DateTime? DateCreated { get; set; }

    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    [Column("state", TypeName = "bit(1)")]
    public bool? State { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }

    public virtual ICollection<OrderState> OrderStates { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    public virtual ICollection<OrderTransaction> OrderTransactions { get; set; }
}
