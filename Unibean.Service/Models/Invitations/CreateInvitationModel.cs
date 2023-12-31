using Unibean.Service.Validations;

namespace Unibean.Service.Models.Invitations;

public class CreateInvitationModel
{
    [ValidStudent]
    public string InviterId { get; set; }

    [ValidStudent]
    public string InviteeId { get; set; }

    public string Description { get; set; }

    public bool? State { get; set; }
}
