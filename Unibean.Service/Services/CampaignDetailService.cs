using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.CampaignDetails;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class CampaignDetailService : ICampaignDetailService
{
    private readonly Mapper mapper;

    private readonly ICampaignDetailRepository campaignDetailRepository;

    public CampaignDetailService(
        ICampaignDetailRepository campaignDetailRepository)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            // Map Campaign Detail Model
            cfg.CreateMap<CampaignDetail, CampaignDetailModel>()
            .ForMember(c => c.VoucherName, opt => opt.MapFrom(src => src.Voucher.VoucherName))
            .ForMember(c => c.VoucherImage, opt => opt.MapFrom(src => src.Voucher.Image))
            .ForMember(c => c.CampaignName, opt => opt.MapFrom(src => src.Campaign.CampaignName))
            .ForMember(c => c.QuantityInStock, opt => opt.MapFrom(src => src.VoucherItems.Count))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<CampaignDetail>, PagedResultModel<CampaignDetailModel>>()
            .ReverseMap();
            cfg.CreateMap<CampaignDetail, CampaignDetailExtraModel>()
            .ForMember(c => c.VoucherName, opt => opt.MapFrom(src => src.Voucher.VoucherName))
            .ForMember(c => c.VoucherImage, opt => opt.MapFrom(src => src.Voucher.Image))
            .ForMember(c => c.VoucherCondition, opt => opt.MapFrom(src => src.Voucher.Condition))
            .ForMember(c => c.VoucherDescription, opt => opt.MapFrom(src => src.Voucher.Description))
            .ForMember(c => c.TypeId, opt => opt.MapFrom(src => src.Voucher.TypeId))
            .ForMember(c => c.TypeName, opt => opt.MapFrom(src => src.Voucher.Type.TypeName))
            .ForMember(c => c.CampaignName, opt => opt.MapFrom(src => src.Campaign.CampaignName))
            .ForMember(c => c.QuantityInStock, opt => opt.MapFrom(
                src => src.VoucherItems.Where(
                   v => (bool)v.IsLocked && !(bool)v.IsBought && !(bool)v.IsUsed).Count()))
            .ForMember(c => c.QuantityInBought, opt => opt.MapFrom(
                src => src.VoucherItems.Where(
                   v => (bool)v.IsLocked && (bool)v.IsBought).Count()))
            .ForMember(c => c.QuantityInUsed, opt => opt.MapFrom(
                src => src.VoucherItems.Where(
                   v => (bool)v.IsLocked && (bool)v.IsUsed).Count()))
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.campaignDetailRepository = campaignDetailRepository;
    }

    public PagedResultModel<CampaignDetailModel> GetAll
        (List<string> campaignIds, List<string> typeIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<CampaignDetailModel>>(campaignDetailRepository.GetAll
            (campaignIds, typeIds, state, propertySort, isAsc, search, page, limit));
    }

    public List<string> GetAllVoucherItemByCampaignDetail(string id)
    {
        return campaignDetailRepository.GetAllVoucherItemByCampaignDetail(id);
    }

    public CampaignDetailExtraModel GetById(string id)
    {
        CampaignDetail entity = campaignDetailRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<CampaignDetailExtraModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy chi tiết chiến dịch");
    }
}
