﻿namespace Unibean.Service.Models.OrderStates;

public class OrderStateModel
{
    public string Id { get; set; }
    public string OrderId { get; set; }
    public string StateId { get; set; }
    public string StateName { get; set; }
    public DateTime? DateCreated { get; set; }
    public string Description { get; set; }
    public bool? States { get; set; }
    public bool? Status { get; set; }
}
