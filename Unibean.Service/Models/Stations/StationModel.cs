﻿namespace Unibean.Service.Models.Stations;

public class StationModel
{
    public string Id { get; set; }
    public string StationName { get; set; }
    public string Address { get; set; }
    public string Image { get; set; }
    public string FileName { get; set; }
    public TimeOnly? OpeningHours { get; set; }
    public TimeOnly? ClosingHours { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public int? StateId { get; set; }
    public string State { get; set; }
    public string StateName { get; set; }
    public bool? Status { get; set; }
}
