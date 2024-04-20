using AutoMapper;
using ClosedXML.Excel;
using Enable.EnumDisplayName;
using MoreLinq;
using System.Data;
using System.Text.RegularExpressions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Authens;
using Unibean.Service.Models.Files;
using Unibean.Service.Models.VoucherItems;
using Unibean.Service.Services.Interfaces;
using InvalidParameterException = Unibean.Service.Models.Exceptions.InvalidParameterException;
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

        return CreateResult(GetEmpdata(list, voucherRepository.GetById(creation.VoucherId).VoucherName));
    }

    public PagedResultModel<VoucherItemModel> GetAll
        (List<string> campaignIds, List<string> campaignDetailIds, List<string> voucherIds, List<string> brandIds,
        List<string> typeIds, List<string> studentIds, bool? isLocked, bool? isBought, bool? isUsed, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<VoucherItemModel>>(voucherItemRepository.GetAll
            (campaignIds, campaignDetailIds, voucherIds, brandIds, typeIds, studentIds, isLocked, isBought,
            isUsed, state, propertySort, isAsc, search, page, limit));
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

    private static void RemoveFile(string filePath)
    {
        try
        {
            // Check if file exists with its full path
            if (Directory.Exists(filePath))
            {
                // If file found, delete it
                Directory.Delete(filePath, true);
                Console.WriteLine("File deleted");
            }
            else Console.WriteLine("File not found");
        }
        catch (IOException ioExp)
        {
            Console.WriteLine(ioExp.Message);
        }
    }

    private static MemoryStream CreateResult(DataTable dt)
    {
        using XLWorkbook wb = new();
        var sheet = wb.AddWorksheet("Voucher Item Record");
        sheet.Cell("A2").InsertTable(dt)
            .Theme = XLTableTheme.TableStyleMedium4;
        sheet.Protect("unibean123");

        // Set for cell
        sheet.Cell("A1").Value = "          *Lưu ý\r\n     - Stt: Số thứ tự của khuyến mãi.\r\n     - Id: Định danh của khuyến mãi.\r\n     - Code: Mã quét của khuyến mãi.\r\n     - Index: Chỉ mục của khuyến mãi (nâng cao).\r\n     - Name: Tên của khuyến mãi.";

        // Set style for second row
        sheet.Row(1).Height = 130;
        sheet.Row(2).Style.Font.Bold = true;
        sheet.Row(2).Style.Font.FontSize = 20;

        // Set style for column A,B,C,D
        sheet.Column("A").Width = 10;
        sheet.Columns("B:C").Width = 55;
        sheet.Column("D").Width = 20;
        sheet.Column("E").Width = 55;
        sheet.Columns("A:E").Style.Font.FontSize = 15;
        sheet.Columns("A:E").Style.Alignment.WrapText = true;
        sheet.Columns("A:E").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        sheet.Columns("F:XFD").Hide();

        // Set style for range
        sheet.Range("A1:E1").Merge();
        sheet.Range("A1:E1").Style.Font.Bold = true;
        sheet.Range("A1:E1").Style.Font.Italic = true;
        sheet.Range("A1:E1").Style.Font.FontColor = XLColor.DavysGrey;

        using MemoryStream ms = new();
        wb.SaveAs(ms);

        return ms;
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

    public MemoryStream GetTemplateVoucherItem()
    {
        var dt = new DataTable();
        dt.Columns.Add("Stt", typeof(string));
        dt.Columns.Add("Code", typeof(string));
        dt.Rows.Add("0", "Ví dụ: '01HQJE9MKJ8SNT5XH2Q3YCGCY4' *Độ dài phải có độ dài từ 3 - 50 kí tự và không chứa khoảng trắng");

        for (int i = 1; i <= 1000; i++)
        {
            dt.Rows.Add(i);
        }

        using XLWorkbook wb = new();
        var sheet = wb.AddWorksheet("Voucher Item Template");
        sheet.Cell("A2").InsertTable(dt).Theme = XLTableTheme.TableStyleMedium4;
        sheet.Protect("unibean");

        // Set style for cell
        sheet.Cell("B3").Style.Font.Italic = true;
        sheet.Cell("C1").FormulaA1 = "\"Số lượng khuyến mãi đã thêm:\r\n     \" & COUNTIF(B4:B1003,\"<>\") - COUNTIF(B4:B1003,\"* *\") & \" / 1000\"";
        sheet.Cell("C1").Style.Font.FontColor = XLColor.Red;
        sheet.Cell("C1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        sheet.Cell("C6").Value = "thinh0938081927";
        sheet.Cell("C6").Style.Font.FontColor = XLColor.White;
        sheet.Cell("A1").Value = "          *Lưu ý\r\n     - Stt: Số thứ tự của khuyến mãi.\r\n     - Code: Mã quét của khuyến mãi.\r\n     - Quantity: Số lượng của khuyến mãi.";

        // Set style for first row
        sheet.Row(1).Height = 90;
        sheet.Row(2).Style.Font.Bold = true;
        sheet.Row(2).Style.Font.FontSize = 20;
        sheet.Row(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        // Set style for column A,B,C,D
        sheet.Column("A").Width = 10;
        sheet.Columns("B").Width = 135;
        sheet.Columns("C").Width = 50;
        sheet.Columns("A:C").Style.Font.FontSize = 15;
        sheet.Columns("A:C").Style.Alignment.WrapText = true;
        sheet.Columns("A:C").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        sheet.Columns("D:XFD").Hide();

        // Set style for range
        sheet.Range("B4:B1003").Style.Protection.Locked = false;
        sheet.Range("B4:B1003").CreateDataValidation();
        sheet.Range("B4:B1003").GetDataValidation().AllowedValues = XLAllowedValues.TextLength;
        sheet.Range("B4:B1003").GetDataValidation().MinValue = "3";
        sheet.Range("B4:B1003").GetDataValidation().MaxValue = "50";
        sheet.Range("B4:B1003").GetDataValidation().InputMessage = "Nhập mã khuyến mãi";
        sheet.Range("B4:B1003").GetDataValidation().ErrorStyle = XLErrorStyle.Information;
        sheet.Range("B4:B1003").GetDataValidation().ErrorTitle = "Mã khuyến mãi không hợp lệ";
        sheet.Range("B4:B1003").GetDataValidation().ErrorMessage = "Mã khuyến mãi phải có độ dài từ 3 - 50 kí tự và không chứa khoảng trắng";
        sheet.Range("A1:B1").Merge();
        sheet.Range("A1:B1").Style.Font.Bold = true;
        sheet.Range("A1:B1").Style.Font.Italic = true;
        sheet.Range("A1:B1").Style.Font.FontColor = XLColor.DavysGrey;

        using MemoryStream ms = new()
        {

        };
        wb.SaveAs(ms);
        wb.Dispose();

        return ms;
    }

    public async Task<MemoryStreamModel> AddTemplate
        (InsertVoucherItemModel insert, JwtRequestModel request)
    {
        if (!request.Role.Equals("Brand"))
        {
            throw new InvalidParameterException("Chỉ thương hiệu thực hiện được chức năng này");
        }
        if (insert.Template != null && insert.Template.Length > 0)
        {
            var upload = $"{Directory.GetCurrentDirectory()}/wwwroot/upload/" + Ulid.NewUlid() + "/";
            if (!Directory.Exists(upload))
            {
                Directory.CreateDirectory(upload);
            }
            var filePath = Path.Combine(upload, insert.Template.FileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await insert.Template.CopyToAsync(stream);
            stream.Close();
            stream.Dispose();

            using var wb = new XLWorkbook(filePath);
            var sheet = wb.Worksheet(1);

            if (!sheet.IsPasswordProtected || !sheet.IsProtected
                || !sheet.Cell("C6").Value.Equals("thinh0938081927"))
            {
                RemoveFile(upload);
                throw new InvalidParameterException("Mẫu tạo khuyến mãi không hợp lệ");
            }

            sheet.Cell("C2").Value = "Chú thích";
            sheet.Cell("C2").Style.Font.FontSize = 23;
            sheet.Cell("C3").Value = "Khuyến mãi không hợp lệ";
            sheet.Cell("C3").Style.Fill.BackgroundColor = XLColor.Red;
            sheet.Cell("C4").Value = "Khuyến mãi trùng lặp";
            sheet.Cell("C4").Style.Fill.BackgroundColor = XLColor.Orange;
            sheet.Cell("C5").Value = "Khuyến mãi đã được sử dụng";
            sheet.Cell("C5").Style.Fill.BackgroundColor = XLColor.Yellow;
            var cells = sheet.Cells("B4:B1003").Where(c => !c.Value.IsBlank).ToList();

            if (cells.Count == 0)
            {
                RemoveFile(upload);
                throw new InvalidParameterException("Mẫu tạo khuyến mãi rỗng");
            }

            sheet.Cells("B3:B1003").Style.Fill.BackgroundColor = XLColor.NoColor;
            var index = voucherItemRepository.GetMaxIndex(insert.VoucherId);
            List<VoucherItem> list = new();
            int errorListValid = 0;
            int errorListDuplicate = 0;
            List<string> l = cells.Select(c => c.GetValue<string>()).ToList();
            var duplicateCodes = l.Select(i => i).GroupBy(x => x).Where(c => c.Count() > 1)
                .Select(c => c.Key);
            foreach (var cell in cells)
            {
                var data = cell.GetValue<string>();

                if (!Regex.IsMatch(data, @"^[^\s]{3,50}$"))
                {
                    cell.Style.Fill.BackgroundColor = XLColor.Red;
                    errorListValid++;
                }
                else if (duplicateCodes.Contains(data))
                {
                    cell.Style.Fill.BackgroundColor = XLColor.Orange;
                }
                else if (voucherItemRepository.CheckVoucherCode(data, request.UserId))
                {
                    cell.Style.Fill.BackgroundColor = XLColor.Yellow;
                    errorListDuplicate++;
                }

                Thread.Sleep(1);
                list.Add(new VoucherItem
                {
                    Id = Ulid.NewUlid().ToString(),
                    VoucherId = insert.VoucherId,
                    VoucherCode = data,
                    Index = ++index,
                    IsLocked = false,
                    IsBought = false,
                    IsUsed = false,
                    DateCreated = DateTime.Now,
                    State = true,
                    Status = true,
                });
            }
            using MemoryStream ms = new();
            wb.SaveAs(ms);
            wb.Dispose();
            RemoveFile(upload);
            if (errorListValid > 0 || errorListDuplicate > 0 || duplicateCodes.Any())
            {
                return new()
                {
                    IsValid = false,
                    Ms = ms,
                };
            }

            voucherItemRepository.AddList(list);
            return new()
            {
                IsValid = true,
                Ms = CreateResult(GetEmpdata(list, voucherRepository.GetById(insert.VoucherId).VoucherName)),
            };
        }
        throw new InvalidParameterException("Tệp không hợp lệ");
    }

    public VoucherItemExtraModel GetByCode(string code, string brandId)
    {
        VoucherItem entity = voucherItemRepository.GetByVoucherCode(code, brandId);
        if (entity != null)
        {
            return mapper.Map<VoucherItemExtraModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy khuyến mãi");
    }

    public VoucherItemExtraModel EntityToExtra(VoucherItem item)
    {
        return mapper.Map<VoucherItemExtraModel>(item);
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
                Thread.Sleep(1);
                yield return context.Mapper.Map<VoucherItem>(source);
            }
        }
    }
}
