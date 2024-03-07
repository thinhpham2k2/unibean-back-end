using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Entities;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Bonuses;

public class CreateBonusModel
{
    [ValidStudent(new[] { StudentState.Active })]
    [Required(ErrorMessage = "Sinh viên là bắt buộc")]
    public string StudentId { get; set; }

    [Required(ErrorMessage = "Chi phí là bắt buộc")]
    [Range(minimum: 1, maximum: (double)decimal.MaxValue, ErrorMessage = "Chi phí phải là số dương")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Chi phí phải là số thực sau dấu phẩy 2 chữ số")]
    public decimal? Amount { get; set; }

    [Required(ErrorMessage = "Mô tả là bắt buộc")]
    [StringLength(int.MaxValue, MinimumLength = 3,
            ErrorMessage = "Độ dài mô tả phải từ 3 ký tự trở lên")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
