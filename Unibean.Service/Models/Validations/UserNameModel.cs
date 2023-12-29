using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Validations;

public class UserNameModel
{
    [ValidUserName]
    [Required(ErrorMessage = "User name is required")]
    [StringLength(50, MinimumLength = 5,
        ErrorMessage = "The length of user name is from 5 to 50 characters")]
    public string UserName { get; set; }
}
