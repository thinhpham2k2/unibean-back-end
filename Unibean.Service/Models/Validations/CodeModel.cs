using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Validations;

public class CodeModel
{
    [ValidCode]
    [Required(ErrorMessage = "Student code is required")]
    [StringLength(50, MinimumLength = 3,
        ErrorMessage = "The length of student code is from 3 to 50 characters")]
    public string Code { get; set; }
}
