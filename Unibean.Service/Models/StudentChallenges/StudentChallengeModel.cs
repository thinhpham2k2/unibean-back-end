namespace Unibean.Service.Models.StudentChallenges;

public class StudentChallengeModel
{
    public string Id { get; set; }
    public string ChallengeId { get; set; }
    public string ChallengeName { get; set; }
    public string StudentId { get; set; }
    public string StudentName { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Current { get; set; }
    public decimal? Condition { get; set; }
    public bool? IsCompleted { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
