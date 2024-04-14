using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.OrderStates;

public class CreateOrderStateModel
{
    /// <summary>
    /// Order = 1, Confirmation = 2, Preparation = 3, Arrival = 4, Receipt = 5, Abort = 6
    /// </summary>
    [ValidState]
    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public int? State { get; set; }

    public string Note { get; set; }
}
