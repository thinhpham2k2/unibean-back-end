using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Accounts;

public class CreateGoogleAccountModel
{
    [ValidRole]
    public string RoleId { get; set; }

    [ValidEmail]
    [EmailAddress]
    [Required(ErrorMessage = "Email is required!")]
    [StringLength(320, MinimumLength = 3,
            ErrorMessage = "Email is invalid")]
    public string Email { get; set; }

    [Required(ErrorMessage = "IsVerify is required!")]
    public bool? IsVerify { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required!")]
    public bool? State { get; set; }
}
