using Unibean.Service.Models.OrderDetails;
using Unibean.Service.Models.OrderStates;

namespace Unibean.Service.Models.Orders;

public class OrderExtraModel
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
    public string StateCurrent { get; set; }
    public virtual ICollection<OrderDetailModel> OrderDetails { get; set; }
    public virtual ICollection<OrderStateModel> StateDetails { get; set; }
}
