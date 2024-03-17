using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class InvitationRepository : IInvitationRepository
{
    private readonly UnibeanDBContext unibeanDB;

    public InvitationRepository(UnibeanDBContext unibeanDB)
    {
        this.unibeanDB = unibeanDB;
    }

    public Invitation Add(Invitation creation)
    {
        try
        {
            var db = unibeanDB;
            creation = db.Invitations.Add(creation).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return creation;
    }

    public bool ExistInvitation(string invitee)
    {
        Invitation invitation = new();
        try
        {
            var db = unibeanDB;
            invitation = db.Invitations
            .Where(s => s.InviteeId.Equals(invitee) && (bool)s.Status)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return invitation != null;
    }
}
