using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Challenges;

public class CreateChallengeModel
{
    [ValidChallengeType]
    [Required(ErrorMessage = "Challenge type is required")]
    public string TypeId { get; set; }

    [Required(ErrorMessage = "Challenge's name is required")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of challenge's name is from 3 to 255 characters")]
    public string ChallengeName { get; set; }

    [Range(0, (double)decimal.MaxValue, ErrorMessage = "The amount must be positive")]
    public decimal? Amount { get; set; }

    [Range(0, (double)decimal.MaxValue, ErrorMessage = "The condition must be positive")]
    public decimal? Condition { get; set; }

    public IFormFile Image { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }
}
