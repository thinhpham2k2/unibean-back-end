namespace Unibean.Service.Models.Wishlists;

public class WishlistModel
{
    public string Id { get; set; }
    public string StudentId { get; set; }
    public string StudentName { get; set; }
    public string PartnerId { get; set; }
    public string BrandName { get; set; }
    public string Description { get; set; }
    public bool? States { get; set; }
    public bool? Status { get; set; }
}
