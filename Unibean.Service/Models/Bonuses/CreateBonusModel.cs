using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Bonuses;

public class CreateBonusModel
{
    [ValidStudent]
    [Required(ErrorMessage = "Student is required")]
    public string StudentId { get; set; }

    [Required(ErrorMessage = "Amount is required")]
    [Range(minimum: 1, maximum: (double)decimal.MaxValue, ErrorMessage = "Amount must be a positive number")]
    public decimal? Amount { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [StringLength(int.MaxValue, MinimumLength = 3,
            ErrorMessage = "The length of description must be 3 characters or more")]
    public string Description { get; set; }

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }
}
