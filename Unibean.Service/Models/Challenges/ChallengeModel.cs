namespace Unibean.Service.Models.Challenges;

public class ChallengeModel
{
    public string Id { get; set; }
    public int TypeId { get; set; }
    public string Type { get; set; }
    public string TypeName { get; set; }
    public string ChallengeName { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Condition { get; set; }
    public string Image { get; set; }
    public string FileName { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
