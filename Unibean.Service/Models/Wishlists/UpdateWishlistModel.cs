using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Entities;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Wishlists;

public class UpdateWishlistModel
{
    [ValidStudent(new[] { StudentState.Active })]
    [Required(ErrorMessage = "Sinh viên là bắt buộc")]
    public string StudentId { get; set; }

    [ValidBrand]
    [Required(ErrorMessage = "Thương hiệu là bắt buộc")]
    public string BrandId { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
