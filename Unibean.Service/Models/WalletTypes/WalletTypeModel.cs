namespace Unibean.Service.Models.WalletTypes;

public class WalletTypeModel
{
    public string Id { get; set; }
    public string TypeName { get; set; }
    public string Image { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
