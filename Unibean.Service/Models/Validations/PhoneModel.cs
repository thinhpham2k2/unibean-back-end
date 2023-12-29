using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Validations;

public class PhoneModel
{
    [Phone]
    [ValidPhone]
    [Required(ErrorMessage = "Phone is required")]
    public string Phone { get; set; }
}
