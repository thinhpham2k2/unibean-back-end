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
    public decimal? Amount { get; set; }

    [Required(ErrorMessage = "Chỉ số hiện tại là bắt buộc")]
    [Range(0, (double)decimal.MaxValue, ErrorMessage = "Chỉ số hiện tại phải dương")]
    public decimal? Current { get; set; }

    [Required(ErrorMessage = "Điều kiện là bắt buộc")]
    [Range(0, (double)decimal.MaxValue, ErrorMessage = "Điều kiện phải dương")]
    public decimal? Condition { get; set; }

    [Required(ErrorMessage = "Trạng thái hoàn thành là bắt buộc")]
    public bool? IsCompleted { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
