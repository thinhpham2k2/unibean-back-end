using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Service.Models.Brands;

public class UpdateBrandModel
{
    [Required(ErrorMessage = "Brand's name is required")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of brand's name is from 3 to 255 characters")]
    public string BrandName { get; set; }

    public string Acronym { get; set; }

    public string Address { get; set; }

    public IFormFile Logo { get; set; }

    public IFormFile CoverPhoto { get; set; }

    public string Link { get; set; }

    public TimeOnly? OpeningHours { get; set; }

    public TimeOnly? ClosingHours { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }
}
