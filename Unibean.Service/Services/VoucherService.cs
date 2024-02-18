using AutoMapper;
using Enable.EnumDisplayName;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Vouchers;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class VoucherService : IVoucherService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "vouchers";

    private readonly IVoucherRepository voucherRepository;

    private readonly IFireBaseService fireBaseService;

    public VoucherService(IVoucherRepository voucherRepository, 
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Voucher, VoucherModel>()
            .ForMember(v => v.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
            .ForMember(v => v.TypeName, opt => opt.MapFrom(src => src.Type.TypeName))
            .ForMember(v => v.NumberOfItems, opt => opt.MapFrom(src => src.VoucherItems.Count))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Voucher>, PagedResultModel<VoucherModel>>()
            .ReverseMap();
            cfg.CreateMap<Voucher, VoucherExtraModel>()
            .ForMember(v => v.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
            .ForMember(v => v.TypeName, opt => opt.MapFrom(src => src.Type.TypeName))
            .ForMember(v => v.NumberOfItems, opt => opt.MapFrom(src => src.VoucherItems.Count))
            .ForMember(v => v.Campaigns, opt => opt.MapFrom(
                src => src.VoucherItems.Select(v => v.CampaignDetail.Campaign).Distinct()))
            .ReverseMap();
            cfg.CreateMap<Campaign, CampaignModel>()
            .ForMember(c => c.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
            .ForMember(c => c.BrandAcronym, opt => opt.MapFrom(src => src.Brand.Acronym))
            .ForMember(c => c.TypeName, opt => opt.MapFrom(src => src.Type.TypeName))
            .ForMember(c => c.CurrentStateId, opt => opt.MapFrom(
                src => (int)src.CampaignActivities.LastOrDefault().State))
            .ForMember(c => c.CurrentState, opt => opt.MapFrom(
                src => src.CampaignActivities.LastOrDefault().State))
            .ForMember(c => c.CurrentStateName, opt => opt.MapFrom(
                src => src.CampaignActivities.LastOrDefault().State.GetDisplayName()))
            .ReverseMap();
            cfg.CreateMap<Voucher, UpdateVoucherModel>()
            .ReverseMap()
            .ForMember(v => v.Brand, opt => opt.MapFrom(src => (string)null))
            .ForMember(v => v.Type, opt => opt.MapFrom(src => (string)null))
            .ForMember(v => v.Image, opt => opt.Ignore())
            .ForMember(v => v.ImageName, opt => opt.Ignore())
            .ForMember(v => v.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
            cfg.CreateMap<Voucher, CreateVoucherModel>()
            .ReverseMap()
            .ForMember(v => v.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(v => v.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(v => v.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(v => v.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.voucherRepository = voucherRepository;
        this.fireBaseService = fireBaseService;
    }

    public async Task<VoucherExtraModel> Add(CreateVoucherModel creation)
    {
        Voucher entity = mapper.Map<Voucher>(creation);

        //Upload image
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            entity.Image = f.URL;
            entity.ImageName = f.FileName;
        }
        return mapper.Map<VoucherExtraModel>(voucherRepository.Add(entity));
    }

    public void Delete(string id)
    {
        Voucher entity = voucherRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Image != null && entity.ImageName != null)
            {
                //Remove image
                fireBaseService.RemoveFileAsync(entity.ImageName, FOLDER_NAME);
            }
            voucherRepository.Delete(id);
        }
        else
        {
            throw new InvalidParameterException("Không tìm thấy khuyến mãi");
        }
    }

    public PagedResultModel<VoucherModel> GetAll
        (List<string> brandIds, List<string> typeIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<VoucherModel>>
            (voucherRepository.GetAll(brandIds, typeIds, state, 
            propertySort, isAsc, search, page, limit));
    }

    public VoucherExtraModel GetById(string id)
    {
        Voucher entity = voucherRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<VoucherExtraModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy khuyến mãi");
    }

    public VoucherModel GetByIdAndCampaign(string id, string campaignId)
    {
        Voucher entity = voucherRepository.GetByIdAndCampaign(id, campaignId);
        if (entity != null)
        {
            return mapper.Map<VoucherModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy khuyến mãi");
    }

    public async Task<VoucherExtraModel> Update(string id, UpdateVoucherModel update)
    {
        Voucher entity = voucherRepository.GetById(id);
        if (entity != null)
        {
            entity = mapper.Map(update, entity);
            if (update.Image != null && update.Image.Length > 0)
            {
                // Remove image
                await fireBaseService.RemoveFileAsync(entity.ImageName, FOLDER_NAME);

                //Upload new image update
                FireBaseFile f = await fireBaseService.UploadFileAsync(update.Image, FOLDER_NAME);
                entity.Image = f.URL;
                entity.ImageName = f.FileName;
            }
            return mapper.Map<VoucherExtraModel>(voucherRepository.Update(entity));
        }
        throw new InvalidParameterException("Không tìm thấy khuyến mãi");
    }
}
