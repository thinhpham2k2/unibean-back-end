using AutoMapper;
using Enable.EnumDisplayName;
using FirebaseAdmin.Messaging;
using Microsoft.IdentityModel.Tokens;
using MoreLinq;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Activities;
using Unibean.Service.Models.Authens;
using Unibean.Service.Models.CampaignActivities;
using Unibean.Service.Models.CampaignCampuses;
using Unibean.Service.Models.CampaignDetails;
using Unibean.Service.Models.CampaignMajors;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.CampaignStores;
using Unibean.Service.Models.Campuses;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Majors;
using Unibean.Service.Models.Stores;
using Unibean.Service.Models.WebHooks;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class CampaignService : ICampaignService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "campaigns";

    private readonly ICampaignRepository campaignRepository;

    private readonly IVoucherRepository voucherRepository;

    private readonly IFireBaseService fireBaseService;

    private readonly IStoreService storeService;

    private readonly IMajorService majorService;

    private readonly ICampusService campusService;

    private readonly IDiscordService discordService;

    private readonly IActivityService activityService;

    private readonly IVoucherItemRepository voucherItemRepository;

    private readonly IStudentRepository studentRepository;

    private readonly ICampaignDetailService campaignDetailService;

    private readonly ICampaignActivityService campaignActivityService;

    private readonly ICampaignActivityRepository campaignActivityRepository;

    public CampaignService(ICampaignRepository campaignRepository,
        IVoucherRepository voucherRepository,
        IFireBaseService fireBaseService,
        IStoreService storeService,
        IMajorService majorService,
        ICampusService campusService,
        IDiscordService discordService,
        IActivityService activityService,
        IVoucherItemRepository voucherItemRepository,
        IStudentRepository studentRepository,
        ICampaignDetailService campaignDetailService,
        ICampaignActivityService campaignActivityService,
        ICampaignActivityRepository campaignActivityRepository)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            // Map Campaign Model
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
            cfg.CreateMap<PagedResultModel<Campaign>, PagedResultModel<CampaignModel>>()
            .ReverseMap();
            // Map Campaign Extra Model
            cfg.CreateMap<Campaign, CampaignExtraModel>()
            .ForMember(c => c.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
            .ForMember(c => c.BrandAcronym, opt => opt.MapFrom(src => src.Brand.Acronym))
            .ForMember(c => c.BrandLogo, opt => opt.MapFrom(src => src.Brand.Account.Avatar))
            .ForMember(c => c.TypeName, opt => opt.MapFrom(src => src.Type.TypeName))
            .ForMember(c => c.TypeImage, opt => opt.MapFrom(src => src.Type.Image))
            .ForMember(c => c.CurrentStateId, opt => opt.MapFrom(
                src => (int)src.CampaignActivities.LastOrDefault().State))
            .ForMember(c => c.CurrentState, opt => opt.MapFrom(
                src => src.CampaignActivities.LastOrDefault().State))
            .ForMember(c => c.CurrentStateName, opt => opt.MapFrom(
                src => src.CampaignActivities.LastOrDefault().State.GetDisplayName()))
            .ForMember(c => c.NumberOfParticipants, opt => opt.MapFrom((src, dest) =>
            {
                try
                {
                    var i = src.CampaignDetails?.SelectMany(c => c.VoucherItems);
                    return i.IsNullOrEmpty() ? 0 : i.Where(v => (bool)v.IsUsed).Count();
                }
                catch
                {
                    return 0;
                }
            }))
            .ForMember(c => c.UsageCost, opt => opt.MapFrom(src => src.TotalSpending))
            .ForMember(c => c.TotalCost, opt => opt.MapFrom(src => src.TotalIncome))
            .ReverseMap();
            // Map Campaign Store Model
            cfg.CreateMap<CampaignStore, CreateCampaignStoreModel>()
            .ReverseMap()
            .ForMember(c => c.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(c => c.Status, opt => opt.MapFrom(src => true));
            // Map Campaign Campus Model
            cfg.CreateMap<CampaignCampus, CreateCampaignCampusModel>()
            .ReverseMap()
            .ForMember(c => c.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(c => c.Status, opt => opt.MapFrom(src => true));
            // Map Campaign Major Model
            cfg.CreateMap<CampaignMajor, CreateCampaignMajorModel>()
            .ReverseMap()
            .ForMember(c => c.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(c => c.Status, opt => opt.MapFrom(src => true));
            // Map Campaign Detail Model
            cfg.CreateMap<CampaignDetail, CreateCampaignDetailModel>()
            .ReverseMap()
            .ForMember(c => c.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(c => c.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(c => c.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(c => c.Status, opt => opt.MapFrom(src => true));
            // Map Create Campaign Model
            cfg.CreateMap<Campaign, CreateCampaignModel>()
            .ReverseMap()
            .ForMember(c => c.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(c => c.Duration, opt => opt.MapFrom(src
                => ((DateOnly)src.EndOn).DayNumber - ((DateOnly)src.StartOn).DayNumber))
            .ForMember(c => c.TotalSpending, opt => opt.MapFrom(src => 0))
            .ForMember(c => c.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(c => c.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(c => c.Status, opt => opt.MapFrom(src => true));
            // Map Update Campaign Model
            cfg.CreateMap<Campaign, UpdateCampaignModel>()
            .ReverseMap()
            .ForMember(c => c.Type, opt => opt.MapFrom(src => (string)null))
            .ForMember(c => c.Image, opt => opt.Ignore())
            .ForMember(c => c.ImageName, opt => opt.Ignore())
            .ForMember(c => c.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
            // Map Create Activity
            cfg.CreateMap<CreateActivityModel, CreateBuyActivityModel>()
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.campaignRepository = campaignRepository;
        this.voucherRepository = voucherRepository;
        this.fireBaseService = fireBaseService;
        this.storeService = storeService;
        this.majorService = majorService;
        this.campusService = campusService;
        this.discordService = discordService;
        this.activityService = activityService;
        this.voucherItemRepository = voucherItemRepository;
        this.studentRepository = studentRepository;
        this.campaignDetailService = campaignDetailService;
        this.campaignActivityService = campaignActivityService;
        this.campaignActivityRepository = campaignActivityRepository;
    }

    public async Task<CampaignExtraModel> Add(CreateCampaignModel creation)
    {
        Campaign campaign = mapper.Map<Campaign>(creation);

        //Upload image
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            campaign.Image = f.URL;
            campaign.ImageName = f.FileName;
        }

        campaign.CampaignDetails = campaign.CampaignDetails.Select(d =>
        {
            var v = voucherRepository.GetById(d.VoucherId);
            d.CampaignId = campaign.Id;
            d.Price = v.Price;
            d.Rate = v.Rate;
            var i = voucherItemRepository.GetIndex
                (d.VoucherId, (int)d.Quantity);
            d.FromIndex = i.FromIndex;
            d.ToIndex = i.ToIndex;
            return d;
        }).ToList();

        campaign = campaignRepository.Add(campaign);

        if (campaign != null)
        {
            campaign.CampaignDetails.ForEach(d =>
            {
                voucherItemRepository.UpdateList
                (d.VoucherId, d.Id, (int)d.Quantity,
                (DateOnly)campaign.StartOn, (DateOnly)campaign.EndOn, new ItemIndex
                {
                    FromIndex = d.FromIndex,
                    ToIndex = d.ToIndex,
                });
            });

            discordService.CreateWebHooks(new DiscordWebhookModel
            {
                Embeds = new() {
                    new() {
                        Author = new()
                        {
                            Name = campaign.Brand.BrandName,
                            Url = campaign.Brand.Link,
                            IconUrl = campaign.Brand.Account.Avatar
                        },

                        Fields = new()
                        {
                            new()
                            {
                                Name = "📢 Chiến dịch mới",
                                Value = campaign.CampaignName
                            },
                            new()
                            {
                                Name = "🆔 chiến dịch mới",
                                Value = "||" + campaign.Id + "||"
                            },
                            new()
                            {
                                Name = "💸 Tổng chi phí",
                                Value = campaign.TotalIncome + " đậu"
                            },
                            new()
                            {
                                Name = "▶️ Bắt đầu",
                                Value = ((DateOnly)campaign.StartOn).ToLongDateString()
                            },
                            new()
                            {
                                Name = "⏸️ Kết thúc",
                                Value = ((DateOnly)campaign.EndOn).ToLongDateString()
                            },
                        },

                        Image = new()
                        {
                            Url = campaign.Image
                        },

                        Footer = new()
                        {
                             Text = "Date created - " + ((DateTime)campaign.DateCreated).ToString("R")
                        },
                    }
                },
            });
        }

        return mapper.Map<CampaignExtraModel>(campaign);
    }

    public bool AddActivity
        (string id, string detailId, CreateBuyActivityModel creation)
    {
        Campaign entity = campaignRepository.GetById(id);
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        if (entity != null && entity.StartOn <= today && today <= entity.EndOn)
        {
            CampaignDetailExtraModel detail = campaignDetailService.GetById(detailId);
            if (detail != null && detail.CampaignId.Equals(entity.Id))
            {
                var items = campaignDetailService.GetAllVoucherItemByCampaignDetail(detailId);

                if (items.Count >= creation.Quantity)
                {
                    if (studentRepository.GetById
                        (creation.StudentId)?.Wallets.Where(w => w.Type.Equals(WalletType.Green)).FirstOrDefault().Balance
                        >= detail.Price * creation.Quantity)
                    {
                        items = items.Take((int)creation.Quantity).ToList();
                        foreach (string itemId in items)
                        {
                            CreateActivityModel create = mapper.Map<CreateActivityModel>(creation);
                            create.VoucherItemId = itemId;
                            activityService.Add(create);
                        }
                        return true;
                    }
                    throw new InvalidParameterException("Số dư đậu xanh của sinh viên không đủ");
                }
                throw new InvalidParameterException("Số lượng của khuyến mãi không đủ");
            }
            throw new InvalidParameterException("Chi tiết chiến dịch không hợp lệ");
        }
        throw new InvalidParameterException("Chiến dịch chưa khởi chạy");
    }

    public void Delete(string id)
    {
        Campaign entity = campaignRepository.GetById(id);
        if (entity != null)
        {
            if (entity.CampaignActivities.LastOrDefault().State.Equals(CampaignState.Closed))
            {
                if (entity.Image != null && entity.ImageName != null)
                {
                    //Remove image
                    fireBaseService.RemoveFileAsync(entity.ImageName, FOLDER_NAME);
                }
                campaignRepository.Delete(id);
            }
            else
            {
                throw new InvalidParameterException("Không thể xóa chiến dịch đang diễn ra");
            }
        }
        else
        {
            throw new InvalidParameterException("Không tìm thấy chiến dịch");
        }
    }

    public PagedResultModel<CampaignModel> GetAll
        (List<string> brandIds, List<string> typeIds, List<string> storeIds,
        List<string> majorIds, List<string> campusIds, List<CampaignState> stateIds,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<CampaignModel>>(campaignRepository
            .GetAll(brandIds, typeIds, storeIds, majorIds, campusIds, stateIds,
            propertySort, isAsc, search, page, limit));
    }

    public CampaignExtraModel GetById(string id)
    {
        Campaign entity = campaignRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<CampaignExtraModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy chiến dịch");
    }

    public PagedResultModel<CampaignActivityModel> GetCampaignActivityListByCampaignId
        (string id, List<CampaignState> stateIds,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        Campaign entity = campaignRepository.GetById(id);
        if (entity != null)
        {
            return campaignActivityService.GetAll
                (new() { id }, stateIds, propertySort,
                isAsc, search, page, limit);
        }
        throw new InvalidParameterException("Không tìm thấy chiến dịch");
    }

    public CampaignDetailExtraModel GetCampaignDetailById(string id, string detailId)
    {
        Campaign entity = campaignRepository.GetById(id);
        if (entity != null)
        {
            CampaignDetailExtraModel detail = campaignDetailService.GetById(detailId);
            if (detail != null && detail.CampaignId.Equals(id))
            {
                return detail;
            }
            throw new InvalidParameterException("Không tìm thấy chi tiết chiến dịch");
        }
        throw new InvalidParameterException("Không tìm thấy chiến dịch");
    }

    public PagedResultModel<CampaignDetailModel> GetCampaignDetailListByCampaignId
        (string id, List<string> typeIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        Campaign entity = campaignRepository.GetById(id);
        if (entity != null)
        {
            return campaignDetailService.GetAll
                (new() { id }, typeIds, state, propertySort,
                isAsc, search, page, limit);
        }
        throw new InvalidParameterException("Không tìm thấy chiến dịch");
    }

    public PagedResultModel<CampusModel> GetCampusListByCampaignId
        (string id, List<string> universityIds, List<string> areaIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        Campaign entity = campaignRepository.GetById(id);
        if (entity != null)
        {
            return campusService.GetAllByCampaign
                (new() { id }, universityIds, areaIds, state,
                propertySort, isAsc, search, page, limit);
        }
        throw new InvalidParameterException("Không tìm thấy chiến dịch");
    }

    public PagedResultModel<MajorModel> GetMajorListByCampaignId
        (string id, bool? state, string propertySort,
        bool isAsc, string search, int page, int limit)
    {
        Campaign entity = campaignRepository.GetById(id);
        if (entity != null)
        {
            return majorService.GetAllByCampaign
                (new() { id }, state, propertySort,
                isAsc, search, page, limit);
        }
        throw new InvalidParameterException("Không tìm thấy chiến dịch");
    }

    public PagedResultModel<StoreModel> GetStoreListByCampaignId
        (string id, List<string> brandIds, List<string> areaIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        Campaign entity = campaignRepository.GetById(id);
        if (entity != null)
        {
            return storeService.GetAllByCampaign
                (new() { id }, brandIds, areaIds, state,
                propertySort, isAsc, search, page, limit);
        }
        throw new InvalidParameterException("Không tìm thấy chiến dịch");
    }

    public async Task<CampaignExtraModel> Update(string id, UpdateCampaignModel update)
    {
        Campaign entity = campaignRepository.GetById(id);
        if (entity != null)
        {
            if (new[] { CampaignState.Pending, CampaignState.Rejected }.Contains(entity.CampaignActivities.LastOrDefault().State.Value)
                || (new[] { CampaignState.Active, CampaignState.Inactive }.Contains(entity.CampaignActivities.LastOrDefault().State.Value)
                && entity.StartOn > DateOnly.FromDateTime(DateTime.Now)))
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
                return mapper.Map<CampaignExtraModel>(campaignRepository.Update(entity));
            }
            throw new InvalidParameterException("Chiến dịch đã quá hạn cập nhật");
        }
        throw new InvalidParameterException("Không tìm thấy chiến dịch");
    }

    public bool UpdateState(string id, CampaignState stateId, JwtRequestModel request)
    {
        if (!new[] { CampaignState.Pending, CampaignState.Expired }.Contains(stateId))
        {
            Campaign entity = campaignRepository.GetById(id);
            if (entity != null)
            {
                if (entity.EndOn >= DateOnly.FromDateTime(DateTime.Now))
                {
                    switch (entity.CampaignActivities.LastOrDefault().State)
                    {
                        case CampaignState.Pending
                        when request.Role.Equals("Brand") || new[] { CampaignState.Inactive, CampaignState.Closed }.Contains(stateId):
                            throw new InvalidParameterException("Trạng thái không hợp lệ cho chiến dịch");
                        case CampaignState.Active
                        when new[] { CampaignState.Rejected, CampaignState.Active }.Contains(stateId):
                            throw new InvalidParameterException("Trạng thái không hợp lệ cho chiến dịch");
                        case CampaignState.Inactive
                        when new[] { CampaignState.Rejected, CampaignState.Inactive }.Contains(stateId):
                            throw new InvalidParameterException("Trạng thái không hợp lệ cho chiến dịch");
                    }

                    // Push notification to mobile app
                    fireBaseService.PushNotificationToStudent(new Message
                    {
                        Data = new Dictionary<string, string>()
                                    {
                                        { "brandId", entity.BrandId },
                                        { "campaignId", entity.Id },
                                    },
                        //Token = registrationToken,
                        Topic = entity.BrandId,
                        Notification = new Notification()
                        {
                            Title = entity.Brand.BrandName + " tạo chiến dịch mới!",
                            Body = "Chiến dịch " + entity.CampaignName,
                            ImageUrl = entity.Image
                        }
                    });

                    if (stateId.Equals(CampaignState.Closed))
                    {
                        // Handle refund
                    }
                    return campaignActivityRepository.Add(new CampaignActivity
                    {
                        Id = Ulid.NewUlid().ToString(),
                        CampaignId = entity.Id,
                        State = stateId,
                        DateCreated = DateTime.Now,
                        Description = stateId.GetEnumDescription(),
                        Status = true,
                    }) != null;
                }
                throw new InvalidParameterException("Chiến dịch này đã kết thúc");
            }
            throw new InvalidParameterException("Không tìm thấy chiến dịch");
        }
        throw new InvalidParameterException("Trạng thái chiến dịch không hợp lệ");
    }
}
