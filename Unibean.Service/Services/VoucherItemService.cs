using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.VoucherItems;
using Unibean.Service.Services.Interfaces;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.Service.Services;

public class VoucherItemService : IVoucherItemService
{
    private readonly Mapper mapper;

    private readonly IVoucherItemRepository voucherItemRepository;

    public VoucherItemService(IVoucherItemRepository voucherItemRepository)
    {
        var config = new MapperConfiguration(cfg
               =>
        {
            cfg.CreateMap<VoucherItem, VoucherItemModel>()
            .ForMember(s => s.VoucherName, opt => opt.MapFrom(src => src.Voucher.VoucherName))
            .ForMember(s => s.VoucherImage, opt => opt.MapFrom(src => src.Voucher.Image))
            .ForMember(s => s.TypeId, opt => opt.MapFrom(src => src.Voucher.Type.Id))
            .ForMember(s => s.TypeName, opt => opt.MapFrom(src => src.Voucher.Type.TypeName))
            .ForMember(s => s.TypeImage, opt => opt.MapFrom(src => src.Voucher.Type.Image))
            .ForMember(s => s.StudentId, opt => opt.MapFrom(src => src.Activities.FirstOrDefault().StudentId))
            .ForMember(s => s.StudentName, opt => opt.MapFrom(src => src.Activities.FirstOrDefault().Student.FullName))
            .ForMember(s => s.BrandId, opt => opt.MapFrom(src => src.Voucher.Brand.Id))
            .ForMember(s => s.BrandName, opt => opt.MapFrom(src => src.Voucher.Brand.BrandName))
            .ForMember(s => s.BrandImage, opt => opt.MapFrom(src => src.Voucher.Brand.Account.Avatar))
            .ForMember(s => s.CampaignDetailId, opt => opt.MapFrom(src => src.CampaignDetailId))
            .ForMember(s => s.CampaignId, opt => opt.MapFrom(src => src.CampaignDetail.CampaignId))
            .ForMember(s => s.CampaignName, opt => opt.MapFrom(src => src.CampaignDetail.Campaign.CampaignName))
            .ForMember(s => s.Price, opt => opt.MapFrom(src => src.CampaignDetail.Price))
            .ForMember(s => s.Rate, opt => opt.MapFrom(src => src.CampaignDetail.Rate))
            .ForMember(s => s.DateBought, opt => opt.MapFrom(src => src.Activities.Where(a
                => a.Type.Equals(Type.Buy)).FirstOrDefault().DateCreated))
            .ForMember(s => s.DateUsed, opt => opt.MapFrom(src => src.Activities.Where(a 
                => a.Type.Equals(Type.Use)).FirstOrDefault().DateCreated))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<VoucherItem>, PagedResultModel<VoucherItemModel>>()
            .ReverseMap();
            cfg.CreateMap<VoucherItem, VoucherItemExtraModel>()
            .ForMember(s => s.VoucherName, opt => opt.MapFrom(src => src.Voucher.VoucherName))
            .ForMember(s => s.VoucherImage, opt => opt.MapFrom(src => src.Voucher.Image))
            .ForMember(s => s.TypeId, opt => opt.MapFrom(src => src.Voucher.Type.Id))
            .ForMember(s => s.TypeName, opt => opt.MapFrom(src => src.Voucher.Type.TypeName))
            .ForMember(s => s.TypeImage, opt => opt.MapFrom(src => src.Voucher.Type.Image))
            .ForMember(s => s.StudentId, opt => opt.MapFrom(src => src.Activities.FirstOrDefault().StudentId))
            .ForMember(s => s.StudentName, opt => opt.MapFrom(src => src.Activities.FirstOrDefault().Student.FullName))
            .ForMember(s => s.BrandId, opt => opt.MapFrom(src => src.Voucher.Brand.Id))
            .ForMember(s => s.BrandName, opt => opt.MapFrom(src => src.Voucher.Brand.BrandName))
            .ForMember(s => s.BrandImage, opt => opt.MapFrom(src => src.Voucher.Brand.Account.Avatar))
            .ForMember(s => s.CampaignDetailId, opt => opt.MapFrom(src => src.CampaignDetailId))
            .ForMember(s => s.CampaignId, opt => opt.MapFrom(src => src.CampaignDetail.CampaignId))
            .ForMember(s => s.CampaignName, opt => opt.MapFrom(src => src.CampaignDetail.Campaign.CampaignName))
            .ForMember(s => s.CampaignImage, opt => opt.MapFrom(src => src.CampaignDetail.Campaign.Image))
            .ForMember(s => s.Price, opt => opt.MapFrom(src => src.CampaignDetail.Price))
            .ForMember(s => s.Rate, opt => opt.MapFrom(src => src.CampaignDetail.Rate))
            .ForMember(s => s.CampaignName, opt => opt.MapFrom(src => src.CampaignDetail.Campaign.CampaignName))
            .ForMember(s => s.CampaignImage, opt => opt.MapFrom(src => src.CampaignDetail.Campaign.Image))
            .ForMember(s => s.UsedAt, opt => opt.MapFrom(src => src.Activities.Where(a
                => a.Type.Equals(Type.Use)).FirstOrDefault().Store.StoreName))
            .ForMember(s => s.DateBought, opt => opt.MapFrom(src => src.Activities.Where(a
                => a.Type.Equals(Type.Buy)).FirstOrDefault().DateCreated))
            .ForMember(s => s.DateUsed, opt => opt.MapFrom(src => src.Activities.Where(a
                => a.Type.Equals(Type.Use)).FirstOrDefault().DateCreated))
            .ForMember(s => s.Condition, opt => opt.MapFrom(src => src.Voucher.Condition))
            .ForMember(s => s.Description, opt => opt.MapFrom(src => src.Voucher.Description))
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.voucherItemRepository = voucherItemRepository;
    }

    public PagedResultModel<VoucherItemModel> GetAll
        (List<string> campaignIds, List<string> voucherIds, List<string> brandIds, 
        List<string> typeIds, List<string> studentIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<VoucherItemModel>>(voucherItemRepository.GetAll
            (campaignIds, voucherIds, brandIds, typeIds, studentIds, state, propertySort, 
            isAsc, search, page, limit));
    }

    public VoucherItemExtraModel GetById(string id)
    {
        VoucherItem entity = voucherItemRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<VoucherItemExtraModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy khuyến mãi");
    }
}
