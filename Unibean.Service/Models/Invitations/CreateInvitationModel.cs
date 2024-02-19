using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Entities;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Invitations;

public class CreateInvitationModel
{
    [ValidStudent(new[] { StudentState.Active })]
    [Required(ErrorMessage = "Cần có người mời")]
    public string InviterId { get; set; }

    [ValidStudent(new[] { StudentState.Active })]
    [Required(ErrorMessage = "Cần có người được mời")]
    public string InviteeId { get; set; }

    public string Description { get; set; }

    public bool? State { get; set; }
}
