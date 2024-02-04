namespace Unibean.Service.Models.Staffs;

public class StaffModel
{
    public string Id { get; set; }
    public string StationId { get; set; }
    public string StationName { get; set; }
    public string AccountId { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Avatar { get; set; }
    public string FileName { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
