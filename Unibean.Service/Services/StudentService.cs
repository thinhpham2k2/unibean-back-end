using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Invitations;
using Unibean.Service.Models.Orders;
using Unibean.Service.Models.StudentChallenges;
using Unibean.Service.Models.Students;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class StudentService : IStudentService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "students";

    private readonly IStudentRepository studentRepository;

    private readonly IFireBaseService fireBaseService;

    private readonly IAccountRepository accountRepository;

    private readonly IInvitationService invitationService;

    private readonly IStudentChallengeService studentChallengeService;

    public StudentService(IStudentRepository studentRepository,
        IFireBaseService fireBaseService,
        IAccountRepository accountRepository,
        IInvitationService invitationService,
        IStudentChallengeService studentChallengeService)
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
            .ForMember(s => s.Challenges, opt => opt.MapFrom(src => src.StudentChallenges))
            .ForMember(s => s.VoucherItems, opt => opt.Ignore())
            .ForMember(s => s.Transactions, opt => opt.MapFrom((src, dest) =>
            {
                return new List<object>()
                .Concat(src.Wallets.SelectMany(wallet => wallet.OrderTransactions))
                .Concat(src.Wallets.SelectMany(wallet => wallet.ActivityTransactions))
                .Concat(src.Wallets.SelectMany(wallet => wallet.ChallengeTransactions))
                .Concat(src.Wallets.SelectMany(wallet => wallet.PaymentTransactions));
            }))
            .AfterMap((src, dest) =>
            {
                dest.Transactions = dest.Transactions.OrderByDescending(t => t.DateCreated).ToList();
            })
            .ReverseMap();
            cfg.CreateMap<OrderTransaction, TransactionModel>()
            .ForMember(t => t.Name, opt => opt.MapFrom(src => "Tạo đơn hàng (" + src.Order.Amount + " đậu)"))
            .ForMember(t => t.RequestId, opt => opt.MapFrom(src => src.OrderId))
            .ForMember(t => t.WalletType, opt => opt.MapFrom(src => src.Wallet.Type.TypeName))
            .ForMember(t => t.WalletImage, opt => opt.MapFrom(src => src.Wallet.Type.Image))
            .ForMember(t => t.TypeName, opt => opt.MapFrom(src => "Đổi quà"))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => src.Order.DateCreated))
            .ReverseMap();
            cfg.CreateMap<ActivityTransaction, TransactionModel>()
            .ForMember(t => t.Name, opt => opt.MapFrom(src => src.Activity.VoucherItem.Voucher.VoucherName))
            .ForMember(t => t.RequestId, opt => opt.MapFrom(src => src.ActivityId))
            .ForMember(t => t.WalletType, opt => opt.MapFrom(src => src.Wallet.Type.TypeName))
            .ForMember(t => t.WalletImage, opt => opt.MapFrom(src => src.Wallet.Type.Image))
            .ForMember(t => t.TypeName, opt => opt.MapFrom(src => src.Activity.Type.TypeName))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => src.Activity.DateCreated))
            .ReverseMap();
            cfg.CreateMap<ChallengeTransaction, TransactionModel>()
            .ForMember(t => t.Name, opt => opt.MapFrom(src => src.Challenge.Challenge.ChallengeName))
            .ForMember(t => t.RequestId, opt => opt.MapFrom(src => src.ChallengeId))
            .ForMember(t => t.WalletType, opt => opt.MapFrom(src => src.Wallet.Type.TypeName))
            .ForMember(t => t.WalletImage, opt => opt.MapFrom(src => src.Wallet.Type.Image))
            .ForMember(t => t.TypeName, opt => opt.MapFrom(src => "Hoàn thành thử thách"))
            .ReverseMap();
            cfg.CreateMap<PaymentTransaction, TransactionModel>()
            .ForMember(t => t.Name, opt => opt.MapFrom(src => "Nạp đậu xanh cho tài khoản"))
            .ForMember(t => t.RequestId, opt => opt.MapFrom(src => src.PaymentId))
            .ForMember(t => t.WalletType, opt => opt.MapFrom(src => src.Wallet.Type.TypeName))
            .ForMember(t => t.WalletImage, opt => opt.MapFrom(src => src.Wallet.Type.Image))
            .ForMember(t => t.TypeName, opt => opt.MapFrom(src => "Thanh toán"))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => src.Payment.DateCreated))
            .ReverseMap();
            cfg.CreateMap<Order, OrderModel>()
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
        this.accountRepository = accountRepository;
        this.invitationService = invitationService;
        this.studentChallengeService = studentChallengeService;
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
}
