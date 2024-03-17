using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unibean.Repository.Entities;

public enum State
{
    [Display(Name = "Đặt hàng thành công")]
    [Description("Sinh viên đã đặt hàng thành công trên ứng dụng")]
    Order = 1,

    [Display(Name = "Xác nhận đơn hàng")]
    [Description("Đơn hàng đã được xác nhận với nhân viên")]
    Confirmation = 2,

    [Display(Name = "Chuẩn bị đơn hàng")]
    [Description("Nhân viên đang chuẩn bị đơn hàng")]
    Preparation = 3,

    [Display(Name = "Hàng đã tới trạm")]
    [Description("Đơn hàng đã tới trạm nhận hàng")]
    Arrival = 4,

    [Display(Name = "Đã nhận hàng")]
    [Description("Sinh viên đã nhận hàng thành công")]
    Receipt = 5,

    [Display(Name = "Đã hủy")]
    [Description("Đơn hàng đã bị hủy bỏ")]
    Abort = 6
}

[Table("tbl_order_state")]
public class OrderState
{
    [Key]
    [Column("id", TypeName = "char(26)")]
    public string Id { get; set; }

    [Column("order_id", TypeName = "char(26)")]
    public string OrderId { get; set; }

    public Order Order { get; set; }

    [Column("state",
        TypeName = "enum('Order', 'Confirmation', 'Preparation', 'Arrival', 'Receipt', 'Abort')")]
    public State? State { get; set; }

    [Column("date_created")]
    public DateTime? DateCreated { get; set; }

    [Column("description", TypeName = "text")]
    public string Description { get; set; }

    [Column("status", TypeName = "bit(1)")]
    public bool? Status { get; set; }
}
