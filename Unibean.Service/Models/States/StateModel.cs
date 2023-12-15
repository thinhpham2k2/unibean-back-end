namespace Unibean.Service.Models.States;

public class StateModel
{
    public string Id { get; set; }
    public string StateName { get; set; }
    public string Image { get; set; }
    public string FileName { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? States { get; set; }
    public bool? Status { get; set; }
}
