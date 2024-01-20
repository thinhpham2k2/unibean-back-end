using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class CampaignService : ICampaignService
{
    private readonly Mapper mapper;

    private readonly ICampaignRepository campaignRepository;

    public CampaignService(ICampaignRepository campaignRepository)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Campaign, CampaignModel>()
            .ForMember(c => c.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
            .ForMember(c => c.BrandAcronym, opt => opt.MapFrom(src => src.Brand.Acronym))
            .ForMember(c => c.TypeName, opt => opt.MapFrom(src => src.Type.TypeName))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Campaign>, PagedResultModel<CampaignModel>>()
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.campaignRepository = campaignRepository;
    }

    public Task<CampaignExtraModel> Add(CreateCampaignModel creation)
    {
        throw new NotImplementedException();
    }

    public void Delete(string id)
    {
        throw new NotImplementedException();
    }

    public PagedResultModel<CampaignModel> GetAll
        (List<string> brandIds, List<string> typeIds, List<string> storeIds, List<string> majorIds,
        List<string> campusIds, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<CampaignModel>>(campaignRepository
            .GetAll(brandIds, typeIds, storeIds, majorIds, campusIds, propertySort, isAsc, search, page, limit));
    }

    public CampaignExtraModel GetById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<CampaignExtraModel> Update(string id, UpdateCampaignModel update)
    {
        throw new NotImplementedException();
    }
}
