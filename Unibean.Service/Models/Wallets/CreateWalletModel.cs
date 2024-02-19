using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Entities;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Wallets;

public class CreateWalletModel
{
    [ValidCampaign(new[] { CampaignState.Active })]
    public string CampaignId { get; set; }

    [ValidStudent(new[] { StudentState.Active })]
    public string StudentId { get; set; }

    [ValidBrand]
    public string BrandId { get; set; }

    /// <summary>
    /// Green = 1, Red = 2
    /// </summary>
    [ValidWalletType]
    [Required(ErrorMessage = "Loại ví là bắt buộc")]
    public int? Type { get; set; }

    public decimal? Balance { get; set; }

    public string Description { get; set; }

    public bool? State { get; set; }
}
