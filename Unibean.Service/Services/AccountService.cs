using AutoMapper;
using Google.Apis.Util;
using Microsoft.IdentityModel.Tokens;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Accounts;
using Unibean.Service.Models.Invitations;
using Unibean.Service.Models.Wallets;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Unibean.Service.Services;

public class AccountService : IAccountService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "accounts";

    private readonly string BRAND_FOLDER_NAME = "brands";

    private readonly string STUDENT_FOLDER_NAME = "students";

    private readonly IAccountRepository accountRepository;

    private readonly IFireBaseService fireBaseService;

    private readonly IRoleService roleService;

    private readonly IBrandRepository brandRepository;

    private readonly IStudentRepository studentRepository;

    private readonly ILevelService levelService;

    private readonly IWalletService walletService;

    private readonly IWalletTypeService walletTypeService;

    private readonly IInvitationService invitationService;

    public AccountService(IAccountRepository accountRepository,
        IFireBaseService fireBaseService,
        IRoleService roleService,
        IBrandRepository brandRepository,
        IStudentRepository studentRepository,
        ILevelService levelService,
        IWalletService walletService,
        IWalletTypeService walletTypeService,
        IInvitationService invitationService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Account, AccountModel>()
            .ForMember(a => a.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
            .ForMember(a => a.UserId, opt => opt.MapFrom((src, dest) =>
            {
                if (src.Role != null)
                {
                    return src.Role.RoleName switch
                    {
                        "Admin" => src.Admins.FirstOrDefault()?.Id,
                        "Brand" => src.Brands.FirstOrDefault()?.Id,
                        "Store" => src.Stores.FirstOrDefault()?.Id,
                        "Student" => src.Students.FirstOrDefault()?.Id,
                        _ => null,
                    };
                }
                return null;
            }))
            .ForMember(a => a.Name, opt => opt.MapFrom((src, dest) =>
            {
                if (src.Role != null)
                {
                    return src.Role.RoleName switch
                    {
                        "Admin" => src.Admins.FirstOrDefault()?.FullName,
                        "Brand" => src.Brands.FirstOrDefault()?.BrandName,
                        "Store" => src.Stores.FirstOrDefault()?.StoreName,
                        "Student" => src.Students.FirstOrDefault()?.FullName,
                        _ => null,
                    };
                }
                return null;
            }))
            .ReverseMap();
            cfg.CreateMap<Account, CreateGoogleAccountModel>()
           .ReverseMap()
           .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
           .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
           .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
           .ForMember(t => t.DateVerified, opt => opt.MapFrom(src => DateTime.Now))
           .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
            // Create Account and Brand
            cfg.CreateMap<Account, CreateBrandAccountModel>()
           .ReverseMap()
           .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
           .ForMember(t => t.Password, opt => opt.MapFrom(src => BCryptNet.HashPassword(src.Password)))
           .ForMember(t => t.IsVerify, opt => opt.MapFrom(src => true))
           .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
           .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
           .ForMember(t => t.DateVerified, opt => opt.MapFrom(src => DateTime.Now))
           .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
            cfg.CreateMap<Brand, CreateBrandAccountModel>()
           .ReverseMap()
           .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
           .ForMember(t => t.TotalIncome, opt => opt.MapFrom(src => 0))
           .ForMember(t => t.TotalSpending, opt => opt.MapFrom(src => 0))
           .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
           .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
           .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
            // Create Account and Student
            cfg.CreateMap<Account, CreateStudentAccountModel>()
           .ReverseMap()
           .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
           .ForMember(t => t.Password, opt => opt.MapFrom(src => BCryptNet.HashPassword(src.Password)))
           .ForMember(t => t.IsVerify, opt => opt.MapFrom(src => false))
           .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
           .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
           .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
            cfg.CreateMap<Student, CreateStudentAccountModel>()
           .ReverseMap()
           .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
           .ForMember(s => s.TotalIncome, opt => opt.MapFrom(src => 0))
           .ForMember(s => s.TotalSpending, opt => opt.MapFrom(src => 0))
           .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
           .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
           .ForMember(t => t.Status, opt => opt.MapFrom(src => true));

        });
        mapper = new Mapper(config);
        this.accountRepository = accountRepository;
        this.fireBaseService = fireBaseService;
        this.roleService = roleService;
        this.brandRepository = brandRepository;
        this.studentRepository = studentRepository;
        this.levelService = levelService;
        this.walletService = walletService;
        this.walletTypeService = walletTypeService;
        this.invitationService = invitationService;
    }

    public async Task<AccountModel> AddBrand(CreateBrandAccountModel creation)
    {
        Account account = mapper.Map<Account>(creation);
        // Set role
        account.RoleId = roleService.GetRoleByName("Brand")?.Id;

        // Upload the cover photo
        if (creation.Logo != null && creation.Logo.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Logo, FOLDER_NAME);
            account.Avatar = f.URL;
            account.FileName = f.FileName;
        }

        account = accountRepository.Add(account);

        Brand brand = mapper.Map<Brand>(creation);
        // Set account Id
        brand.AccountId = account.Id;

        // Upload the cover photo
        if (creation.CoverPhoto != null && creation.CoverPhoto.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.CoverPhoto, BRAND_FOLDER_NAME);
            brand.CoverPhoto = f.URL;
            brand.CoverFileName = f.FileName;
        }

        brand = brandRepository.Add(brand);

        // Create wallet
        if (brand != null)
        {
            walletService.Add(new CreateWalletModel
            {
                BrandId = brand.Id,
                TypeId = walletTypeService.GetWalletTypeByName("Green bean")?.Id,
                Balance = 0,
                Description = null,
                State = true
            });
            walletService.Add(new CreateWalletModel
            {
                BrandId = brand.Id,
                TypeId = walletTypeService.GetWalletTypeByName("Red bean")?.Id,
                Balance = 0,
                Description = null,
                State = true
            });
        }

        return mapper.Map<AccountModel>(account);
    }

    public AccountModel AddGoogle(CreateGoogleAccountModel creation)
    {
        return mapper.Map<AccountModel>(accountRepository.Add(mapper.Map<Account>(creation)));
    }

    public async Task<AccountModel> AddStudent(CreateStudentAccountModel creation)
    {
        Account account = mapper.Map<Account>(creation);
        // Set role
        account.RoleId = roleService.GetRoleByName("Student")?.Id;
        account = accountRepository.Add(account);

        Student student = mapper.Map<Student>(creation);
        // Set account
        student.AccountId = account.Id;

        // Set level
        student.LevelId = levelService.GetLevelByName("Iron")?.Id;

        // Upload the student card front image
        if (creation.StudentCardFront != null && creation.StudentCardFront.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.StudentCardFront, STUDENT_FOLDER_NAME);
            student.StudentCardFront = f.URL;
            student.FileNameFront = f.FileName;
        }

        // Upload the student card back image
        if (creation.StudentCardBack != null && creation.StudentCardBack.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.StudentCardBack, STUDENT_FOLDER_NAME);
            student.StudentCardBack = f.URL;
            student.FileNameBack = f.FileName;
        }

        student = studentRepository.Add(student);

        if (student != null)
        {
            // Create wallet
            walletService.Add(new CreateWalletModel
            {
                StudentId = student.Id,
                TypeId = walletTypeService.GetWalletTypeByName("Green bean")?.Id,
                Balance = 0,
                Description = null,
                State = true
            });
            walletService.Add(new CreateWalletModel
            {
                StudentId = student.Id,
                TypeId = walletTypeService.GetWalletTypeByName("Red bean")?.Id,
                Balance = 0,
                Description = null,
                State = true
            });

            // Set invitation
            if (creation.InviteCode.IsNullOrEmpty())
            {
                invitationService.Add(new CreateInvitationModel
                {
                    InviterId = creation.InviteCode,
                    InviteeId = student.Id,
                    Description = null,
                    State = true
                });
            }

            // Take the challenge
        }

        return mapper.Map<AccountModel>(account);
    }

    public AccountModel GetByEmail(string email)
    {
        return mapper.Map<AccountModel>(accountRepository.GetByEmail(email));
    }

    public AccountModel GetByUserNameAndPassword(string userName, string password)
    {
        return mapper.Map<AccountModel>(accountRepository.GetByUserNameAndPassword(userName, password));
    }
}
