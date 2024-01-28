namespace Unibean.Service.Models.Brands;

public class BrandExtraModel
{
    public string Id { get; set; }
    public string AccountId { get; set; }
    public string BrandName { get; set; }
    public string Acronym { get; set; }
    public string UserName { get; set; }
    public string Address { get; set; }
    public string Logo { get; set; }
    public string LogoFileName { get; set; }
    public string CoverPhoto { get; set; }
    public string CoverFileName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Link { get; set; }
    public TimeOnly? OpeningHours { get; set; }
    public TimeOnly? ClosingHours { get; set; }
    public decimal? TotalIncome { get; set; }
    public decimal? TotalSpending { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
    public int? NumberOfFollowers { get; set; }
    public bool? IsFavor { get; set; }
    public decimal? GreenWallet { get; set; }
    public string GreenWalletImage { get; set; }
    public decimal? RedWallet { get; set; }
    public string RedWalletImage { get; set; }
}
