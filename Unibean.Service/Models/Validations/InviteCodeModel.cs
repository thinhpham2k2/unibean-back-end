using Unibean.Service.Validations;

namespace Unibean.Service.Models.Validations;

public class InviteCodeModel
{
    [ValidInviteCode]
    public string InviteCode { get; set; }
}
