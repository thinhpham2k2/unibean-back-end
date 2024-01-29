using AutoMapper.Configuration.Annotations;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.Service.Models.Activities;

public class CreateBuyActivityModel
{
    [ValidStudent]
    [Required(ErrorMessage = "Sinh viên là bắt buộc")]
    public string StudentId { get; set; }

    [Ignore]
    public Type Type { get; set; } = Type.Buy;

    [ValidQuantity]
    [Required(ErrorMessage = "Số lượng là bắt buộc")]
    [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
    public int? Quantity { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
