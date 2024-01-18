using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Wishlists;

public class UpdateWishlistModel
{
    [ValidStudent]
    [Required(ErrorMessage = "Student is required")]
    public string StudentId { get; set; }

    [ValidBrand]
    [Required(ErrorMessage = "Brand is required")]
    public string BrandId { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }
}
