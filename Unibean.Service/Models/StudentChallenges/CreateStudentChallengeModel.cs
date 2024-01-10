using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.StudentChallenges;

public class CreateStudentChallengeModel
{
    [ValidStudentChallenge]
    [Required(ErrorMessage = "Student challenge is required")]
    public string ChallengeId { get; set; }

    [ValidStudent]
    [Required(ErrorMessage = "Student is required")]
    public string StudentId { get; set; }

    [Required(ErrorMessage = "Amount is required")]
    [Range(0, (double)decimal.MaxValue, ErrorMessage = "The amount must be positive")]
    public decimal? Amount { get; set; }

    [Required(ErrorMessage = "Current is required")]
    [Range(0, (double)decimal.MaxValue, ErrorMessage = "The current must be positive")]
    public decimal? Current { get; set; }

    [Required(ErrorMessage = "Condition is required")]
    [Range(0, (double)decimal.MaxValue, ErrorMessage = "The condition must be positive")]
    public decimal? Condition { get; set; }

    [Required(ErrorMessage = "Complete state is required")]
    public bool? IsCompleted { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }
}
