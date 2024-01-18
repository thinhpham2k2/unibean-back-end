﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Admins;

public class CreateAdminModel
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

    [Required(ErrorMessage = "Full name is required")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of full name is from 3 to 255 characters")]
    public string FullName { get; set; }

    public IFormFile Avatar { get; set; }

    [ValidEmail]
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [Phone]
    [ValidPhone]
    [Required(ErrorMessage = "Phone is required")]
    public string Phone { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }
}
