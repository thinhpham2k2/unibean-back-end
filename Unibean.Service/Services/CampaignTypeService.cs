using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.CampaignTypes;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class CampaignTypeService : ICampaignTypeService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "campaignTypes";

    private readonly ICampaignTypeRepository campaignTypeRepository;

    private readonly IFireBaseService fireBaseService;

    public CampaignTypeService(ICampaignTypeRepository campaignTypeRepository, 
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<CampaignType, CampaignTypeModel>().ReverseMap();
            cfg.CreateMap<PagedResultModel<CampaignType>, PagedResultModel<CampaignTypeModel>>()
            .ReverseMap();
            cfg.CreateMap<CampaignType, UpdateCampaignTypeModel>()
            .ReverseMap()
            .ForMember(t => t.Image, opt => opt.Ignore())
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
            cfg.CreateMap<CampaignType, CreateCampaignTypeModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.campaignTypeRepository = campaignTypeRepository;
        this.fireBaseService = fireBaseService;
    }

    public async Task<CampaignTypeModel> Add(CreateCampaignTypeModel creation)
    {
        CampaignType entity = mapper.Map<CampaignType>(creation);

        //Upload image
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            entity.Image = f.URL;
            entity.FileName = f.FileName;
        }
        return mapper.Map<CampaignTypeModel>(campaignTypeRepository.Add(entity));
    }

    public void Delete(string id)
    {
        CampaignType entity = campaignTypeRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Image != null && entity.FileName != null)
            {
                //Remove image
                fireBaseService.RemoveFileAsync(entity.FileName, FOLDER_NAME);
            }
            campaignTypeRepository.Delete(id);
        }
        else
        {
            throw new InvalidParameterException("Not found campaign type");
        }
    }

    public PagedResultModel<CampaignTypeModel> GetAll(string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<CampaignTypeModel>>(campaignTypeRepository.GetAll(propertySort, isAsc, search, page, limit));
    }

    public CampaignTypeModel GetById(string id)
    {
        CampaignType entity = campaignTypeRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<CampaignTypeModel>(entity);
        }
        throw new InvalidParameterException("Not found campaign type");
    }

    public async Task<CampaignTypeModel> Update(string id, UpdateCampaignTypeModel update)
    {
        CampaignType entity = campaignTypeRepository.GetById(id);
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
            return mapper.Map<CampaignTypeModel>(campaignTypeRepository.Update(entity));
        }
        throw new InvalidParameterException("Not found campaign type");
    }
}
