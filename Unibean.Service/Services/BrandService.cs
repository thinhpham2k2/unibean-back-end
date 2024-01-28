using AutoMapper;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Authens;
using Unibean.Service.Models.Brands;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Stores;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Models.Vouchers;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Unibean.Service.Services;

public class BrandService : IBrandService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "brands";

    private readonly string ACCOUNT_FOLDER_NAME = "accounts";

    private readonly IBrandRepository brandRepository;

    private readonly IFireBaseService fireBaseService;

    private readonly IRoleService roleService;

    private readonly IAccountRepository accountRepository;

    private readonly ICampaignService campaignService;

    private readonly IStoreService storeService;

    private readonly IVoucherService voucherService;

    private readonly IBonusTransactionService bonusTransactionService;

    private readonly IWalletTransactionService walletTransactionService;

    private readonly IRequestTransactionService requestTransactionService;

    public BrandService(IBrandRepository brandRepository,
        IFireBaseService fireBaseService,
        IRoleService roleService,
        IAccountRepository accountRepository,
        ICampaignService campaignService,
        IStoreService storeService,
        IVoucherService voucherService,
        IBonusTransactionService bonusTransactionService,
        IWalletTransactionService walletTransactionService,
        IRequestTransactionService requestTransactionService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            // Map Brand Model
            cfg.CreateMap<Brand, BrandModel>()
            .ForMember(p => p.UserName, opt => opt.MapFrom(src => src.Account.UserName))
            .ForMember(p => p.Logo, opt => opt.MapFrom(src => src.Account.Avatar))
            .ForMember(p => p.LogoFileName, opt => opt.MapFrom(src => src.Account.FileName))
            .ForMember(p => p.Email, opt => opt.MapFrom(src => src.Account.Email))
            .ForMember(p => p.Phone, opt => opt.MapFrom(src => src.Account.Phone))
            .ForMember(p => p.GreenWallet, opt => opt.MapFrom(src => src.Wallets.FirstOrDefault().Balance))
            .ForMember(p => p.GreenWalletImage, opt => opt.MapFrom(src => src.Wallets.FirstOrDefault().Type.Image))
            .ForMember(p => p.RedWallet, opt => opt.MapFrom(src => src.Wallets.Skip(1).FirstOrDefault().Balance))
            .ForMember(p => p.RedWalletImage, opt => opt.MapFrom(src => src.Wallets.Skip(1).FirstOrDefault().Type.Image))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Brand>, PagedResultModel<BrandModel>>()
            .ForMember(p => p.Result, opt => opt.Ignore())
            .ReverseMap();
            // Map Brand Extra Model
            cfg.CreateMap<Brand, BrandExtraModel>()
            .ForMember(p => p.UserName, opt => opt.MapFrom(src => src.Account.UserName))
            .ForMember(p => p.Logo, opt => opt.MapFrom(src => src.Account.Avatar))
            .ForMember(p => p.LogoFileName, opt => opt.MapFrom(src => src.Account.FileName))
            .ForMember(p => p.Email, opt => opt.MapFrom(src => src.Account.Email))
            .ForMember(p => p.Phone, opt => opt.MapFrom(src => src.Account.Phone))
            .ForMember(p => p.NumberOfFollowers, opt => opt.MapFrom(src => src.Wishlists.Count))
            .ForMember(p => p.GreenWallet, opt => opt.MapFrom(src => src.Wallets.FirstOrDefault().Balance))
            .ForMember(p => p.GreenWalletImage, opt => opt.MapFrom(src => src.Wallets.FirstOrDefault().Type.Image))
            .ForMember(p => p.RedWallet, opt => opt.MapFrom(src => src.Wallets.Skip(1).FirstOrDefault().Balance))
            .ForMember(p => p.RedWalletImage, opt => opt.MapFrom(src => src.Wallets.Skip(1).FirstOrDefault().Type.Image))
            .ReverseMap();
            // Map Create Brand Google Model
            cfg.CreateMap<Brand, CreateBrandGoogleModel>()
            .ReverseMap()
            .ForMember(p => p.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(p => p.TotalIncome, opt => opt.MapFrom(src => 0))
            .ForMember(p => p.TotalSpending, opt => opt.MapFrom(src => 0))
            .ForMember(p => p.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(p => p.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(p => p.Status, opt => opt.MapFrom(src => true));
            // Map Create Brand Model
            cfg.CreateMap<Brand, CreateBrandModel>()
            .ReverseMap()
            .ForMember(p => p.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(p => p.TotalIncome, opt => opt.MapFrom(src => 0))
            .ForMember(p => p.TotalSpending, opt => opt.MapFrom(src => 0))
            .ForMember(p => p.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(p => p.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(p => p.Status, opt => opt.MapFrom(src => true));
            cfg.CreateMap<Account, CreateBrandModel>()
            .ReverseMap()
            .ForMember(p => p.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(p => p.Password, opt => opt.MapFrom(src => BCryptNet.HashPassword(src.Password)))
            .ForMember(p => p.IsVerify, opt => opt.MapFrom(src => true))
            .ForMember(p => p.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(p => p.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(p => p.DateVerified, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(p => p.Status, opt => opt.MapFrom(src => true));
            // Map Update Brand Model
            cfg.CreateMap<Brand, UpdateBrandModel>()
            .ReverseMap()
            .ForMember(p => p.CoverPhoto, opt => opt.Ignore())
            .ForMember(p => p.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForPath(p => p.Account.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForPath(p => p.Account.Description, opt => opt.MapFrom(src => src.Description))
            .ForPath(p => p.Account.State, opt => opt.MapFrom(src => src.State));
        });
        mapper = new Mapper(config);
        this.brandRepository = brandRepository;
        this.fireBaseService = fireBaseService;
        this.roleService = roleService;
        this.accountRepository = accountRepository;
        this.campaignService = campaignService;
        this.storeService = storeService;
        this.voucherService = voucherService;
        this.bonusTransactionService = bonusTransactionService;
        this.walletTransactionService = walletTransactionService;
        this.requestTransactionService = requestTransactionService;
    }

    public async Task<BrandModel> Add(CreateBrandModel creation)
    {
        Account account = mapper.Map<Account>(creation);
        account.RoleId = roleService.GetRoleByName("Brand")?.Id;

        //Upload logo
        if (creation.Logo != null && creation.Logo.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Logo, ACCOUNT_FOLDER_NAME);
            account.Avatar = f.URL;
            account.FileName = f.FileName;
        }

        account = accountRepository.Add(account);
        Brand brand = mapper.Map<Brand>(creation);
        brand.AccountId = account.Id;

        //Upload cover photo
        if (creation.CoverPhoto != null && creation.CoverPhoto.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.CoverPhoto, FOLDER_NAME);
            brand.CoverPhoto = f.URL;
            brand.CoverFileName = f.FileName;
        }

        return mapper.Map<BrandModel>(brandRepository.Add(brand));
    }

    public BrandModel AddGoogle(CreateBrandGoogleModel creation)
    {
        return mapper.Map<BrandModel>(brandRepository.Add(mapper.Map<Brand>(creation)));
    }

    public void Delete(string id)
    {
        Brand entity = brandRepository.GetById(id);
        if (entity != null)
        {
            // Cover photo
            if (entity.CoverPhoto != null && entity.CoverPhoto.Length > 0)
            {
                // Remove image
                fireBaseService.RemoveFileAsync(entity.CoverFileName, FOLDER_NAME);
            }

            // Logo (Avatar)
            if (entity.Account.Avatar != null && entity.Account.Avatar.Length > 0)
            {
                // Remove image
                fireBaseService.RemoveFileAsync(entity.Account.FileName, ACCOUNT_FOLDER_NAME);
            }

            brandRepository.Delete(id);
            accountRepository.Delete(entity.Account.Id);
        }
        else
        {
            throw new InvalidParameterException("Không tìm thấy thương hiệu");
        }
    }

    public PagedResultModel<BrandModel> GetAll
        (bool? state, string propertySort, bool isAsc, string search, 
        int page, int limit, JwtRequestModel request)
    {
        if (request.Role.Equals("Student"))
        {
            PagedResultModel<Brand> result = brandRepository.GetAll(state, propertySort, isAsc, search, page, limit);
            PagedResultModel<BrandModel> pages = mapper.Map<PagedResultModel<BrandModel>>(result);
            pages.Result = result.Result.Select(r =>
            {
                return mapper.Map<Brand, BrandModel>(r, opt
                    => opt.AfterMap((src, dest)
                    => dest.IsFavor = src.Wishlists.Where(w => w.StudentId.Equals(request.UserId))?.Count() > 0));
            }).ToList();

            return pages;
        }
        else
        {
            return mapper.Map<PagedResultModel<Brand>, PagedResultModel<BrandModel>>
                (brandRepository.GetAll(state, propertySort, isAsc, search, page, limit), opt
                => opt.AfterMap((src, dest)
                => dest.Result = mapper.Map<List<BrandModel>>(src.Result)));
        }
    }

    public BrandExtraModel GetById(string id, JwtRequestModel request)
    {
        Brand entity = brandRepository.GetById(id);
        if (entity != null)
        {
            return request.Role.Equals("Student") ? mapper.Map<Brand, BrandExtraModel>(entity, opt
                => opt.AfterMap((src, dest) => dest.IsFavor = src.Wishlists.Where(w
                => w.StudentId.Equals(request.UserId)).Any()))
                : mapper.Map<BrandExtraModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy thương hiệu");
    }

    public PagedResultModel<CampaignModel> GetCampaignListByBrandId
        (string id, List<string> typeIds, List<string> storeIds, List<string> majorIds, List<string> campusIds,
        bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return campaignService.GetAll(new() { id }, typeIds, storeIds, majorIds, 
            campusIds, state, propertySort, isAsc, search, page, limit);
    }

    public PagedResultModel<TransactionModel> GetHistoryTransactionListByStudentId
        (string id, List<string> walletTypeIds, bool? state, string propertySort, 
        bool isAsc, string search, int page, int limit)
    {
        var query = bonusTransactionService.GetAll
            (brandRepository.GetById(id).Wallets.Select(w => w.Id).ToList(), new(), walletTypeIds, search)
            .Concat(walletTransactionService.GetAll
            (brandRepository.GetById(id).Wallets.Select(w => w.Id).ToList(), new(), walletTypeIds, search))
            .Concat(requestTransactionService.GetAll
            (brandRepository.GetById(id).Wallets.Select(w => w.Id).ToList(), new(), walletTypeIds, search))
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

    public PagedResultModel<StoreModel> GetStoreListByBrandId
        (string id, List<string> areaIds, bool? state, string propertySort, 
        bool isAsc, string search, int page, int limit)
    {
        return storeService.GetAll(new() { id }, areaIds, state, 
            propertySort, isAsc, search, page, limit);
    }

    public PagedResultModel<VoucherModel> GetVoucherListByBrandId
        (string id, List<string> typeIds, bool? state, string propertySort,
        bool isAsc, string search, int page, int limit)
    {
        return voucherService.GetAll(new() { id }, typeIds, state, 
            propertySort, isAsc, search, page, limit);
    }

    public async Task<BrandExtraModel> Update(string id, UpdateBrandModel update)
    {
        Brand entity = brandRepository.GetById(id);
        if (entity != null)
        {
            entity = mapper.Map(update, entity);

            // Cover photo
            if (update.CoverPhoto != null && update.CoverPhoto.Length > 0)
            {
                // Remove image
                await fireBaseService.RemoveFileAsync(entity.CoverFileName, FOLDER_NAME);

                //Upload new image update
                FireBaseFile f = await fireBaseService.UploadFileAsync(update.CoverPhoto, FOLDER_NAME);
                entity.CoverPhoto = f.URL;
                entity.CoverFileName = f.FileName;
            }

            // Logo (Avatar)
            if (update.Logo != null && update.Logo.Length > 0)
            {
                // Remove image
                await fireBaseService.RemoveFileAsync(entity.Account.FileName, ACCOUNT_FOLDER_NAME);

                //Upload new image update
                FireBaseFile f = await fireBaseService.UploadFileAsync(update.Logo, ACCOUNT_FOLDER_NAME);
                entity.Account.Avatar = f.URL;
                entity.Account.FileName = f.FileName;
            }

            return mapper.Map<BrandExtraModel>(brandRepository.Update(entity));
        }
        throw new InvalidParameterException("Không tìm thấy thương hiệu");
    }
}
