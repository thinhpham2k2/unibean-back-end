using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Invitations;

public class CreateInvitationModel
{
    [ValidStudent]
    [Required(ErrorMessage = "Inviter is required")]
    public string InviterId { get; set; }

    [ValidStudent]
    [Required(ErrorMessage = "Invitee is required")]
    public string InviteeId { get; set; }

    public string Description { get; set; }

    public bool? State { get; set; }
}
