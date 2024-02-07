namespace Unibean.Service.Models.Students;

public class StudentModel
{
    public string Id { get; set; }
    public string MajorId { get; set; }
    public string MajorName { get; set; }
    public string UniversityId { get; set; }
    public string UniversityName { get; set; }
    public string CampusId { get; set; }
    public string CampusName { get; set; }
    public string AccountId { get; set; }
    public string UserName { get; set; }
    public string StudentCardFront { get; set; }
    public string FileNameFront { get; set; }
    public string StudentCardBack { get; set; }
    public string FileNameBack { get; set; }
    public string FullName { get; set; }
    public string Code { get; set; }
    public string Gender { get; set; }
    public string InviteCode { get; set; }
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
    public int? GreenWalletId { get; set; }
    public string GreenWallet { get; set; }
    public string GreenWalletName { get; set; }
    public decimal? GreenWalletBalance { get; set; }
    public int? RedWalletId { get; set; }
    public string RedWallet { get; set; }
    public string RedWalletName { get; set; }
    public decimal? RedWalletBalance { get; set; }
}
