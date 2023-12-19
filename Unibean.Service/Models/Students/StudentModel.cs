namespace Unibean.Service.Models.Students;

public class StudentModel
{
    public string Id { get; set; }
    public string LevelId { get; set; }
    public string LevelName { get; set; }
    public string GenderId { get; set; }
    public string GenderName { get; set; }
    public string MajorId { get; set; }
    public string MajorName { get; set; }
    public string CampusId { get; set; }
    public string CampusName { get; set; }
    public string AccountId { get; set; }
    public string UserName { get; set; }
    public string StudentCard { get; set; }
    public string FileName { get; set; }
    public string FullName { get; set; }
    public string Code { get; set; }
    public string Email { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string Phone { get; set; }
    public string Avatar { get; set; }
    public string ImageName { get; set; }
    public string Address { get; set; }
    public decimal? TotalIncome { get; set; }
    public decimal? TotalSpending { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public DateTime? DateVerified { get; set; }
    public bool? IsVerify { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
