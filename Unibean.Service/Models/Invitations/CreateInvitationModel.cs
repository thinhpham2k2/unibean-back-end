using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Invitations;

public class CreateInvitationModel
{
    [ValidStudent]
    [Required(ErrorMessage = "Cần có người mời")]
    public string InviterId { get; set; }

    [ValidStudent]
    [Required(ErrorMessage = "Cần có người được mời")]
    public string InviteeId { get; set; }

    public string Description { get; set; }

    public bool? State { get; set; }
}
