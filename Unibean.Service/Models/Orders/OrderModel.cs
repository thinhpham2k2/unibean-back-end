namespace Unibean.Service.Models.Orders;

public class OrderModel
{
    public string Id { get; set; }
    public string StudentId { get; set; }
    public string StudentName { get; set; }
    public string StationId { get; set; }
    public string StationName { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? DateCreated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
