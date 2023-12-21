using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Service.Models.Levels;

public class CreateLevelModel
{
    [Required(ErrorMessage = " Level's name is required!")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of level's name is from 3 to 255 characters")]
    public string LevelName { get; set; }

    [Range(0, (double)decimal.MaxValue, ErrorMessage = "The condition must be positive")]
    public decimal? Condition { get; set; }

    public IFormFile Image { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required!")]
    public bool? State { get; set; }
}
