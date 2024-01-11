using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Products;

public class CreateProductModel
{
    [ValidCategory]
    [Required(ErrorMessage = "Category is required")]
    public string CategoryId { get; set; }

    [Required(ErrorMessage = "Product's name is required")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of product's name is from 3 to 255 characters")]
    public string ProductName { get; set; }

    public List<IFormFile> ProductImages { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(minimum: 1, maximum: double.MaxValue, ErrorMessage = "Price must be a positive number")]
    public decimal? Price { get; set; }

    [Required(ErrorMessage = "Weight is required")]
    [Range(minimum: 0, maximum: double.MaxValue, ErrorMessage = "Weight must be a positive number")]
    public decimal? Weight { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "Quantity must be greater than or equal to 0")]
    public int? Quantity { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }
}
