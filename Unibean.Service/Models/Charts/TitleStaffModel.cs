using Unibean.Repository.Entities;

namespace Unibean.Service.Models.Charts;

public class TitleStaffModel
{
    public long? NumberOfOrders { get; set; }
    public decimal? CostOfOrders { get; set; }
    public string StationName { get; set; }
    public string StationImage { get; set; }
    public int? StationStateId { get; set; }
    public string StationState { get; set; }
    public string StationStateName { get; set; }
}
