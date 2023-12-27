﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Service.Models.Roles;

public class UpdateRoleModel
{
    [Required(ErrorMessage = "Type's name is required")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "The length of type's name is from 3 to 255 characters")]
    public string RoleName { get; set; }

    public IFormFile Image { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }
}
