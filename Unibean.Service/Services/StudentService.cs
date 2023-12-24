using AutoMapper;
using Unibean.Repository.Entities;
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
            .ReverseMap();
            cfg.CreateMap<Student, CreateStudentGoogleModel>()
            .ReverseMap()
            .ForMember(s => s.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(s => s.StudentCard, opt => opt.Ignore())
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

        //Upload image
        if (creation.StudentCard != null && creation.StudentCard.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.StudentCard, FOLDER_NAME);
            entity.StudentCard = f.URL;
            entity.FileName = f.FileName;
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
}
