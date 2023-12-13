namespace Unibean.Service.Models.OrderDetails;

public class OrderDetailModel
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public string OrderId { get; set; }
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
    public decimal? Amount { get; set; }
    public bool? States { get; set; }
    public bool? Status { get; set; }
}
