using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Service.Models.Categories;

public class CreateCategoryModel
{
    [Required(ErrorMessage = "Category's name is required")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of category's name is from 3 to 255 characters")]
    public string CategoryName { get; set; }

    public IFormFile Image { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }
}
