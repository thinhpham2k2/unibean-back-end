using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Repository.Entities;

public class Order
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    //[Column("student_id", TypeName = "char(26)")]
    //public string StudentId { get; set; }

    //public Student Student { get; set; }

    [Column("station_id", TypeName = "char(26)")]
    public string StationId { get; set; }

    public Station Station { get; set; }

    [Column("amount")]
    public decimal? Amount { get; set; }

    [Column("date_created")]
    public DateTime? DateCreated { get; set; }

    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    [Column("state", TypeName = "bit(1)")]
    public bool? States { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }

    public virtual ICollection<OrderState> OrderStates { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    //public virtual ICollection<OrderDetail> OrderDetails { get; set; }
}
