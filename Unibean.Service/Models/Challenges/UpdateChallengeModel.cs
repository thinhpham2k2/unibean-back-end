﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Challenges;

public class UpdateChallengeModel
{
    /// <summary>
    /// Verify = 1, Welcome = 2, Spread = 3, Consume = 4
    /// </summary>
    [ValidChallengeType]
    [Required(ErrorMessage = "Loại thử thách là bắt buộc")]
    public int? Type { get; set; }

    [Required(ErrorMessage = "Tên của thử thách là bắt buộc")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "Độ dài tên thử thách từ 3 đến 255 ký tự")]
    public string ChallengeName { get; set; }

    [Required(ErrorMessage = "Điểm thưởng là bắt buộc")]
    [Range(0, (double)decimal.MaxValue, ErrorMessage = "Điểm thưởng phải dương")]
    public decimal? Amount { get; set; }

    [Required(ErrorMessage = "Điều kiện là bắt buộc")]
    [Range(0, (double)decimal.MaxValue, ErrorMessage = "Điều kiện phải dương")]
    public decimal? Condition { get; set; }

    public IFormFile Image { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
