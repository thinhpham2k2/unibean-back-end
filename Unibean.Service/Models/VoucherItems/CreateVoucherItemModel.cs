using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.VoucherItems;

public class CreateVoucherItemModel
{
    [ValidVoucher]
    [Required(ErrorMessage = "Khuyến mãi là bắt buộc")]
    public string VoucherId { get; set; }

    [Required(ErrorMessage = "Số lượng là bắt buộc")]
    [Range(minimum: 1, 1000, ErrorMessage = "Số lượng phải lớn hơn 0 và không quá 1000")]
    public int? Quantity { get; set; }

    [JsonIgnore]
    public int? Index { get; set; }

    public string Description { get; set; }
}
