using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Validations;

public class EmailModel
{
    [ValidEmail]
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }
}
