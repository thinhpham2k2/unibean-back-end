using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Stores;

public class UpdateStoreModel
{
    [ValidArea]
    [Required(ErrorMessage = "Area is required")]
    public string AreaId { get; set; }

    [Required(ErrorMessage = "Store name is required")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of store name is from 3 to 255 characters")]
    public string StoreName { get; set; }

    [Required(ErrorMessage = "Address is required")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of address is from 3 to 255 characters")]
    public string Address { get; set; }

    public IFormFile Avatar { get; set; }

    public TimeOnly? OpeningHours { get; set; }

    public TimeOnly? ClosingHours { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }
}
