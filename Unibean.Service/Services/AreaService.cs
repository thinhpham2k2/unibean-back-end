using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Areas;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class AreaService : IAreaService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "areas";

    private readonly IAreaRepository areaRepository;

    private readonly IFireBaseService fireBaseService;

    public AreaService(IAreaRepository areaRepository, 
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Area, AreaModel>()
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Area>, PagedResultModel<AreaModel>>()
            .ReverseMap();
            cfg.CreateMap<Area, UpdateAreaModel>()
            .ReverseMap()
            .ForMember(t => t.Image, opt => opt.Ignore())
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
            cfg.CreateMap<Area, CreateAreaModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.areaRepository = areaRepository;
        this.fireBaseService = fireBaseService;
    }

    public async Task<AreaModel> Add(CreateAreaModel creation)
    {
        Area entity = mapper.Map<Area>(creation);

        //Upload image
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            entity.Image = f.URL;
            entity.FileName = f.FileName;
        }
        return mapper.Map<AreaModel>(areaRepository.Add(entity));
    }

    public void Delete(string id)
    {
        Area entity = areaRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Image != null && entity.FileName != null)
            {
                //Remove image
                fireBaseService.RemoveFileAsync(entity.FileName, FOLDER_NAME);
            }
            areaRepository.Delete(id);
        }
        else
        {
            throw new InvalidParameterException("Không tìm thấy khu vực");
        }
    }

    public PagedResultModel<AreaModel> GetAll
        (bool? state, string propertySort, bool isAsc, 
        string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<AreaModel>>(areaRepository.GetAll
            (state, propertySort, isAsc, search, page, limit));
    }

    public AreaModel GetById(string id)
    {
        Area entity = areaRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<AreaModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy khu vực");
    }

    public async Task<AreaModel> Update(string id, UpdateAreaModel update)
    {
        Area entity = areaRepository.GetById(id);
        if (entity != null)
        {
            entity = mapper.Map(update, entity);
            if (update.Image != null && update.Image.Length > 0)
            {
                // Remove image
                await fireBaseService.RemoveFileAsync(entity.FileName, FOLDER_NAME);

                //Upload new image update
                FireBaseFile f = await fireBaseService.UploadFileAsync(update.Image, FOLDER_NAME);
                entity.Image = f.URL;
                entity.FileName = f.FileName;
            }
            return mapper.Map<AreaModel>(areaRepository.Update(entity));
        }
        throw new InvalidParameterException("Không tìm thấy khu vực");
    }
}
