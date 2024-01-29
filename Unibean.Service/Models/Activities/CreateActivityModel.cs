using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.Service.Models.Activities;

public class CreateActivityModel
{
    public string StoreId { get; set; }

    [ValidStudent]
    [Required(ErrorMessage = "Sinh viên là bắt buộc")]
    public string StudentId { get; set; }

    [Required(ErrorMessage = "Sinh viên là bắt buộc")]
    public string VoucherItemId { get; set; }

    [ValidType]
    [Required(ErrorMessage = "Loại hoạt động là bắt buộc")]
    public Type Type { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
