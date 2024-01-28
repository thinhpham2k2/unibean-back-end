﻿using AutoMapper;
using FirebaseAdmin.Messaging;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.CampaignCampuses;
using Unibean.Service.Models.CampaignMajors;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.CampaignStores;
using Unibean.Service.Models.Campuses;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Majors;
using Unibean.Service.Models.Stores;
using Unibean.Service.Models.Vouchers;
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

    private readonly IVoucherService voucherService;

    private readonly IStoreService storeService;

    private readonly IMajorService majorService;

    private readonly ICampusService campusService;

    private readonly IDiscordService discordService;

    public CampaignService(ICampaignRepository campaignRepository,
        IVoucherRepository voucherRepository,
        IFireBaseService fireBaseService,
        IVoucherService voucherService,
        IStoreService storeService,
        IMajorService majorService,
        ICampusService campusService,
        IDiscordService discordService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            // Map Campaign Model
            cfg.CreateMap<Campaign, CampaignModel>()
            .ForMember(c => c.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
            .ForMember(c => c.BrandAcronym, opt => opt.MapFrom(src => src.Brand.Acronym))
            .ForMember(c => c.TypeName, opt => opt.MapFrom(src => src.Type.TypeName))
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
            .ForMember(c => c.NumberOfParticipants, opt => opt.MapFrom(src
                => src.VoucherItems.Where(v => (bool)v.IsUsed).Count()))
            .ForMember(c => c.UsageCost, opt => opt.MapFrom(src
                => src.VoucherItems.Where(v => (bool)v.IsUsed).Select(v
                => v.Price * v.Rate).Sum()))
            .ForMember(c => c.UsageCost, opt => opt.MapFrom(src
                => src.VoucherItems.Where(v => (bool)v.IsUsed).Select(v
                => v.Price * v.Rate).Sum()))
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
            // Map Create Campaign Model
            cfg.CreateMap<Campaign, CreateCampaignModel>()
            .ReverseMap()
            .ForMember(c => c.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(c => c.Duration, opt => opt.MapFrom(src
                => ((DateOnly)src.EndOn).DayNumber - ((DateOnly)src.StartOn).DayNumber))
            .ForMember(c => c.TotalSpending, opt => opt.MapFrom(src => 0))
            .ForMember(c => c.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(c => c.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(c => c.State, opt => opt.MapFrom(src => false))
            .ForMember(c => c.Status, opt => opt.MapFrom(src => true));
            // Map Update Campaign Model
            cfg.CreateMap<Campaign, UpdateCampaignModel>()
            .ReverseMap()
            .ForMember(c => c.Type, opt => opt.MapFrom(src => (string)null))
            .ForMember(c => c.Image, opt => opt.Ignore())
            .ForMember(c => c.ImageName, opt => opt.Ignore())
            .ForMember(c => c.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
        });
        mapper = new Mapper(config);
        this.campaignRepository = campaignRepository;
        this.voucherRepository = voucherRepository;
        this.fireBaseService = fireBaseService;
        this.voucherService = voucherService;
        this.storeService = storeService;
        this.majorService = majorService;
        this.campusService = campusService;
        this.discordService = discordService;
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
        campaign.VoucherItems = new List<VoucherItem>();

        foreach (var voucher in creation.Vouchers)
        {
            var v = voucherRepository.GetById(voucher.VoucherId);
            for (var i = 0; i < voucher.Quantity; i++)
            {
                var id = Ulid.NewUlid().ToString();
                campaign.VoucherItems.Add(new VoucherItem
                {
                    Id = id,
                    VoucherId = voucher.VoucherId,
                    CampaignId = campaign.Id,
                    VoucherCode = id,
                    Price = v.Price,
                    Rate = v.Rate,
                    IsBought = false,
                    IsUsed = false,
                    ValidOn = campaign.StartOn,
                    ExpireOn = campaign.EndOn,
                    DateCreated = campaign.DateCreated,
                    Description = v.Description,
                    State = true,
                    Status = true
                });
            }
        }

        campaign = campaignRepository.Add(campaign);

        if (campaign != null)
        {
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

    public void Delete(string id)
    {
        Campaign entity = campaignRepository.GetById(id);
        if (entity != null)
        {
            if (!(bool)entity.State && entity.EndOn < DateOnly.FromDateTime(DateTime.Now))
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
            throw new InvalidParameterException("Không tìm thấy Chiến dịch");
        }
    }

    public PagedResultModel<CampaignModel> GetAll
        (List<string> brandIds, List<string> typeIds, List<string> storeIds, List<string> majorIds,
        List<string> campusIds, bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<CampaignModel>>(campaignRepository
            .GetAll(brandIds, typeIds, storeIds, majorIds, campusIds, state, 
            propertySort, isAsc, search, page, limit));
    }

    public CampaignExtraModel GetById(string id)
    {
        Campaign entity = campaignRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<CampaignExtraModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy Chiến dịch");
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
        throw new InvalidParameterException("Không tìm thấy Chiến dịch");
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
        throw new InvalidParameterException("Không tìm thấy Chiến dịch");
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
        throw new InvalidParameterException("Không tìm thấy Chiến dịch");
    }

    public PagedResultModel<VoucherModel> GetVoucherListByCampaignId
        (string id, List<string> typeIds, bool? state, 
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        Campaign entity = campaignRepository.GetById(id);
        if (entity != null)
        {
            return voucherService.GetAllByCampaign
                (new() { id }, typeIds, state, propertySort,
                isAsc, search, page, limit);
        }
        throw new InvalidParameterException("Không tìm thấy Chiến dịch");
    }

    public async Task<CampaignExtraModel> Update(string id, UpdateCampaignModel update)
    {
        Campaign entity = campaignRepository.GetById(id);
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
            return mapper.Map<CampaignExtraModel>(campaignRepository.Update(entity));
        }
        throw new InvalidParameterException("Không tìm thấy Chiến dịch");
    }

    public CampaignExtraModel UpdateState(string id)
    {
        Campaign entity = campaignRepository.GetById(id);
        if (entity != null)
        {
            if (!(bool)entity.State)
            {
                entity.State = true;

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

                return mapper.Map<CampaignExtraModel>(campaignRepository.Update(entity));
            }
            throw new InvalidParameterException("Chiến dịch này đã được phê duyệt");
        }
        throw new InvalidParameterException("Không tìm thấy Chiến dịch");
    }
}
