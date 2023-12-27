using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Campuses;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class CampusService : ICampusService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "campuses";

    private readonly ICampusRepository campusRepository;

    private readonly IFireBaseService fireBaseService;

    public CampusService(ICampusRepository campusRepository, 
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Campus, CampusModel>()
            .ForMember(c => c.UniversityName, opt => opt.MapFrom(src => src.University.UniversityName))
            .ForMember(c => c.AreaName, opt => opt.MapFrom(src => src.Area.AreaName))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Campus>, PagedResultModel<CampusModel>>()
            .ReverseMap();
            cfg.CreateMap<Campus, UpdateCampusModel>()
            .ReverseMap()
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
            cfg.CreateMap<Campus, CreateCampusModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.campusRepository = campusRepository;
        this.fireBaseService = fireBaseService;
    }

    public async Task<CampusModel> Add(CreateCampusModel creation)
    {
        Campus entity = mapper.Map<Campus>(creation);

        //Upload image
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            entity.Image = f.URL;
            entity.FileName = f.FileName;
        }
        return mapper.Map<CampusModel>(campusRepository.Add(entity));
    }

    public void Delete(string id)
    {
        Campus entity = campusRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Image != null && entity.FileName != null)
            {
                //Remove image
                fireBaseService.RemoveFileAsync(entity.FileName, FOLDER_NAME);
            }
            campusRepository.Delete(id);
        }
        else
        {
            throw new InvalidParameterException("Not found campus");
        }
    }

    public PagedResultModel<CampusModel> GetAll(List<string> universityIds, List<string> areaIds, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<CampusModel>>(campusRepository.GetAll(universityIds, areaIds, propertySort, isAsc, search, page, limit));
    }

    public CampusModel GetById(string id)
    {
        Campus entity = campusRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<CampusModel>(entity);
        }
        throw new InvalidParameterException("Not found campus");
    }

    public async Task<CampusModel> Update(string id, UpdateCampusModel update)
    {
        Campus entity = campusRepository.GetById(id);
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
            return mapper.Map<CampusModel>(campusRepository.Update(entity));
        }
        throw new InvalidParameterException("Not found campus");
    }
}
