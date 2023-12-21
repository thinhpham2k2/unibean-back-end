using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Students;

public class CreateGoogleStudentModel
{
    [ValidGender]
    public string GenderId { get; set; }

    [ValidMajor]
    public string MajorId { get; set; }

    [ValidCampus]
    public string CampusId { get; set; }

    [ValidAccount]
    public string AccountId { get; set; }

    [Required(ErrorMessage = "Student card image is required!")]
    public IFormFile StudentCard { get; set; }

    [Required(ErrorMessage = "Full name is required!")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of full name is from 3 to 255 characters")]
    public string FullName { get; set; }

    public string Code { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email is required!")]
    public string Email { get; set; }

    [ValidBirthday]
    public DateOnly? DateOfBirth { get; set; }

    [Required(ErrorMessage = "Phone is required!")]
    public string Phone { get; set; }

    public string Address { get; set; }

    [Required(ErrorMessage = "State is required!")]
    public bool? State { get; set; }
}
