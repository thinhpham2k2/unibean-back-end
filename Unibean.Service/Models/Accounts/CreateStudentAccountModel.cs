using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Accounts;

public class CreateStudentAccountModel
{
    [ValidUserName]
    [Required(ErrorMessage = "User name is required")]
    [StringLength(50, MinimumLength = 5,
        ErrorMessage = "The length of user name is from 5 to 50 characters")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(255, MinimumLength = 8,
        ErrorMessage = "The length of password is from 8 to 255 characters")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Password confirm is required")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string PasswordConfirmed { get; set; }

    [ValidMajor]
    [Required(ErrorMessage = "Major is required")]
    public string MajorId { get; set; }

    [ValidCampus]
    [Required(ErrorMessage = "Campus is required")]
    public string CampusId { get; set; }

    [Required(ErrorMessage = "Full name is required")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of full name is from 3 to 255 characters")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Student card front image is required")]
    public IFormFile StudentCardFront { get; set; }

    [Required(ErrorMessage = "Student card back image is required")]
    public IFormFile StudentCardBack { get; set; }

    [ValidCode]
    [Required(ErrorMessage = "Student code is required")]
    [StringLength(50, MinimumLength = 3,
        ErrorMessage = "The length of student code is from 3 to 50 characters")]
    public string Code { get; set; }

    [ValidGender]
    [Required(ErrorMessage = "Gender is required")]
    public int Gender { get; set; }

    [ValidInviteCode]
    public string InviteCode { get; set; }

    [ValidEmail]
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [ValidBirthday]
    [Required(ErrorMessage = "Date of birth is required")]
    public DateOnly? DateOfBirth { get; set; }

    [Phone]
    [ValidPhone]
    [Required(ErrorMessage = "Phone is required")]
    public string Phone { get; set; }

    public string Address { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }
}
