namespace Unibean.Service.Models.Products;

public class ProductModel
{
    public string Id { get; set; }
    public string CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string LevelId { get; set; }
    public string LevelName { get; set; }
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public decimal? Price { get; set; }
    public decimal? Weight { get; set; }
    public int? Quantity { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
