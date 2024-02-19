using AutoMapper;
using Enable.EnumDisplayName;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.CampaignActivities;
using Unibean.Service.Models.CampaignDetails;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class CampaignActivityService : ICampaignActivityService
{
    private readonly IMapper mapper;

    private readonly ICampaignActivityRepository campaignActivityRepository;

    public CampaignActivityService(ICampaignActivityRepository campaignActivityRepository)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            // Map Campaign Activity Model
            cfg.CreateMap<CampaignActivity, CampaignActivityModel>()
            .ForMember(c => c.CampaignName, opt => opt.MapFrom(src => src.Campaign.CampaignName))
            .ForMember(c => c.StateId, opt => opt.MapFrom(src => src.State))
            .ForMember(c => c.StateName, opt => opt.MapFrom(src => src.State.GetDisplayName()))
            .ForMember(c => c.StateDescription, opt => opt.MapFrom(src => src.State.GetEnumDescription()))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<CampaignActivity>, PagedResultModel<CampaignActivityModel>>()
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.campaignActivityRepository = campaignActivityRepository;
    }

    public PagedResultModel<CampaignActivityModel> GetAll
        (List<string> campaignIds, List<CampaignState> stateIds, 
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<CampaignActivityModel>>(campaignActivityRepository.GetAll
            (campaignIds, stateIds, propertySort, isAsc, search, page, limit));
    }

    public CampaignActivityModel GetById(string id)
    {
        CampaignActivity entity = campaignActivityRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<CampaignActivityModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy hoạt động chiến dịch");
    }
}
