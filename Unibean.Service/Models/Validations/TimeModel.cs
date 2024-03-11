using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Validations;

public class TimeModel
{
    [ValidStartOn]
    [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
    public DateOnly? StartOn { get; set; }

    [ValidEndOn]
    [Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
    public DateOnly? EndOn { get; set; }
}
