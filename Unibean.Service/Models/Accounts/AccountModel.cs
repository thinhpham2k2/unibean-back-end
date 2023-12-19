namespace Unibean.Service.Models.Accounts;

public class AccountModel
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public string UserName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Avatar { get; set; }
    public string FileName { get; set; }
    public bool? IsVerify { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public DateTime? DateVerified { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
