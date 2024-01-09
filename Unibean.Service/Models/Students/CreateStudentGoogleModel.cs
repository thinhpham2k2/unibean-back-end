using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Students;

public class CreateStudentGoogleModel
{
    [ValidGender]
    [Required(ErrorMessage = "Gender is required")]
    public string GenderId { get; set; }

    [ValidMajor]
    [Required(ErrorMessage = "Major is required")]
    public string MajorId { get; set; }

    [ValidCampus]
    [Required(ErrorMessage = "Campus is required")]
    public string CampusId { get; set; }

    [ValidAccount]
    [Required(ErrorMessage = "Account is required")]
    public string AccountId { get; set; }

    [Required(ErrorMessage = "Student card front image is required")]
    public IFormFile StudentCardFront { get; set; }

    [Required(ErrorMessage = "Student card back image is required")]
    public IFormFile StudentCardBack { get; set; }

    [Required(ErrorMessage = "Full name is required")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of full name is from 3 to 255 characters")]
    public string FullName { get; set; }

    [ValidCode]
    [Required(ErrorMessage = "Student code is required")]
    [StringLength(50, MinimumLength = 3,
        ErrorMessage = "The length of student code is from 3 to 50 characters")]
    public string Code { get; set; }

    [ValidInviteCode]
    public string InviteCode { get; set; }

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

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }
}
