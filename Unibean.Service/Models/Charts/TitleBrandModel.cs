using Enable.EnumDisplayName;
using Unibean.Repository.Entities;

namespace Unibean.Service.Models.Charts;

public class TitleBrandModel
{
    public long? NumberOfCampagins { get; set; }
    public long? NumberOfStores { get; set; }
    public long? NumberOfVoucherItems { get; set; }
    public decimal? Balance { get; set; }
    public int? BrandWalletTypeId { get; set; } = (int)WalletType.Green;
    public string BrandWalletType { get; set; } = WalletType.Green.ToString();
    public string BrandWalletTypeName { get; set; } = WalletType.Green.GetDisplayName();
}
