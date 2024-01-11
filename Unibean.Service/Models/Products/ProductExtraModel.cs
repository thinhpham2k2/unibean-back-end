namespace Unibean.Service.Models.Products;

public class ProductExtraModel
{
    public string Id { get; set; }
    public string CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string CategoryImage { get; set; }
    public string ProductName { get; set; }
    public List<string> ProductImages { get; set; }
    public decimal? Price { get; set; }
    public decimal? Weight { get; set; }
    public int? Quantity { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
    public int? NumOfSold { get; set; }
}
