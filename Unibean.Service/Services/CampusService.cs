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
            cfg.CreateMap<Campus, CampusExtraModel>()
            .ForMember(c => c.UniversityName, opt => opt.MapFrom(src => src.University.UniversityName))
            .ForMember(c => c.AreaName, opt => opt.MapFrom(src => src.Area.AreaName))
            .ForMember(c => c.NumberOfStudent, opt => opt.MapFrom(src => src.Students.Count))
            .ReverseMap();
            cfg.CreateMap<Campus, UpdateCampusModel>()
            .ReverseMap()
            .ForMember(t => t.University, opt => opt.MapFrom(src => (string)null))
            .ForMember(t => t.Area, opt => opt.MapFrom(src => (string)null))
            .ForMember(t => t.Image, opt => opt.Ignore())
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

    public async Task<CampusExtraModel> Add(CreateCampusModel creation)
    {
        Campus entity = mapper.Map<Campus>(creation);

        //Upload image
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            entity.Image = f.URL;
            entity.FileName = f.FileName;
        }
        return mapper.Map<CampusExtraModel>(campusRepository.Add(entity));
    }

    public void Delete(string id)
    {
        Campus entity = campusRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Students.Count.Equals(0))
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
                throw new InvalidParameterException("Xóa thất bại do tồn tại tài khoản ở cơ sở");
            }
        }
        else
        {
            throw new InvalidParameterException("Không tìm thấy cơ sở");
        }
    }

    public PagedResultModel<CampusModel> GetAll
        (List<string> universityIds, List<string> areaIds, bool? state, 
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<CampusModel>>(campusRepository.GetAll
            (universityIds, areaIds, state, propertySort, isAsc, search, page, limit));
    }

    public PagedResultModel<CampusModel> GetAllByCampaign
        (List<string> campaignIds, List<string> universityIds, List<string> areaIds,
        bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<CampusModel>>(campusRepository.GetAllByCampaign
            (campaignIds, universityIds, areaIds, state, propertySort, isAsc, search, page, limit));
    }

    public CampusExtraModel GetById(string id)
    {
        Campus entity = campusRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<CampusExtraModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy cơ sở");
    }

    public async Task<CampusExtraModel> Update(string id, UpdateCampusModel update)
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
            return mapper.Map<CampusExtraModel>(campusRepository.Update(entity));
        }
        throw new InvalidParameterException("Không tìm thấy cơ sở");
    }
}
