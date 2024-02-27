using AutoMapper;
using ClosedXML.Excel;
using Enable.EnumDisplayName;
using System.Data;
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

    private readonly IVoucherRepository voucherRepository;

    private readonly IVoucherItemRepository voucherItemRepository;

    public VoucherItemService(
        IVoucherRepository voucherRepository,
        IVoucherItemRepository voucherItemRepository)
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
            .ForMember(s => s.DateLocked, opt => opt.MapFrom(src => src.DateIssued))
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
            .ForMember(s => s.CampaignStateId, opt => opt.MapFrom(
                src => src.CampaignDetail.Campaign.CampaignActivities.LastOrDefault().State))
            .ForMember(s => s.CampaignState, opt => opt.MapFrom(
                src => src.CampaignDetail.Campaign.CampaignActivities.LastOrDefault().State))
            .ForMember(s => s.CampaignStateName, opt => opt.MapFrom(
                src => src.CampaignDetail.Campaign.CampaignActivities.LastOrDefault().State.GetDisplayName()))
            .ForMember(s => s.Price, opt => opt.MapFrom(src => src.CampaignDetail.Price))
            .ForMember(s => s.Rate, opt => opt.MapFrom(src => src.CampaignDetail.Rate))
            .ForMember(s => s.CampaignName, opt => opt.MapFrom(src => src.CampaignDetail.Campaign.CampaignName))
            .ForMember(s => s.CampaignImage, opt => opt.MapFrom(src => src.CampaignDetail.Campaign.Image))
            .ForMember(s => s.UsedAt, opt => opt.MapFrom(src => src.Activities.Where(a
                => a.Type.Equals(Type.Use)).FirstOrDefault().Store.StoreName))
            .ForMember(s => s.DateLocked, opt => opt.MapFrom(src => src.DateIssued))
            .ForMember(s => s.DateBought, opt => opt.MapFrom(src => src.Activities.Where(a
                => a.Type.Equals(Type.Buy)).FirstOrDefault().DateCreated))
            .ForMember(s => s.DateUsed, opt => opt.MapFrom(src => src.Activities.Where(a
                => a.Type.Equals(Type.Use)).FirstOrDefault().DateCreated))
            .ForMember(s => s.Condition, opt => opt.MapFrom(src => src.Voucher.Condition))
            .ForMember(s => s.Description, opt => opt.MapFrom(src => src.Voucher.Description))
            .ReverseMap();
            // Create Mapper CreateVoucherItemModel To VoucherItem List
            cfg.CreateMap<CreateVoucherItemModel, VoucherItem>()
            .ForMember(s => s.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(s => s.Index, opt => opt.MapFrom(src => src.Index))
            .ForMember(s => s.IsLocked, opt => opt.MapFrom(src => false))
            .ForMember(s => s.IsBought, opt => opt.MapFrom(src => false))
            .ForMember(s => s.IsUsed, opt => opt.MapFrom(src => false))
            .ForMember(s => s.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(s => s.State, opt => opt.MapFrom(src => true))
            .ForMember(s => s.Status, opt => opt.MapFrom(src => true))
            .AfterMap((src, dest) =>
            {
                dest.VoucherCode = dest.Id;
            })
            .ReverseMap();
            cfg.CreateMap<CreateVoucherItemModel, IEnumerable<VoucherItem>>()
            .ConvertUsing<VoucherItemListConverter>();
        });
        mapper = new Mapper(config);
        this.voucherRepository = voucherRepository;
        this.voucherItemRepository = voucherItemRepository;
    }

    public MemoryStream Add(CreateVoucherItemModel creation)
    {
        creation.Index = voucherItemRepository.GetMaxIndex(creation.VoucherId);
        var list = mapper.Map<IEnumerable<VoucherItem>>(creation).ToList();
        voucherItemRepository.AddList(list);

        using XLWorkbook wb = new();
        var sheet = wb.AddWorksheet
            (GetEmpdata(list, voucherRepository.GetById(creation.VoucherId).VoucherName),
            "Voucher Item Record");

        // Set style for first row
        sheet.Row(1).Style.Font.Bold = true;
        sheet.Row(1).Style.Font.FontSize = 20;

        // Set style for column A,B,C,D
        sheet.Column("A").Width = 10;
        sheet.Columns("B:C").Width = 55;
        sheet.Column("D").Width = 20;
        sheet.Column("E").Width = 55;
        sheet.Columns("A:E").Style.Font.FontSize = 15;
        sheet.Columns("A:E").Style.Alignment.WrapText = true;
        sheet.Columns("A:E").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        using MemoryStream ms = new();
        wb.SaveAs(ms);

        return ms;
    }

    public PagedResultModel<VoucherItemModel> GetAll
        (List<string> campaignIds, List<string> voucherIds, List<string> brandIds,
        List<string> typeIds, List<string> studentIds, bool? isLocked, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<VoucherItemModel>>(voucherItemRepository.GetAll
            (campaignIds, voucherIds, brandIds, typeIds, studentIds, isLocked, state,
            propertySort, isAsc, search, page, limit));
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

    private static DataTable GetEmpdata(List<VoucherItem> items, string voucherName)
    {
        var dt = new DataTable();
        dt.Columns.Add("Stt", typeof(string));
        dt.Columns.Add("Id", typeof(string));
        dt.Columns.Add("Code", typeof(string));
        dt.Columns.Add("Index", typeof(string));
        dt.Columns.Add("Name", typeof(string));
        if (items.Count > 0)
        {
            for (int i = 1; i <= items.Count; i++)
            {
                dt.Rows.Add(i, items[i - 1].Id, items[i - 1].VoucherCode, items[i - 1].Index, voucherName);
            }
        }
        return dt;
    }

    public void Delete(string id)
    {
        VoucherItem entity = voucherItemRepository.GetById(id);
        if (entity != null)
        {
            if (!(bool)entity.IsLocked && !(bool)entity.IsBought && !(bool)entity.IsUsed)
            {
                voucherItemRepository.Delete(id);
            }
            else
            {
                throw new InvalidParameterException("Không thể xóa khuyến mãi do đã được sở hữu, sử dụng hoặc đã thuộc về chiến dịch");
            }
        }
        else
        {
            throw new InvalidParameterException("Không tìm thấy khuyến mãi");
        }
    }

    public class VoucherItemListConverter :
             ITypeConverter<CreateVoucherItemModel, IEnumerable<VoucherItem>>
    {
        public IEnumerable<VoucherItem> Convert
        (CreateVoucherItemModel source, IEnumerable<VoucherItem> destination, ResolutionContext context)
        {
            for (int i = 0; i < source.Quantity; i++)
            {
                source.Index += 1;
                System.Threading.Thread.Sleep(1);
                yield return context.Mapper.Map<VoucherItem>(source);
            }
        }
    }
}
