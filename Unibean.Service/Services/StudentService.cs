using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Students;
using Unibean.Service.Models.Wallets;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class StudentService : IStudentService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "students";

    private readonly IStudentRepository studentRepository;

    private readonly IFireBaseService fireBaseService;

    private readonly IWalletService walletService;

    private readonly IWalletTypeService walletTypeService;

    private readonly ILevelService levelService;

    private readonly IAccountRepository accountRepository;

    public StudentService(IStudentRepository studentRepository, 
        IFireBaseService fireBaseService, 
        IWalletService walletService,
        IWalletTypeService walletTypeService,
        ILevelService levelService,
        IAccountRepository accountRepository)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Student, StudentModel>()
            .ForMember(s => s.LevelName, opt => opt.MapFrom(src => src.Level.LevelName))
            .ForMember(s => s.GenderName, opt => opt.MapFrom(src => src.Gender.GenderName))
            .ForMember(s => s.MajorName, opt => opt.MapFrom(src => src.Major.MajorName))
            .ForMember(s => s.CampusName, opt => opt.MapFrom(src => src.Campus.CampusName))
            .ForMember(s => s.UserName, opt => opt.MapFrom(src => src.Account.UserName))
            .ForMember(s => s.Email, opt => opt.MapFrom(src => src.Account.Email))
            .ForMember(s => s.Phone, opt => opt.MapFrom(src => src.Account.Phone))
            .ForMember(s => s.Avatar, opt => opt.MapFrom(src => src.Account.Avatar))
            .ForMember(s => s.ImageName, opt => opt.MapFrom(src => src.Account.FileName))
            .ForMember(s => s.DateVerified, opt => opt.MapFrom(src => src.Account.DateVerified))
            .ForMember(s => s.IsVerify, opt => opt.MapFrom(src => src.Account.IsVerify))
            .ForMember(s => s.GreenWallet, opt => opt.MapFrom(src=> src.Wallets.Where(w 
                => w.Type.TypeName.Equals("Green bean")).FirstOrDefault().Balance))
            .ForMember(s => s.RedWallet, opt => opt.MapFrom(src => src.Wallets.Where(w 
                => w.Type.TypeName.Equals("Red bean")).FirstOrDefault().Balance))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Student>, PagedResultModel<StudentModel>>()
            .ReverseMap();
            cfg.CreateMap<Student, CreateStudentGoogleModel>()
            .ReverseMap()
            .ForMember(s => s.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(s => s.TotalIncome, opt => opt.MapFrom(src => 0))
            .ForMember(s => s.TotalSpending, opt => opt.MapFrom(src => 0))
            .ForMember(s => s.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(s => s.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(s => s.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.studentRepository = studentRepository;
        this.fireBaseService = fireBaseService;
        this.walletService = walletService;
        this.walletTypeService = walletTypeService;
        this.levelService = levelService;
        this.accountRepository = accountRepository;
    }

    public async Task<StudentModel> AddGoogle(CreateStudentGoogleModel creation)
    {
        Student entity = mapper.Map<Student>(creation);

        Account account = accountRepository.GetById(creation.AccountId);

        if (!account.Email.Equals(creation.Email) || !account.Role.RoleName.Equals("Student"))
        {
            throw new InvalidParameterException("Logging in with your Google account is not valid");
        }

        account.Phone = creation.Phone;
        account.Email = creation.Email;
        account.State = true;
        accountRepository.Update(account);

        // Upload the student card front image
        if (creation.StudentCardFront != null && creation.StudentCardFront.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.StudentCardFront, FOLDER_NAME);
            entity.StudentCardFront = f.URL;
            entity.FileNameFront = f.FileName;
        }

        // Upload the student card back image
        if (creation.StudentCardBack != null && creation.StudentCardBack.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.StudentCardBack, FOLDER_NAME);
            entity.StudentCardBack = f.URL;
            entity.FileNameBack = f.FileName;
        }

        // Set level
        entity.LevelId = levelService.GetLevelByName("Iron")?.Id;

        entity = studentRepository.Add(entity);

        // Create wallet
        if (entity != null)
        {
            walletService.Add(new CreateWalletModel
            {
                StudentId = entity.Id,
                TypeId = walletTypeService.GetWalletTypeByName("Green bean")?.Id,
                Balance = 0,
                Description = null,
                State = true
            });
            walletService.Add(new CreateWalletModel
            {
                StudentId = entity.Id,
                TypeId = walletTypeService.GetWalletTypeByName("Red bean")?.Id,
                Balance = 0,
                Description = null,
                State = true
            });
        }
        return mapper.Map<StudentModel>(entity);
    }

    public PagedResultModel<StudentModel> GetAll
        (List<string> levelIds, List<string> genderIds, List<string> majorIds, List<string> campusIds, bool? isVerify, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<StudentModel>>
            (studentRepository.GetAll(levelIds, genderIds, majorIds, campusIds, isVerify, propertySort, isAsc, search, page, limit));
    }
}
