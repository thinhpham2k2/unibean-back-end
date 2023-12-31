using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface IInvitationRepository
{
    Invitation Add(Invitation creation);
}
