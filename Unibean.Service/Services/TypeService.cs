using AutoMapper;
using Unibean.Repository.Paging;
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
            cfg.CreateMap<Type, UpdateTypeModel>().ReverseMap();
            cfg.CreateMap<Type, CreateTypeModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.typeRepository = typeRepository;
    }

    public TypeModel Add(CreateTypeModel creation)
    {
        return mapper.Map<TypeModel>(typeRepository.Add(mapper.Map<Type>(creation)));
    }

    public void Delete(string id)
    {
        throw new NotImplementedException();
    }

    public PagedResultModel<TypeModel> GetAll(string propertySort, bool isAsc, string search, int page, int limit)
    {
        throw new NotImplementedException();
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

    public TypeModel Update(string id, UpdateTypeModel update)
    {
        throw new NotImplementedException();
    }
}
