using Unibean.Service.Models.Types;

namespace Unibean.Service.Services.Interfaces;

public interface ITypeService
{
    TypeModel GetById(string id);
}
