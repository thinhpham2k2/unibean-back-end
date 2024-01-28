using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.OrderStates;

public class CreateOrderStateModel
{
    [ValidState]
    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public string StateId { get; set; }

    public string Description { get; set; }
}
