using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class CampusRepository : ICampusRepository
{
    public Campus GetById(string id)
    {
        Campus campus = new();
        try
        {
            using var db = new UnibeanDBContext();
            campus = db.Campuses
            .Where(s => s.Id.Equals(id) && s.Status.Equals(true))
            .Include(s => s.University)
            .Include(s => s.Area)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return campus;
    }
}
