using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Invitations;
using Unibean.Service.Models.StudentChallenges;
using Unibean.Service.Models.Students;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;
using BCryptNet = BCrypt.Net.BCrypt;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Unibean.Service.Services;

public class StudentService : IStudentService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "students";

    private readonly string ACCOUNT_FOLDER_NAME = "accounts";

    private readonly IStudentRepository studentRepository;

    private readonly IFireBaseService fireBaseService;

    private readonly IAccountRepository accountRepository;

    private readonly IInvitationService invitationService;

    private readonly IStudentChallengeService studentChallengeService;

    private readonly IRoleService roleService;

    private readonly IChallengeTransactionService challengeTransactionService;

    private readonly IOrderTransactionService orderTransactionService;

    private readonly IPaymentTransactionService paymentTransactionService;

    private readonly IActivityTransactionService activityTransactionService;

    public StudentService(IStudentRepository studentRepository,
        IFireBaseService fireBaseService,
        IAccountRepository accountRepository,
        IInvitationService invitationService,
        IStudentChallengeService studentChallengeService,
        IRoleService roleService,
        IChallengeTransactionService challengeTransactionService,
        IOrderTransactionService orderTransactionService,
        IPaymentTransactionService paymentTransactionService,
        IActivityTransactionService activityTransactionService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Student, StudentModel>()
            .ForMember(s => s.LevelName, opt => opt.MapFrom(src => src.Level.LevelName))
            .ForMember(s => s.GenderName, opt => opt.MapFrom(src => src.Gender.GenderName))
            .ForMember(s => s.MajorName, opt => opt.MapFrom(src => src.Major.MajorName))
            .ForMember(s => s.CampusName, opt => opt.MapFrom(src => src.Campus.CampusName))
            .ForMember(s => s.InviteCode, opt => opt.MapFrom(src => src.Id))
            .ForMember(s => s.UserName, opt => opt.MapFrom(src => src.Account.UserName))
            .ForMember(s => s.Email, opt => opt.MapFrom(src => src.Account.Email))
            .ForMember(s => s.Phone, opt => opt.MapFrom(src => src.Account.Phone))
            .ForMember(s => s.Avatar, opt => opt.MapFrom(src => src.Account.Avatar))
            .ForMember(s => s.ImageName, opt => opt.MapFrom(src => src.Account.FileName))
            .ForMember(s => s.DateVerified, opt => opt.MapFrom(src => src.Account.DateVerified))
            .ForMember(s => s.IsVerify, opt => opt.MapFrom(src => src.Account.IsVerify))
            .ForMember(s => s.GreenWallet, opt => opt.MapFrom(src => src.Wallets.FirstOrDefault().Balance))
            .ForMember(s => s.RedWallet, opt => opt.MapFrom(src => src.Wallets.Skip(1).FirstOrDefault().Balance))
            .ReverseMap();
            cfg.CreateMap<Student, StudentExtraModel>()
            .ForMember(s => s.LevelName, opt => opt.MapFrom(src => src.Level.LevelName))
            .ForMember(s => s.GenderName, opt => opt.MapFrom(src => src.Gender.GenderName))
            .ForMember(s => s.MajorName, opt => opt.MapFrom(src => src.Major.MajorName))
            .ForMember(s => s.CampusName, opt => opt.MapFrom(src => src.Campus.CampusName))
            .ForMember(s => s.InviteCode, opt => opt.MapFrom(src => src.Id))
            .ForMember(s => s.UserName, opt => opt.MapFrom(src => src.Account.UserName))
            .ForMember(s => s.Email, opt => opt.MapFrom(src => src.Account.Email))
            .ForMember(s => s.Phone, opt => opt.MapFrom(src => src.Account.Phone))
            .ForMember(s => s.Avatar, opt => opt.MapFrom(src => src.Account.Avatar))
            .ForMember(s => s.ImageName, opt => opt.MapFrom(src => src.Account.FileName))
            .ForMember(s => s.DateVerified, opt => opt.MapFrom(src => src.Account.DateVerified))
            .ForMember(s => s.IsVerify, opt => opt.MapFrom(src => src.Account.IsVerify))
            .ForMember(s => s.GreenWallet, opt => opt.MapFrom(src => src.Wallets.FirstOrDefault().Balance))
            .ForMember(s => s.RedWallet, opt => opt.MapFrom(src => src.Wallets.Skip(1).FirstOrDefault().Balance))
            .ForMember(s => s.Following, opt => opt.MapFrom(src => src.Wishlists.Count))
            .ForMember(s => s.Inviter, opt => opt.MapFrom(src => src.Invitees.FirstOrDefault().Inviter.FullName))
            .ForMember(s => s.Invitee, opt => opt.MapFrom(src => src.Inviters.Count))
            .ReverseMap();
            cfg.CreateMap<StudentChallenge, StudentChallengeModel>()
            .ForMember(c => c.StudentName, opt => opt.MapFrom(src => src.Student.FullName))
            .ForMember(c => c.ChallengeType, opt => opt.MapFrom(src => src.Challenge.Type.TypeName))
            .ForMember(c => c.ChallengeName, opt => opt.MapFrom(src => src.Challenge.ChallengeName))
            .ForMember(c => c.ChallengeImage, opt => opt.MapFrom(src => src.Challenge.Image))
            .ForMember(c => c.IsClaimed, opt => opt.MapFrom(src => src.ChallengeTransactions.Any()))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Student>, PagedResultModel<StudentModel>>()
            .ReverseMap();
            // Map Create Student Model
            cfg.CreateMap<Student, CreateStudentGoogleModel>()
            .ReverseMap()
            .ForMember(s => s.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(s => s.TotalIncome, opt => opt.MapFrom(src => 0))
            .ForMember(s => s.TotalSpending, opt => opt.MapFrom(src => 0))
            .ForMember(s => s.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(s => s.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(s => s.Status, opt => opt.MapFrom(src => true));
            cfg.CreateMap<Student, CreateStudentModel>()
            .ReverseMap()
            .ForMember(s => s.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(s => s.TotalIncome, opt => opt.MapFrom(src => 0))
            .ForMember(s => s.TotalSpending, opt => opt.MapFrom(src => 0))
            .ForMember(s => s.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(s => s.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(s => s.Status, opt => opt.MapFrom(src => true));
            cfg.CreateMap<Account, CreateStudentModel>()
            .ReverseMap()
            .ForMember(s => s.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(s => s.Password, opt => opt.MapFrom(src => BCryptNet.HashPassword(src.Password)))
            .ForMember(s => s.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(s => s.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(s => s.Status, opt => opt.MapFrom(src => true))
            .AfterMap((src, dest) =>
            {
                dest.DateVerified = (bool)src.IsVerify ? DateTime.Now : null;
            });
            // Map Update Student Model
            cfg.CreateMap<Student, UpdateStudentModel>()
            .ReverseMap()
            .ForMember(t => t.Gender, opt => opt.MapFrom(src => (string)null))
            .ForMember(t => t.Major, opt => opt.MapFrom(src => (string)null))
            .ForMember(t => t.Campus, opt => opt.MapFrom(src => (string)null))
            .ForMember(s => s.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
        });
        mapper = new Mapper(config);
        this.studentRepository = studentRepository;
        this.fireBaseService = fireBaseService;
        this.accountRepository = accountRepository;
        this.invitationService = invitationService;
        this.studentChallengeService = studentChallengeService;
        this.roleService = roleService;
        this.challengeTransactionService = challengeTransactionService;
        this.orderTransactionService = orderTransactionService;
        this.paymentTransactionService = paymentTransactionService;
        this.activityTransactionService = activityTransactionService;
    }

    public async Task<StudentModel> Add(CreateStudentModel creation)
    {
        Account account = mapper.Map<Account>(creation);
        account.RoleId = roleService.GetRoleByName("Student")?.Id;

        //Upload avatar
        if (creation.Avatar != null && creation.Avatar.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Avatar, ACCOUNT_FOLDER_NAME);
            account.Avatar = f.URL;
            account.FileName = f.FileName;
        }

        account = accountRepository.Add(account);
        Student student = mapper.Map<Student>(creation);
        student.AccountId = account.Id;

        // Upload the student card front image
        if (creation.StudentCardFront != null && creation.StudentCardFront.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.StudentCardFront, FOLDER_NAME);
            student.StudentCardFront = f.URL;
            student.FileNameFront = f.FileName;
        }

        // Upload the student card back image
        if (creation.StudentCardBack != null && creation.StudentCardBack.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.StudentCardBack, FOLDER_NAME);
            student.StudentCardBack = f.URL;
            student.FileNameBack = f.FileName;
        }

        student = studentRepository.Add(student);

        if (student != null)
        {
            // Set invitation
            if (!creation.InviteCode.IsNullOrEmpty())
            {
                invitationService.Add(new CreateInvitationModel
                {
                    InviterId = creation.InviteCode,
                    InviteeId = student.Id,
                    Description = null,
                    State = true
                });

                // Take the challenge
                studentChallengeService.Update(studentRepository
                    .GetById(student.Id).StudentChallenges
                    .Where(s => s.Status.Equals(true)
                    && s.IsCompleted.Equals(false)
                    && s.Challenge.Type.TypeName.Equals("Welcome")), 1);

                studentChallengeService.Update(studentRepository
                    .GetById(creation.InviteCode).StudentChallenges
                    .Where(s => s.Status.Equals(true)
                    && s.IsCompleted.Equals(false)
                    && s.Challenge.Type.TypeName.Equals("Spread")), 1);
            }
        }

        return mapper.Map<StudentModel>(student);
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

        entity = studentRepository.Add(entity);

        if (entity != null)
        {
            // Set invitation
            if (!creation.InviteCode.IsNullOrEmpty())
            {
                invitationService.Add(new CreateInvitationModel
                {
                    InviterId = creation.InviteCode,
                    InviteeId = entity.Id,
                    Description = null,
                    State = true
                });

                // Take the challenge
                studentChallengeService.Update(studentRepository
                    .GetById(entity.Id).StudentChallenges
                    .Where(s => s.Status.Equals(true)
                    && s.IsCompleted.Equals(false)
                    && s.Challenge.Type.TypeName.Equals("Welcome")), 1);

                studentChallengeService.Update(studentRepository
                    .GetById(creation.InviteCode).StudentChallenges
                    .Where(s => s.Status.Equals(true)
                    && s.IsCompleted.Equals(false)
                    && s.Challenge.Type.TypeName.Equals("Spread")), 1);
            }
        }

        return mapper.Map<StudentModel>(entity);
    }

    public void Delete(string id)
    {
        Student entity = studentRepository.GetById(id);
        if (entity != null)
        {
            // Student card front image
            if (entity.StudentCardFront != null && entity.StudentCardFront.Length > 0)
            {
                // Remove image
                fireBaseService.RemoveFileAsync(entity.FileNameFront, FOLDER_NAME);
            }

            // Student card back image
            if (entity.StudentCardBack != null && entity.StudentCardBack.Length > 0)
            {
                // Remove image
                fireBaseService.RemoveFileAsync(entity.FileNameBack, FOLDER_NAME);
            }

            // Avatar
            if (entity.Account.Avatar != null && entity.Account.Avatar.Length > 0)
            {
                // Remove image
                fireBaseService.RemoveFileAsync(entity.Account.FileName, ACCOUNT_FOLDER_NAME);
            }

            studentRepository.Delete(id);
            accountRepository.Delete(entity.Account.Id);
        }
        else
        {
            throw new InvalidParameterException("Not found student");
        }
    }

    public PagedResultModel<StudentModel> GetAll
        (List<string> levelIds, List<string> genderIds, List<string> majorIds, List<string> campusIds, bool? isVerify, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<StudentModel>>
            (studentRepository.GetAll(levelIds, genderIds, majorIds, campusIds, isVerify, propertySort, isAsc, search, page, limit));
    }

    public StudentExtraModel GetById(string id)
    {
        Student entity = studentRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<StudentExtraModel>(entity);
        }
        throw new InvalidParameterException("Not found student");
    }

    public PagedResultModel<TransactionModel> GetHistoryTransactionByStudentId
        (string id, string propertySort, bool isAsc, string search, int page, int limit)
    {
        Student entity = studentRepository.GetById(id);

        if (entity != null)
        {
            var query = challengeTransactionService.GetAll
                (studentRepository.GetById(id).Wallets.Select(w => w.Id).ToList(), new(), search)
                .Concat(orderTransactionService.GetAll
                (studentRepository.GetById(id).Wallets.Select(w => w.Id).ToList(), new(), search))
                .Concat(paymentTransactionService.GetAll
                (studentRepository.GetById(id).Wallets.Select(w => w.Id).ToList(), new(), search))
                .Concat(activityTransactionService.GetAll
                (studentRepository.GetById(id).Wallets.Select(w => w.Id).ToList(), new(), search))
                .AsQueryable()
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
        throw new InvalidParameterException("Not found student");
    }

    public static bool CompareStrings(string str1, string str2)
    {
        return string.Equals(
            str1.Normalize(NormalizationForm.FormD),
            str2.Normalize(NormalizationForm.FormD),
            StringComparison.OrdinalIgnoreCase);
    }

    public async Task<StudentModel> Update(string id, UpdateStudentModel update)
    {
        Student entity = studentRepository.GetById(id);
        if (entity != null)
        {
            entity = mapper.Map(update, entity);

            //Upload avatar
            if (update.Avatar != null && update.Avatar.Length > 0)
            {
                // Remove image
                await fireBaseService.RemoveFileAsync(entity.Account.FileName, ACCOUNT_FOLDER_NAME);

                //Upload new image update
                FireBaseFile f = await fireBaseService.UploadFileAsync(update.Avatar, ACCOUNT_FOLDER_NAME);
                entity.Account.Avatar = f.URL;
                entity.Account.FileName = f.FileName;
            }

            entity.Account.DateUpdated = DateTime.Now;
            accountRepository.Update(entity.Account);

            return mapper.Map<StudentModel>(studentRepository.Update(entity));
        }
        throw new InvalidParameterException("Not found student");
    }
}
