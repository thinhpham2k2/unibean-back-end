using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Districts;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class DistrictService : IDistrictService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "districts";

    private readonly IDistrictRepository districtRepository;

    private readonly IFireBaseService fireBaseService;

    public DistrictService(IDistrictRepository districtRepository, 
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<District, DistrictModel>()
            .ForMember(t => t.CityName, opt => opt.MapFrom(src => src.City.CityName))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<District>, PagedResultModel<DistrictModel>>()
            .ReverseMap();
            cfg.CreateMap<District, UpdateDistrictModel>()
            .ReverseMap()
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
            cfg.CreateMap<District, CreateDistrictModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.districtRepository = districtRepository;
        this.fireBaseService = fireBaseService;
    }

    public async Task<DistrictModel> Add(CreateDistrictModel creation)
    {
        District entity = mapper.Map<District>(creation);

        //Upload image
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            entity.Image = f.URL;
            entity.FileName = f.FileName;
        }
        return mapper.Map<DistrictModel>(districtRepository.Add(entity));
    }

    public void Delete(string id)
    {
        District entity = districtRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Image != null && entity.FileName != null)
            {
                //Remove image
                fireBaseService.RemoveFileAsync(entity.FileName, FOLDER_NAME);
            }
            districtRepository.Delete(id);
        }
        else
        {
            throw new InvalidParameterException("Not found district");
        }
    }

    public PagedResultModel<DistrictModel> GetAll(List<string> cityIds, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<DistrictModel>>(districtRepository.GetAll(cityIds, propertySort, isAsc, search, page, limit));
    }

    public DistrictModel GetById(string id)
    {
        District entity = districtRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<DistrictModel>(entity);
        }
        throw new InvalidParameterException("Not found district");
    }

    public async Task<DistrictModel> Update(string id, UpdateDistrictModel update)
    {
        District entity = districtRepository.GetById(id);
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
            return mapper.Map<DistrictModel>(districtRepository.Update(entity));
        }
        throw new InvalidParameterException("Not found district");
    }
}
