using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.VoucherItems;

public class CreateVoucherItemModel
{
    [ValidVoucher]
    [Required(ErrorMessage = "Voucher is required")]
    public string VoucherId { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int? Quantity { get; set; }

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }
}
