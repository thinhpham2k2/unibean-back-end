using AutoMapper;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Types;
using Unibean.Service.Services.Interfaces;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.Service.Services;

public class TypeService : ITypeService
{
    private readonly Mapper mapper;

    private readonly ITypeRepository typeRepository;

    public TypeService(ITypeRepository typeRepository)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Type, TypeModel>().ReverseMap();
        });
        mapper = new Mapper(config);
        this.typeRepository = typeRepository;
    }

    public TypeModel GetById(string id)
    {
        Type entity = typeRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<TypeModel>(entity);
        }
        throw new InvalidParameterException("Not found activity's type");
    }
}
