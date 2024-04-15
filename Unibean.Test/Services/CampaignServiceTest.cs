using FakeItEasy;
using FirebaseAdmin.Messaging;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Activities;
using Unibean.Service.Models.Authens;
using Unibean.Service.Models.CampaignActivities;
using Unibean.Service.Models.CampaignDetails;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.Campuses;
using Unibean.Service.Models.Majors;
using Unibean.Service.Models.Stores;
using Unibean.Service.Models.WebHooks;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class CampaignServiceTest
{
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

    private readonly IStudentChallengeService studentChallengeService;

    private readonly IEmailService emailService;

    private readonly IBrandRepository brandRepository;

    public CampaignServiceTest()
    {
        campaignRepository = A.Fake<ICampaignRepository>();
        voucherRepository = A.Fake<IVoucherRepository>();
        fireBaseService = A.Fake<IFireBaseService>();
        storeService = A.Fake<IStoreService>();
        majorService = A.Fake<IMajorService>();
        campusService = A.Fake<ICampusService>();
        discordService = A.Fake<IDiscordService>();
        activityService = A.Fake<IActivityService>();
        voucherItemRepository = A.Fake<IVoucherItemRepository>();
        studentRepository = A.Fake<IStudentRepository>();
        campaignDetailService = A.Fake<ICampaignDetailService>();
        campaignActivityService = A.Fake<ICampaignActivityService>();
        campaignActivityRepository = A.Fake<ICampaignActivityRepository>();
        studentChallengeService = A.Fake<IStudentChallengeService>();
        emailService = A.Fake<IEmailService>();
        brandRepository = A.Fake<IBrandRepository>();
    }

    [Fact]
    public void CampaignService_Add()
    {
        // Arrange
        string id = "id";
        int duration = 100;
        string campaignName = "campaignName";
        string brandName = "brandName";
        string link = "link";
        string avatar = "avatar";
        string image = "image";
        CreateCampaignModel creation = new()
        {
            StartOn = DateOnly.FromDateTime(DateTime.Now),
            EndOn = DateOnly.FromDateTime(DateTime.Now).AddDays(100)
        };
        A.CallTo(() => campaignRepository.Add(A<Campaign>.Ignored)).Returns(new()
        {
            Id = id,
            Duration = duration,
            CampaignName = campaignName,
            Brand = new()
            {
                BrandName = brandName,
                Link = link,
                Account = new()
                {
                    Avatar = avatar,
                }
            },
            TotalIncome = 0,
            Image = image,
            DateCreated = DateTime.Now,
            StartOn = DateOnly.FromDateTime(DateTime.Now),
            EndOn = DateOnly.FromDateTime(DateTime.Now).AddDays(100),
            CampaignDetails = new List<CampaignDetail>()
        });
        A.CallTo(() => brandRepository.GetById(A<string>.Ignored)).Returns(new()
        {
            Account = new()
        });
        A.CallTo(() => discordService.CreateWebHooks(A<DiscordWebhookModel>.Ignored));
        var service = new CampaignService(campaignRepository, voucherRepository,
             fireBaseService, storeService, majorService, campusService, discordService,
             activityService, voucherItemRepository, studentRepository, campaignDetailService,
             campaignActivityService, campaignActivityRepository, studentChallengeService, emailService, brandRepository);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<CampaignExtraModel>));
        Assert.Equal(id, result.Result.Id);
        Assert.Equal(duration, result.Result.Duration);
    }

    [Fact]
    public void CampaignService_AddActivity()
    {
        // Arrange
        string id = "id";
        string detailId = "detailId";
        CreateBuyActivityModel creation = new()
        {
            StudentId = id,
            Quantity = 1,
        };
        A.CallTo(() => campaignRepository.GetById(id)).Returns(new()
        {
            Id = id,
            StartOn = DateOnly.FromDateTime(DateTime.Now),
            EndOn = DateOnly.FromDateTime(DateTime.Now).AddDays(100)
        });
        A.CallTo(() => campaignDetailService.GetById(detailId)).Returns(new()
        {
            CampaignId = id,
            Price = 0
        });
        A.CallTo(() => campaignDetailService.GetAllVoucherItemByCampaignDetail(detailId)).Returns(new()
        {
            ""
        });
        A.CallTo(() => studentRepository.GetById(creation.StudentId)).Returns(new()
        {
            Id = creation.StudentId,
            Wallets = new List<Wallet>()
            {
                new()
                {
                    Type = WalletType.Green,
                    Balance = 0
                }
            },
            StudentChallenges = new List<StudentChallenge>()
        });
        A.CallTo(() => studentChallengeService.Update(A<List<StudentChallenge>>.Ignored, A<decimal>.Ignored));
        var service = new CampaignService(campaignRepository, voucherRepository,
             fireBaseService, storeService, majorService, campusService, discordService,
             activityService, voucherItemRepository, studentRepository, campaignDetailService,
             campaignActivityService, campaignActivityRepository, studentChallengeService, emailService, brandRepository);

        // Act & Assert
        var result = service.AddActivity(id, detailId, creation);
    }

    [Fact]
    public void CampaignService_Delete()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => campaignRepository.GetById(id)).Returns(new()
        {
            Id = id,
            CampaignActivities = new List<CampaignActivity>()
            {
                new()
                {
                    State = CampaignState.Closed
                }
            },
        });
        A.CallTo(() => campaignRepository.Delete(id));
        var service = new CampaignService(campaignRepository, voucherRepository,
             fireBaseService, storeService, majorService, campusService, discordService,
             activityService, voucherItemRepository, studentRepository, campaignDetailService,
             campaignActivityService, campaignActivityRepository, studentChallengeService, emailService, brandRepository);

        // Act & Assert
        service.Delete(id);
    }

    [Fact]
    public void CampaignService_GetAll()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> typeIds = new();
        List<string> storeIds = new();
        List<string> majorIds = new();
        List<string> campusIds = new();
        List<CampaignState> stateIds = new();
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<Campaign> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => campaignRepository.GetAll(brandIds, typeIds, storeIds, majorIds, campusIds,
            stateIds, propertySort, isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new CampaignService(campaignRepository, voucherRepository,
             fireBaseService, storeService, majorService, campusService, discordService,
             activityService, voucherItemRepository, studentRepository, campaignDetailService,
             campaignActivityService, campaignActivityRepository, studentChallengeService, emailService, brandRepository);

        // Act
        var result = service.GetAll(brandIds, typeIds, storeIds, majorIds, campusIds,
            stateIds, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<CampaignModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void CampaignService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => campaignRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        var service = new CampaignService(campaignRepository, voucherRepository,
             fireBaseService, storeService, majorService, campusService, discordService,
             activityService, voucherItemRepository, studentRepository, campaignDetailService,
             campaignActivityService, campaignActivityRepository, studentChallengeService, emailService, brandRepository);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(CampaignExtraModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void CampaignService_GetCampaignActivityListByCampaignId()
    {
        // Arrange
        string id = "id";
        List<CampaignState> stateIds = new();
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<CampaignActivityModel> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => campaignRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        A.CallTo(() => campaignActivityService.GetAll(new() { id }, stateIds, propertySort,
                isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new CampaignService(campaignRepository, voucherRepository,
             fireBaseService, storeService, majorService, campusService, discordService,
             activityService, voucherItemRepository, studentRepository, campaignDetailService,
             campaignActivityService, campaignActivityRepository, studentChallengeService, emailService, brandRepository);

        // Act
        var result = service.GetCampaignActivityListByCampaignId(id, stateIds, propertySort,
                isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<CampaignActivityModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void CampaignService_GetCampaignDetailById()
    {
        // Arrange
        string id = "id";
        string detailId = "detailId";
        A.CallTo(() => campaignRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        A.CallTo(() => campaignDetailService.GetById(detailId))
            .Returns(new()
            {
                Id = detailId,
                CampaignId = id,
            });
        var service = new CampaignService(campaignRepository, voucherRepository,
             fireBaseService, storeService, majorService, campusService, discordService,
             activityService, voucherItemRepository, studentRepository, campaignDetailService,
             campaignActivityService, campaignActivityRepository, studentChallengeService, emailService, brandRepository);

        // Act
        var result = service.GetCampaignDetailById(id, detailId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(CampaignDetailExtraModel));
        Assert.Equal(id, result.CampaignId);
        Assert.Equal(detailId, result.Id);
    }

    [Fact]
    public void CampaignService_GetCampaignDetailListByCampaignId()
    {
        // Arrange
        string id = "id";
        List<string> typeIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<CampaignDetailModel> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => campaignRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        A.CallTo(() => campaignDetailService.GetAll(new() { id }, typeIds, state, propertySort,
                isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new CampaignService(campaignRepository, voucherRepository,
             fireBaseService, storeService, majorService, campusService, discordService,
             activityService, voucherItemRepository, studentRepository, campaignDetailService,
             campaignActivityService, campaignActivityRepository, studentChallengeService, emailService, brandRepository);

        // Act
        var result = service.GetCampaignDetailListByCampaignId(id, typeIds, state, propertySort,
                isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<CampaignDetailModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void CampaignService_GetCampusListByCampaignId()
    {
        // Arrange
        string id = "id";
        List<string> universityIds = new();
        List<string> areaIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<CampusModel> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => campaignRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        A.CallTo(() => campusService.GetAllByCampaign(new() { id }, universityIds, areaIds, state,
            propertySort, isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new CampaignService(campaignRepository, voucherRepository,
             fireBaseService, storeService, majorService, campusService, discordService,
             activityService, voucherItemRepository, studentRepository, campaignDetailService,
             campaignActivityService, campaignActivityRepository, studentChallengeService, emailService, brandRepository);

        // Act
        var result = service.GetCampusListByCampaignId(id, universityIds, areaIds, state,
            propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<CampusModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void CampaignService_GetMajorListByCampaignId()
    {
        // Arrange
        string id = "id";
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<MajorModel> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => campaignRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        A.CallTo(() => majorService.GetAllByCampaign(new() { id }, state,
            propertySort, isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new CampaignService(campaignRepository, voucherRepository,
             fireBaseService, storeService, majorService, campusService, discordService,
             activityService, voucherItemRepository, studentRepository, campaignDetailService,
             campaignActivityService, campaignActivityRepository, studentChallengeService, emailService, brandRepository);

        // Act
        var result = service.GetMajorListByCampaignId(id, state, propertySort, isAsc,
            search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<MajorModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void CampaignService_GetStoreListByCampaignId()
    {
        // Arrange
        string id = "id";
        List<string> brandIds = new();
        List<string> areaIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<StoreModel> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => campaignRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        A.CallTo(() => storeService.GetAllByCampaign(new() { id }, brandIds, areaIds, state,
            propertySort, isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new CampaignService(campaignRepository, voucherRepository,
             fireBaseService, storeService, majorService, campusService, discordService,
             activityService, voucherItemRepository, studentRepository, campaignDetailService,
             campaignActivityService, campaignActivityRepository, studentChallengeService, emailService, brandRepository);

        // Act
        var result = service.GetStoreListByCampaignId(id, brandIds, areaIds, state, propertySort,
            isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<StoreModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void CampaignService_Update()
    {
        // Arrange
        string id = "id";
        UpdateCampaignModel update = A.Fake<UpdateCampaignModel>();
        A.CallTo(() => campaignRepository.GetById(id)).Returns(new()
        {
            StartOn = DateOnly.FromDateTime(DateTime.Now).AddDays(3),
            EndOn = DateOnly.FromDateTime(DateTime.Now).AddDays(100),
            Brand = new()
            {
                Account = new()
                {
                }
            },
            CampaignActivities = new List<CampaignActivity>()
            {
                new()
                {
                    State = CampaignState.Active
                }
            },
        });
        A.CallTo(() => campaignRepository.Update(A<Campaign>.Ignored))
            .Returns(new()
            {
                Id = id,
            });
        var service = new CampaignService(campaignRepository, voucherRepository,
             fireBaseService, storeService, majorService, campusService, discordService,
             activityService, voucherItemRepository, studentRepository, campaignDetailService,
             campaignActivityService, campaignActivityRepository, studentChallengeService, emailService, brandRepository);

        // Act
        var result = service.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<CampaignExtraModel>));
        Assert.Equal(id, result.Result.Id);
    }

    [Fact]
    public void CampaignService_UpdateState()
    {
        // Arrange
        string id = "id";
        string note = "note";
        CampaignState stateId = CampaignState.Inactive;
        JwtRequestModel request = A.Fake<JwtRequestModel>();
        A.CallTo(() => campaignRepository.GetById(id)).Returns(new()
        {
            StartOn = DateOnly.FromDateTime(DateTime.Now).AddDays(3),
            EndOn = DateOnly.FromDateTime(DateTime.Now).AddDays(100),
            Brand = new()
            {
                Account = new()
                {
                }
            },
            CampaignActivities = new List<CampaignActivity>()
            {
                new()
                {
                    State = CampaignState.Active
                }
            },
        });
        A.CallTo(() => fireBaseService.PushNotificationToStudent(A<Message>.Ignored));
        A.CallTo(() => emailService.SendEmailCamapaignClose(A<List<string>>.Ignored, A<string>.Ignored));
        A.CallTo(() => campaignRepository.ExpiredToClosed(id));
        A.CallTo(() => campaignActivityRepository.Add(A<CampaignActivity>.Ignored));
        var service = new CampaignService(campaignRepository, voucherRepository,
             fireBaseService, storeService, majorService, campusService, discordService,
             activityService, voucherItemRepository, studentRepository, campaignDetailService,
             campaignActivityService, campaignActivityRepository, studentChallengeService, emailService, brandRepository);

        // Act & Assert
        var result = service.UpdateState(id, stateId, note, request);
    }
}
