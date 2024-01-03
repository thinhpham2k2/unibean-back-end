using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Students;

public class UpdateStudentModel
{

    [Required(ErrorMessage = "Full name is required")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of full name is from 3 to 255 characters")]
    public string FullName { get; set; }

    [ValidGender]
    [Required(ErrorMessage = "Gender is required")]
    public string GenderId { get; set; }

    [ValidMajor]
    [Required(ErrorMessage = "Major is required")]
    public string MajorId { get; set; }

    [ValidCampus]
    [Required(ErrorMessage = "Campus is required")]
    public string CampusId { get; set; }

    public IFormFile Avatar { get; set; }

    public string Code { get; set; }

    public string Address { get; set; }

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }
}
