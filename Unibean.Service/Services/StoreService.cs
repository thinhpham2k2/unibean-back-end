using AutoMapper;
using DocumentFormat.OpenXml.Vml.Office;
using FirebaseAdmin.Messaging;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Activities;
using Unibean.Service.Models.CampaignDetails;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Stores;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Models.VoucherItems;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Unibean.Service.Services;

public enum StoreTransactionType
{
    ActivityTransaction = 1,
    BonusTransaction = 2
}

public class StoreService : IStoreService
{
    private readonly Mapper mapper;

    private readonly string ACCOUNT_FOLDER_NAME = "accounts";

    private readonly IStoreRepository storeRepository;

    private readonly IFireBaseService fireBaseService;

    private readonly IAccountRepository accountRepository;

    private readonly ICampaignDetailService campaignDetailService;

    private readonly IActivityService activityService;

    private readonly IBonusService bonusService;

    private readonly IVoucherItemRepository voucherItemRepository;

    private readonly IStudentRepository studentRepository;

    private readonly IVoucherItemService voucherItemService;

    public StoreService(IStoreRepository storeRepository,
        IFireBaseService fireBaseService,
        IAccountRepository accountRepository,
        ICampaignDetailService campaignDetailService,
        IActivityService activityService,
        IBonusService bonusService,
        IVoucherItemRepository voucherItemRepository,
        IStudentRepository studentRepository,
        IVoucherItemService voucherItemService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Store, StoreModel>()
            .ForMember(s => s.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
            .ForMember(s => s.AreaName, opt => opt.MapFrom(src => src.Area.AreaName))
            .ForMember(s => s.UserName, opt => opt.MapFrom(src => src.Account.UserName))
            .ForMember(s => s.Avatar, opt => opt.MapFrom(src => src.Account.Avatar))
            .ForMember(s => s.AvatarFileName, opt => opt.MapFrom(src => src.Account.FileName))
            .ForMember(s => s.Email, opt => opt.MapFrom(src => src.Account.Email))
            .ForMember(s => s.Phone, opt => opt.MapFrom(src => src.Account.Phone))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Store>, PagedResultModel<StoreModel>>()
            .ReverseMap();
            // Map Store Extra Model
            cfg.CreateMap<Store, StoreExtraModel>()
            .ForMember(s => s.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
            .ForMember(s => s.BrandLogo, opt => opt.MapFrom(src => src.Brand.Account.Avatar))
            .ForMember(s => s.AreaName, opt => opt.MapFrom(src => src.Area.AreaName))
            .ForMember(s => s.AreaImage, opt => opt.MapFrom(src => src.Area.Image))
            .ForMember(s => s.UserName, opt => opt.MapFrom(src => src.Account.UserName))
            .ForMember(s => s.Avatar, opt => opt.MapFrom(src => src.Account.Avatar))
            .ForMember(s => s.AvatarFileName, opt => opt.MapFrom(src => src.Account.FileName))
            .ForMember(s => s.Email, opt => opt.MapFrom(src => src.Account.Email))
            .ForMember(s => s.Phone, opt => opt.MapFrom(src => src.Account.Phone))
            .ForMember(s => s.NumberOfCampaigns, opt => opt.MapFrom(src => src.CampaignStores.Count))
            .ForMember(s => s.NumberOfVouchers, opt => opt.MapFrom(src => src.Activities.Count))
            .ForMember(s => s.NumberOfBonuses, opt => opt.MapFrom(src => src.Bonuses.Count))
            .ForMember(s => s.AmountOfBonuses, opt => opt.MapFrom(src => src.Bonuses.Select(b => b.Amount).Sum()))
            .ReverseMap();
            // Map Create Store Model
            cfg.CreateMap<Store, CreateStoreModel>()
            .ReverseMap()
            .ForMember(s => s.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(s => s.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(s => s.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(s => s.Status, opt => opt.MapFrom(src => true));
            cfg.CreateMap<Account, CreateStoreModel>()
            .ReverseMap()
            .ForMember(s => s.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(s => s.Role, opt => opt.MapFrom(src => Role.Store))
            .ForMember(s => s.Password, opt => opt.MapFrom(src => BCryptNet.HashPassword(src.Password)))
            .ForMember(s => s.IsVerify, opt => opt.MapFrom(src => true))
            .ForMember(s => s.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(s => s.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(s => s.DateVerified, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(s => s.Status, opt => opt.MapFrom(src => true));
            cfg.CreateMap<Store, UpdateStoreModel>()
            .ReverseMap()
            .ForMember(s => s.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForPath(s => s.Account.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForPath(s => s.Account.Description, opt => opt.MapFrom(src => src.Description))
            .ForPath(s => s.Account.State, opt => opt.MapFrom(src => src.State));
            // Map Create Activity
            cfg.CreateMap<CreateActivityModel, CreateUseActivityModel>()
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.storeRepository = storeRepository;
        this.fireBaseService = fireBaseService;
        this.accountRepository = accountRepository;
        this.campaignDetailService = campaignDetailService;
        this.activityService = activityService;
        this.bonusService = bonusService;
        this.voucherItemRepository = voucherItemRepository;
        this.studentRepository = studentRepository;
        this.voucherItemService = voucherItemService;
    }

    public async Task<StoreExtraModel> Add(CreateStoreModel creation)
    {
        Account account = mapper.Map<Account>(creation);

        //Upload avatar
        if (creation.Avatar != null && creation.Avatar.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Avatar, ACCOUNT_FOLDER_NAME);
            account.Avatar = f.URL;
            account.FileName = f.FileName;
        }

        account = accountRepository.Add(account);
        Store store = mapper.Map<Store>(creation);
        store.AccountId = account.Id;

        return mapper.Map<StoreExtraModel>(storeRepository.Add(store));
    }

    public bool AddActivity
        (string id, string code, CreateUseActivityModel creation)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        var store = storeRepository.GetById(id);
        var item = voucherItemRepository.GetByVoucherCode(code, store.BrandId)
            ?? throw new InvalidParameterException("Không tìm thấy khuyến mãi");
        if (new[] { CampaignState.Active, CampaignState.Inactive }.Contains
            (item.CampaignDetail.Campaign.CampaignActivities.LastOrDefault().State.Value))
        {
            if (item.CampaignDetail.Campaign.StartOn <= today && today <= item.CampaignDetail.Campaign.EndOn)
            {
                if (item.CampaignDetail.Campaign.CampaignStores.Any(c => c.StoreId.Equals(id)))
                {
                    if ((bool)item.IsBought && item.Activities.FirstOrDefault() != null)
                    {
                        if (!(bool)item.IsUsed)
                        {
                            var stu = studentRepository.GetById
                                (item.Activities.FirstOrDefault().StudentId);
                            if (stu != null && stu.State.Equals(StudentState.Active))
                            {
                                CreateActivityModel create = mapper.Map<CreateActivityModel>(creation);
                                create.StudentId = item.Activities.FirstOrDefault().StudentId;
                                create.VoucherItemId = item.Id;
                                create.StoreId = id;

                                // Push notification to mobile app
                                fireBaseService.PushNotificationToStudent(new Message
                                {
                                    Data = new Dictionary<string, string>()
                                    {
                                        { "brandId", "" },
                                        { "campaignId", "" },
                                        { "image", "https://image" },
                                    },
                                    //Token = registrationToken,
                                    Topic = stu.Id,
                                    Notification = new Notification()
                                    {
                                        Title = store.StoreName + " đã quét thành công " + item.Voucher.VoucherName,
                                        Body = "Bạn đã nhận được " 
                                        + item.CampaignDetail.Price * item.CampaignDetail.Rate 
                                        + " đậu đỏ do sử dụng " + item.Voucher.VoucherName,
                                        ImageUrl = "https://image"
                                    }
                                });

                                return activityService.Add(create) != null;
                            }
                            throw new InvalidParameterException("Sinh viên không hợp lệ");
                        }
                        throw new InvalidParameterException("Mã khuyến mãi đã được sử dụng");
                    }
                    throw new InvalidParameterException("Mã khuyến mãi chưa được thanh toán");
                }
                throw new InvalidParameterException("Mã khuyến mãi không được áp dụng cho cửa hàng này");
            }
            throw new InvalidParameterException("Chiến dịch chưa khởi chạy");
        }
        throw new InvalidParameterException("Chiến dịch không hợp lệ");
    }

    public void Delete(string id)
    {
        Store entity = storeRepository.GetById(id);
        if (entity != null)
        {
            if (entity.CampaignStores.All(
                s => new[] { CampaignState.Closed, CampaignState.Cancelled }.Contains(s.Campaign.CampaignActivities.LastOrDefault().State.Value)))
            {
                // Avatar
                if (entity.Account.Avatar != null && entity.Account.Avatar.Length > 0)
                {
                    // Remove image
                    fireBaseService.RemoveFileAsync(entity.Account.FileName, ACCOUNT_FOLDER_NAME);
                }

                storeRepository.Delete(id);
                accountRepository.Delete(entity.Account.Id);
            }
            else
            {
                throw new InvalidParameterException("Xóa thất bại do tồn tại chiến dịch hoạt động ở cửa hàng");
            }
        }
        else
        {
            throw new InvalidParameterException("Không tìm thấy cửa hàng");
        }
    }

    public PagedResultModel<StoreModel> GetAll
        (List<string> brandIds, List<string> areaIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<StoreModel>>
            (storeRepository.GetAll(brandIds, areaIds, state, propertySort, isAsc, search, page, limit));
    }

    public PagedResultModel<StoreModel> GetAllByCampaign
        (List<string> campaignIds, List<string> brandIds, List<string> areaIds,
        bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<StoreModel>>
            (storeRepository.GetAllByCampaign(campaignIds, brandIds, areaIds,
            state, propertySort, isAsc, search, page, limit));
    }

    public StoreExtraModel GetById(string id)
    {
        Store entity = storeRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<StoreExtraModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy cửa hàng");
    }

    public PagedResultModel<StoreTransactionModel> GetHistoryTransactionListByStoreId
        (string id, List<StoreTransactionType> typeIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        Store entity = storeRepository.GetById(id);
        if (entity != null)
        {
            var query = (typeIds.Contains(StoreTransactionType.ActivityTransaction) || typeIds.Count == 0 ?
                activityService.GetList
                (new() { id }, new(), new(), search) : new())

                .Concat(typeIds.Contains(StoreTransactionType.BonusTransaction) || typeIds.Count == 0 ?
                bonusService.GetList
                (new(), new() { id }, new(), search) : new())

                .AsQueryable()
                .Where(t => state == null || state.Equals(t.State))
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();

            return new()
            {
                CurrentPage = page,
                PageSize = limit,
                PageCount = (int)Math.Ceiling((double)query.Count() / limit),
                Result = result,
                RowCount = result.Count,
                TotalCount = query.Count()
            };
        }
        throw new InvalidParameterException("Không tìm thấy cửa hàng");
    }

    public PagedResultModel<CampaignDetailModel> GetCampaignDetailByStoreId
        (string id, List<string> campaignIds, List<string> typeIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        Store entity = storeRepository.GetById(id);
        if (entity != null)
        {
            return campaignDetailService.GetAllByStore
                (id, campaignIds, typeIds, state, propertySort, isAsc, search, page, limit);
        }
        throw new InvalidParameterException("Không tìm thấy cửa hàng");
    }

    public async Task<StoreExtraModel> Update(string id, UpdateStoreModel update)
    {
        Store entity = storeRepository.GetById(id);
        if (entity != null)
        {
            entity = mapper.Map(update, entity);

            // Avatar
            if (update.Avatar != null && update.Avatar.Length > 0)
            {
                // Remove image
                await fireBaseService.RemoveFileAsync(entity.Account.FileName, ACCOUNT_FOLDER_NAME);

                //Upload new image update
                FireBaseFile f = await fireBaseService.UploadFileAsync(update.Avatar, ACCOUNT_FOLDER_NAME);
                entity.Account.Avatar = f.URL;
                entity.Account.FileName = f.FileName;
            }

            return mapper.Map<StoreExtraModel>(storeRepository.Update(entity));
        }
        throw new InvalidParameterException("Không tìm thấy cửa hàng");
    }

    public CampaignDetailExtraModel GetCampaignDetailById(string id, string detailId)
    {
        Store entity = storeRepository.GetById(id);
        if (entity != null)
        {
            return campaignDetailService.GetById(detailId);
        }
        throw new InvalidParameterException("Không tìm thấy cửa hàng");
    }

    public VoucherItemExtraModel GetVoucherItemByCode(string id, string code)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);

        var item = voucherItemRepository.GetByVoucherCode(code, storeRepository.GetById(id).BrandId)
            ?? throw new InvalidParameterException("Không tìm thấy khuyến mãi");
        if (item.ValidOn <= today && today <= item.ExpireOn)
        {
            if (item.CampaignDetail.Campaign.CampaignStores.Any(c => c.StoreId.Equals(id)))
            {

                if ((bool)item.IsBought && item.Activities.FirstOrDefault() != null)
                {
                    if (!(bool)item.IsUsed)
                    {
                        return voucherItemService.EntityToExtra(item);
                    }
                    throw new InvalidParameterException("Mã khuyến mãi đã được sử dụng");
                }
                throw new InvalidParameterException("Mã khuyến mãi chưa được thanh toán");
            }
            throw new InvalidParameterException("Mã khuyến mãi không được áp dụng cho cửa hàng này");
        }
        throw new InvalidParameterException("Khuyến mãi đã quá hạn sử dụng");
    }
}
