using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Stations;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class StationService : IStationService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "stations";

    private readonly IStationRepository stationRepository;

    private readonly IFireBaseService fireBaseService;

    public StationService(IStationRepository stationRepository, 
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Station, StationModel>()
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Station>, PagedResultModel<StationModel>>()
            .ReverseMap();
            cfg.CreateMap<Station, UpdateStationModel>()
            .ReverseMap()
            .ForMember(t => t.Image, opt => opt.Ignore())
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
            cfg.CreateMap<Station, CreateStationModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.stationRepository = stationRepository;
        this.fireBaseService = fireBaseService;
    }

    public async Task<StationModel> Add(CreateStationModel creation)
    {
        Station entity = mapper.Map<Station>(creation);

        //Upload image
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            entity.Image = f.URL;
            entity.FileName = f.FileName;
        }
        return mapper.Map<StationModel>(stationRepository.Add(entity));
    }

    public void Delete(string id)
    {
        Station entity = stationRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Image != null && entity.FileName != null)
            {
                //Remove image
                fireBaseService.RemoveFileAsync(entity.FileName, FOLDER_NAME);
            }
            stationRepository.Delete(id);
        }
        else
        {
            throw new InvalidParameterException("Not found station");
        }
    }

    public PagedResultModel<StationModel> GetAll(string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<StationModel>>(stationRepository.GetAll
            (propertySort, isAsc, search, page, limit));
    }

    public StationModel GetById(string id)
    {
        Station entity = stationRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<StationModel>(entity);
        }
        throw new InvalidParameterException("Not found station");
    }

    public async Task<StationModel> Update(string id, UpdateStationModel update)
    {
        Station entity = stationRepository.GetById(id);
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
            return mapper.Map<StationModel>(stationRepository.Update(entity));
        }
        throw new InvalidParameterException("Not found station");
    }
}
