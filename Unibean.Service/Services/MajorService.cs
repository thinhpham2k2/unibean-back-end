using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Majors;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class MajorService : IMajorService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "majors";

    private readonly IMajorRepository majorRepository;

    private readonly IFireBaseService fireBaseService;

    public MajorService(IMajorRepository majorRepository, 
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Major, MajorModel>().ReverseMap();
            cfg.CreateMap<PagedResultModel<Major>, PagedResultModel<MajorModel>>()
            .ReverseMap();
            cfg.CreateMap<Major, UpdateMajorModel>()
            .ReverseMap()
            .ForMember(t => t.Image, opt => opt.Ignore())
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
            cfg.CreateMap<Major, CreateMajorModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.majorRepository = majorRepository;
        this.fireBaseService = fireBaseService;
    }

    public async Task<MajorModel> Add(CreateMajorModel creation)
    {
        Major entity = mapper.Map<Major>(creation);

        //Upload image
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            entity.Image = f.URL;
            entity.FileName = f.FileName;
        }
        return mapper.Map<MajorModel>(majorRepository.Add(entity));
    }

    public void Delete(string id)
    {
        Major entity = majorRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Image != null && entity.FileName != null)
            {
                //Remove image
                fireBaseService.RemoveFileAsync(entity.FileName, FOLDER_NAME);
            }
            majorRepository.Delete(id);
        }
        else
        {
            throw new InvalidParameterException("Not found major");
        }
    }

    public PagedResultModel<MajorModel> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<MajorModel>>(
            majorRepository.GetAll(state, propertySort, isAsc, search, page, limit));
    }

    public PagedResultModel<MajorModel> GetAllByCampaign
        (List<string> campaignIds, bool? state, string propertySort, 
        bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<MajorModel>>
            (majorRepository.GetAllByCampaign(campaignIds, state, 
            propertySort, isAsc, search, page, limit));
    }

    public MajorModel GetById(string id)
    {
        Major entity = majorRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<MajorModel>(entity);
        }
        throw new InvalidParameterException("Not found major");
    }

    public async Task<MajorModel> Update(string id, UpdateMajorModel update)
    {
        Major entity = majorRepository.GetById(id);
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
            return mapper.Map<MajorModel>(majorRepository.Update(entity));
        }
        throw new InvalidParameterException("Not found major");
    }
}
