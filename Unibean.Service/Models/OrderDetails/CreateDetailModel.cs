using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.OrderDetails;

public class CreateDetailModel
{
    [ValidProduct]
    [Required(ErrorMessage = "Sản phẩm là bắt buộc")]
    public string ProductId { get; set; }

    [ValidQuantity]
    [Required(ErrorMessage = "Số lượng là bắt buộc")]
    [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
    public int? Quantity { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
