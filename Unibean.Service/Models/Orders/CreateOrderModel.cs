using System.ComponentModel.DataAnnotations;
using Unibean.Service.Models.OrderDetails;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Orders;

public class CreateOrderModel
{
    [ValidStudent]
    [Required(ErrorMessage = "Student is required")]
    public string StudentId { get; set; }

    [ValidStation]
    [Required(ErrorMessage = "Station is required")]
    public string StationId { get; set; }

    [ValidAmount]
    [Required(ErrorMessage = "Amount is required")]
    [Range(minimum: 1, maximum: (double)decimal.MaxValue, ErrorMessage = "Amount must be a positive number")]
    public decimal? Amount { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }

    [Required(ErrorMessage = "Order's details is required")]
    public List<CreateDetailModel> OrderDetails { get; set; }
}
