using Unibean.Service.Models.Invitations;

namespace Unibean.Service.Services.Interfaces;

public interface IInvitationService
{
    InvitationModel Add(CreateInvitationModel creation);
}
