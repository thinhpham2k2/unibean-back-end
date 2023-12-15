using AutoMapper;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Types;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.Service.Services;

public class TypeService : ITypeService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "types";

    private readonly ITypeRepository typeRepository;

    private readonly IFireBaseService fireBaseService;

    public TypeService(ITypeRepository typeRepository,
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Type, TypeModel>().ReverseMap();
            cfg.CreateMap<Type, UpdateTypeModel>()
            .ReverseMap()
            .ForMember(t => t.Image, opt => opt.Ignore())
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
            cfg.CreateMap<Type, CreateTypeModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.Image, opt => opt.Ignore())
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.typeRepository = typeRepository;
        this.fireBaseService = fireBaseService;
    }

    public async Task<TypeModel> Add(CreateTypeModel creation)
    {
        Type entity = mapper.Map<Type>(creation);
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            entity.Image = f.URL;
            entity.FileName = f.FileName;
        }
        return mapper.Map<TypeModel>(typeRepository.Add(entity));
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

    public async Task<TypeModel> Update(string id, UpdateTypeModel update)
    {
        Type entity = typeRepository.GetById(id);
        if (entity != null)
        {
            entity = mapper.Map(update, entity);
            if (update.Image != null && update.Image.Length > 0)
            {
                await fireBaseService.RemoveFileAsync(entity.FileName, FOLDER_NAME);
                FireBaseFile f = await fireBaseService.UploadFileAsync(update.Image, FOLDER_NAME);
                entity.Image = f.URL;
                entity.FileName = f.FileName;
            }
            return mapper.Map<TypeModel>(typeRepository.Update(entity));
        }
        throw new InvalidParameterException("Not found activity's type");
    }
}
