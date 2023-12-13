namespace Unibean.Service.Models.Genders;

public class GenderModel
{
    public string Id { get; set; }
    public string GenderName { get; set; }
    public string Image { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
