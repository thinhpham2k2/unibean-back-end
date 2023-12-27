using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Service.Models.Types;

public class UpdateTypeModel
{
    [Required(ErrorMessage = "Type's name is required")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of type's name is from 3 to 255 characters")]
    public string TypeName { get; set; }

    public IFormFile Image { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }
}
