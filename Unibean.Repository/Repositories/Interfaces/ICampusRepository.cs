using Unibean.Repository.Entities;

namespace Unibean.Repository.Repositories.Interfaces;

public interface ICampusRepository
{
    Campus GetById(string id);
}
