using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Districts;

public class CreateDistrictModel
{
    [ValidCity]
    [Required(ErrorMessage = "City is required!")]
    public string CityId { get; set; }

    [Required(ErrorMessage = "District's name is required!")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of district's name is from 3 to 255 characters")]
    public string DistrictName { get; set; }

    public IFormFile Image { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required!")]
    public bool? State { get; set; }
}
