using AutoMapper;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Stores;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Models.Vouchers;
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

    private readonly string FOLDER_NAME = "stores";

    private readonly string ACCOUNT_FOLDER_NAME = "accounts";

    private readonly IStoreRepository storeRepository;

    private readonly IFireBaseService fireBaseService;

    private readonly IRoleService roleService;

    private readonly IAccountRepository accountRepository;

    private readonly IVoucherService voucherService;

    private readonly IActivityService activityService;

    private readonly IBonusService bonusService;

    public StoreService(IStoreRepository storeRepository,
        IFireBaseService fireBaseService,
        IRoleService roleService,
        IAccountRepository accountRepository,
        IVoucherService voucherService,
        IActivityService activityService,
        IBonusService bonusService)
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
        });
        mapper = new Mapper(config);
        this.storeRepository = storeRepository;
        this.fireBaseService = fireBaseService;
        this.roleService = roleService;
        this.accountRepository = accountRepository;
        this.voucherService = voucherService;
        this.activityService = activityService;
        this.bonusService = bonusService;
    }

    public async Task<StoreModel> Add(CreateStoreModel creation)
    {
        Account account = mapper.Map<Account>(creation);
        account.RoleId = roleService.GetRoleByName("Store")?.Id;

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

        return mapper.Map<StoreModel>(storeRepository.Add(store));
    }

    public void Delete(string id)
    {
        Store entity = storeRepository.GetById(id);
        if (entity != null)
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
            throw new InvalidParameterException("Not found store");
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
        throw new InvalidParameterException("Not found store");
    }

    public PagedResultModel<StoreTransactionModel> GetHistoryTransactionListByStoreId
        (string id, List<StoreTransactionType> typeIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
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

    public PagedResultModel<VoucherModel> GetVoucherListByStoreId
        (string id, List<string> campaignIds, List<string> typeIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        return voucherService.GetAllByStore
            (new() { id }, campaignIds, typeIds, state, propertySort, isAsc, search, page, limit);
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
        throw new InvalidParameterException("Not found store");
    }
}
