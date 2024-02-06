using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Entities;
using Unibean.Service.Models.OrderDetails;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Orders;

public class CreateOrderModel
{
    [ValidStation(new[] { StationState.Active })]
    [Required(ErrorMessage = "Trạm là bắt buộc")]
    public string StationId { get; set; }

    [ValidAmount]
    [Required(ErrorMessage = "Chi phí là bắt buộc")]
    [Range(minimum: 1, maximum: (double)decimal.MaxValue, ErrorMessage = "Chi phí phải là số dương")]
    public decimal? Amount { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }

    [ValidDetail]
    [Required(ErrorMessage = "Thông tin chi tiết của đơn hàng là bắt buộc")]
    public ICollection<CreateDetailModel> OrderDetails { get; set; }
}
