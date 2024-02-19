namespace Unibean.Service.Models.Wishlists;

public class WishlistModel
{
    public string Id { get; set; }
    public string StudentId { get; set; }
    public string StudentName { get; set; }
    public string StudentImage { get; set; }
    public string BrandId { get; set; }
    public string BrandName { get; set; }
    public string BrandImage { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
