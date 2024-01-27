using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Cities;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class CityService : ICityService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "cities";

    private readonly ICityRepository cityRepository;

    private readonly IFireBaseService fireBaseService;

    public CityService(ICityRepository cityRepository, 
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<City, CityModel>().ReverseMap();
            cfg.CreateMap<PagedResultModel<City>, PagedResultModel<CityModel>>()
            .ReverseMap();
            cfg.CreateMap<City, UpdateCityModel>()
            .ReverseMap()
            .ForMember(t => t.Image, opt => opt.Ignore())
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
            cfg.CreateMap<City, CreateCityModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.cityRepository = cityRepository;
        this.fireBaseService = fireBaseService;
    }

    public async Task<CityModel> Add(CreateCityModel creation)
    {
        City entity = mapper.Map<City>(creation);

        //Upload image
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            entity.Image = f.URL;
            entity.FileName = f.FileName;
        }
        return mapper.Map<CityModel>(cityRepository.Add(entity));
    }

    public void Delete(string id)
    {
        City entity = cityRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Image != null && entity.FileName != null)
            {
                //Remove image
                fireBaseService.RemoveFileAsync(entity.FileName, FOLDER_NAME);
            }
            cityRepository.Delete(id);
        }
        else
        {
            throw new InvalidParameterException("Not found city");
        }
    }

    public PagedResultModel<CityModel> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<CityModel>>(
            cityRepository.GetAll(state, propertySort, isAsc, search, page, limit));
    }

    public CityModel GetById(string id)
    {
        City entity = cityRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<CityModel>(entity);
        }
        throw new InvalidParameterException("Not found city");
    }

    public async Task<CityModel> Update(string id, UpdateCityModel update)
    {
        City entity = cityRepository.GetById(id);
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
            return mapper.Map<CityModel>(cityRepository.Update(entity));
        }
        throw new InvalidParameterException("Not found city");
    }
}
