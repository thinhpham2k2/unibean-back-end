using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Entities;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.StudentChallenges;

public class CreateStudentChallengeModel
{
    [ValidStudentChallenge]
    [Required(ErrorMessage = "Thử thách là bắt buộc")]
    public string ChallengeId { get; set; }

    [ValidStudent(new[] { StudentState.Active })]
    [Required(ErrorMessage = "Sinh viên là bắt buộc")]
    public string StudentId { get; set; }

    [Required(ErrorMessage = "Điểm thưởng là bắt buộc")]
    [Range(0, (double)decimal.MaxValue, ErrorMessage = "Điểm thưởng phải dương")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Điểm thưởng phải là số thực sau dấu phẩy 2 chữ số")]
    public decimal? Amount { get; set; }

    [Required(ErrorMessage = "Chỉ số hiện tại là bắt buộc")]
    [Range(0, (double)decimal.MaxValue, ErrorMessage = "Chỉ số hiện tại phải dương")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Chỉ số hiện tại phải là số thực sau dấu phẩy 2 chữ số")]
    public decimal? Current { get; set; }

    [Required(ErrorMessage = "Điều kiện là bắt buộc")]
    [Range(0, (double)decimal.MaxValue, ErrorMessage = "Điều kiện phải dương")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Điều kiện phải là số thực sau dấu phẩy 2 chữ số")]
    public decimal? Condition { get; set; }

    [Required(ErrorMessage = "Trạng thái hoàn thành là bắt buộc")]
    public bool? IsCompleted { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
