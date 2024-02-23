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
using Unibean.Service.Models.Orders;
using Unibean.Service.Models.VoucherItems;
using Enable.EnumDisplayName;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Unibean.Service.Services;

public enum TransactionType
{
    ActivityTransaction = 1,
    OrderTransaction = 2,
    ChallengeTransaction = 3,
    BonusTransaction = 4
}

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

    private readonly IChallengeTransactionService challengeTransactionService;

    private readonly IOrderTransactionService orderTransactionService;

    private readonly IBonusTransactionService bonusTransactionService;

    private readonly IActivityTransactionService activityTransactionService;

    private readonly IOrderService orderService;

    private readonly IVoucherItemService voucherItemService;

    private readonly IEmailService emailService;

    public StudentService(IStudentRepository studentRepository,
        IFireBaseService fireBaseService,
        IAccountRepository accountRepository,
        IInvitationService invitationService,
        IStudentChallengeService studentChallengeService,
        IChallengeTransactionService challengeTransactionService,
        IOrderTransactionService orderTransactionService,
        IBonusTransactionService bonusTransactionService,
        IActivityTransactionService activityTransactionService,
        IOrderService orderService,
        IVoucherItemService voucherItemService,
        IEmailService emailService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Student, StudentModel>()
            .ForMember(s => s.MajorName, opt => opt.MapFrom(src => src.Major.MajorName))
            .ForMember(s => s.UniversityId, opt => opt.MapFrom(src => src.Campus.UniversityId))
            .ForMember(s => s.UniversityName, opt => opt.MapFrom(src => src.Campus.University.UniversityName))
            .ForMember(s => s.CampusName, opt => opt.MapFrom(src => src.Campus.CampusName))
            .ForMember(s => s.InviteCode, opt => opt.MapFrom(src => src.Id))
            .ForMember(s => s.UserName, opt => opt.MapFrom(src => src.Account.UserName))
            .ForMember(s => s.Email, opt => opt.MapFrom(src => src.Account.Email))
            .ForMember(s => s.Phone, opt => opt.MapFrom(src => src.Account.Phone))
            .ForMember(s => s.Avatar, opt => opt.MapFrom(src => src.Account.Avatar))
            .ForMember(s => s.ImageName, opt => opt.MapFrom(src => src.Account.FileName))
            .ForMember(s => s.DateVerified, opt => opt.MapFrom(src => src.Account.DateVerified))
            .ForMember(s => s.IsVerify, opt => opt.MapFrom(src => src.Account.IsVerify))
            .ForMember(s => s.StateId, opt => opt.MapFrom(src => (int)src.State))
            .ForMember(s => s.StateName, opt => opt.MapFrom(src => src.State.GetDisplayName()))
            .ForMember(s => s.GreenWalletId, opt => opt.MapFrom(src => (int)src.Wallets.Where(
                w => w.Type.Equals(WalletType.Green)).FirstOrDefault().Type))
            .ForMember(s => s.GreenWallet, opt => opt.MapFrom(src => src.Wallets.Where(
                w => w.Type.Equals(WalletType.Green)).FirstOrDefault().Type))
            .ForMember(s => s.GreenWalletName, opt => opt.MapFrom(src => src.Wallets.Where(
                w => w.Type.Equals(WalletType.Green)).FirstOrDefault().Type.GetDisplayName()))
            .ForMember(s => s.GreenWalletBalance, opt => opt.MapFrom(src => src.Wallets.Where(
                w => w.Type.Equals(WalletType.Green)).FirstOrDefault().Balance))
            .ForMember(s => s.RedWalletId, opt => opt.MapFrom(src => (int)src.Wallets.Where(
                w => w.Type.Equals(WalletType.Red)).FirstOrDefault().Type))
            .ForMember(s => s.RedWallet, opt => opt.MapFrom(src => src.Wallets.Where(
                w => w.Type.Equals(WalletType.Red)).FirstOrDefault().Type))
            .ForMember(s => s.RedWalletName, opt => opt.MapFrom(src => src.Wallets.Where(
                w => w.Type.Equals(WalletType.Red)).FirstOrDefault().Type.GetDisplayName()))
            .ForMember(s => s.RedWalletBalance, opt => opt.MapFrom(src => src.Wallets.Where(
                w => w.Type.Equals(WalletType.Red)).FirstOrDefault().Balance))
            .ReverseMap();
            // Map Student Extra Model
            cfg.CreateMap<Student, StudentExtraModel>()
            .ForMember(s => s.MajorName, opt => opt.MapFrom(src => src.Major.MajorName))
            .ForMember(s => s.MajorImage, opt => opt.MapFrom(src => src.Major.Image))
            .ForMember(s => s.UniversityId, opt => opt.MapFrom(src => src.Campus.UniversityId))
            .ForMember(s => s.UniversityName, opt => opt.MapFrom(src => src.Campus.University.UniversityName))
            .ForMember(s => s.UniversityImage, opt => opt.MapFrom(src => src.Campus.University.Image))
            .ForMember(s => s.CampusName, opt => opt.MapFrom(src => src.Campus.CampusName))
            .ForMember(s => s.CampusImage, opt => opt.MapFrom(src => src.Campus.Image))
            .ForMember(s => s.InviteCode, opt => opt.MapFrom(src => src.Id))
            .ForMember(s => s.UserName, opt => opt.MapFrom(src => src.Account.UserName))
            .ForMember(s => s.Email, opt => opt.MapFrom(src => src.Account.Email))
            .ForMember(s => s.Phone, opt => opt.MapFrom(src => src.Account.Phone))
            .ForMember(s => s.Avatar, opt => opt.MapFrom(src => src.Account.Avatar))
            .ForMember(s => s.ImageName, opt => opt.MapFrom(src => src.Account.FileName))
            .ForMember(s => s.DateVerified, opt => opt.MapFrom(src => src.Account.DateVerified))
            .ForMember(s => s.IsVerify, opt => opt.MapFrom(src => src.Account.IsVerify))
            .ForMember(s => s.StateId, opt => opt.MapFrom(src => (int)src.State))
            .ForMember(s => s.StateName, opt => opt.MapFrom(src => src.State.GetDisplayName()))
            .ForMember(s => s.GreenWalletId, opt => opt.MapFrom(src => (int)src.Wallets.Where(
                w => w.Type.Equals(WalletType.Green)).FirstOrDefault().Type))
            .ForMember(s => s.GreenWallet, opt => opt.MapFrom(src => src.Wallets.Where(
                w => w.Type.Equals(WalletType.Green)).FirstOrDefault().Type))
            .ForMember(s => s.GreenWalletName, opt => opt.MapFrom(src => src.Wallets.Where(
                w => w.Type.Equals(WalletType.Green)).FirstOrDefault().Type.GetDisplayName()))
            .ForMember(s => s.GreenWalletBalance, opt => opt.MapFrom(src => src.Wallets.Where(
                w => w.Type.Equals(WalletType.Green)).FirstOrDefault().Balance))
            .ForMember(s => s.RedWalletId, opt => opt.MapFrom(src => (int)src.Wallets.Where(
                w => w.Type.Equals(WalletType.Red)).FirstOrDefault().Type))
            .ForMember(s => s.RedWallet, opt => opt.MapFrom(src => src.Wallets.Where(
                w => w.Type.Equals(WalletType.Red)).FirstOrDefault().Type))
            .ForMember(s => s.RedWalletName, opt => opt.MapFrom(src => src.Wallets.Where(
                w => w.Type.Equals(WalletType.Red)).FirstOrDefault().Type.GetDisplayName()))
            .ForMember(s => s.RedWalletBalance, opt => opt.MapFrom(src => src.Wallets.Where(
                w => w.Type.Equals(WalletType.Red)).FirstOrDefault().Balance))
            .ForMember(s => s.Following, opt => opt.MapFrom(src => src.Wishlists.Count))
            .ForMember(s => s.Inviter, opt => opt.MapFrom(src => src.Invitees.FirstOrDefault().Inviter.FullName))
            .ForMember(s => s.Invitee, opt => opt.MapFrom(src => src.Inviters.Count))
            .ReverseMap();
            cfg.CreateMap<StudentChallenge, StudentChallengeModel>()
            .ForMember(c => c.StudentName, opt => opt.MapFrom(src => src.Student.FullName))
            .ForMember(c => c.ChallengeTypeId, opt => opt.MapFrom(src => (int)src.Challenge.Type))
            .ForMember(c => c.ChallengeType, opt => opt.MapFrom(src => src.Challenge.Type))
            .ForMember(c => c.ChallengeTypeName, opt => opt.MapFrom(src => src.Challenge.Type.GetDisplayName()))
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
            .ForMember(s => s.State, opt => opt.MapFrom(src => StudentState.Pending))
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
            .ForMember(s => s.Role, opt => opt.MapFrom(src => Role.Student))
            .ForMember(s => s.Password, opt => opt.MapFrom(src => BCryptNet.HashPassword(src.Password)))
            .ForMember(s => s.IsVerify, opt => opt.MapFrom(src => !src.State.Equals(1)))
            .ForMember(s => s.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(s => s.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(s => s.Status, opt => opt.MapFrom(src => true))
            .AfterMap((src, dest) =>
            {
                dest.DateVerified = !src.State.Equals(1) ? DateTime.Now : null;
            });
            // Map Update Student Model
            cfg.CreateMap<Student, UpdateStudentModel>()
            .ReverseMap()
            .ForMember(t => t.Major, opt => opt.MapFrom(src => (string)null))
            .ForMember(t => t.Campus, opt => opt.MapFrom(src => (string)null))
            .ForMember(s => s.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForPath(s => s.Account.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForPath(s => s.Account.State, opt => opt.MapFrom(src => true));
            // Map Update Student Verificaion Model
            cfg.CreateMap<Student, UpdateStudentVerifyModel>()
            .ReverseMap()
            .ForMember(s => s.FileNameFront, opt => opt.Ignore())
            .ForMember(s => s.FileNameBack, opt => opt.Ignore())
            .ForMember(s => s.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForPath(s => s.Account.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForPath(s => s.Account.State, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.studentRepository = studentRepository;
        this.fireBaseService = fireBaseService;
        this.accountRepository = accountRepository;
        this.invitationService = invitationService;
        this.studentChallengeService = studentChallengeService;
        this.challengeTransactionService = challengeTransactionService;
        this.orderTransactionService = orderTransactionService;
        this.bonusTransactionService = bonusTransactionService;
        this.activityTransactionService = activityTransactionService;
        this.orderService = orderService;
        this.voucherItemService = voucherItemService;
        this.emailService = emailService;
    }

    public async Task<StudentModel> Add(CreateStudentModel creation)
    {
        Account account = mapper.Map<Account>(creation);

        //Upload avatar
        if (creation.Avatar != null && creation.Avatar.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Avatar, ACCOUNT_FOLDER_NAME);
            account.Avatar = f.URL;
            account.FileName = f.FileName;
        }

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

        accountRepository.Add(account);
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
                    .Where(s => (bool)s.Status
                    && s.IsCompleted.Equals(false)
                    && s.Challenge.Type.Equals(ChallengeType.Welcome)), 1);

                studentChallengeService.Update(studentRepository
                    .GetById(creation.InviteCode).StudentChallenges
                    .Where(s => (bool)s.Status
                    && s.IsCompleted.Equals(false)
                    && s.Challenge.Type.Equals(ChallengeType.Spread)), 1);
            }

            if (new[] { StudentState.Active, StudentState.Inactive }.Contains
                (student.State.Value))
            {
                studentChallengeService.Update(studentRepository
                .GetById(student.Id).StudentChallenges
                .Where(s => (bool)s.Status
                && s.IsCompleted.Equals(false)
                && s.Challenge.Type.Equals(ChallengeType.Verify)), 1);
            }

            // Send mail
            switch (student.State)
            {
                case StudentState.Pending:
                    emailService.SendEmailStudentRegister(account.Email);
                    break;
                case StudentState.Active:
                    emailService.SendEmailStudentRegisterApprove(account.Email);
                    break;
                case StudentState.Inactive:
                    emailService.SendEmailStudentRegisterApprove(account.Email);
                    break;
                case StudentState.Rejected:
                    emailService.SendEmailStudentRegisterReject(account.Email);
                    break;
            }
        }

        return mapper.Map<StudentModel>(student);
    }

    public async Task<StudentModel> AddGoogle(CreateStudentGoogleModel creation)
    {
        Student entity = mapper.Map<Student>(creation);

        Account account = accountRepository.GetById(creation.AccountId);

        if (account.Email.IsNullOrEmpty()
            || !account.Email.Equals(creation.Email)
            || !account.Role.Equals(Role.Student))
        {
            throw new InvalidParameterException("Đăng nhập bằng tài khoản Google của bạn không hợp lệ");
        }

        account.Phone = creation.Phone;
        account.Email = creation.Email;
        account.State = true;
        accountRepository.Update(account);
        emailService.SendEmailStudentRegister(account.Email);

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
                    .Where(s => (bool)s.Status
                    && s.IsCompleted.Equals(false)
                    && s.Challenge.Type.Equals(ChallengeType.Welcome)), 1);

                studentChallengeService.Update(studentRepository
                    .GetById(creation.InviteCode).StudentChallenges
                    .Where(s => (bool)s.Status
                    && s.IsCompleted.Equals(false)
                    && s.Challenge.Type.Equals(ChallengeType.Spread)), 1);
            }
        }

        return mapper.Map<StudentModel>(entity);
    }

    public void Delete(string id)
    {
        Student entity = studentRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Orders.All(o => new[] { State.Abort, State.Receipt }.
            Contains(o.OrderStates.LastOrDefault().State.Value)))
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
                throw new InvalidParameterException("Xóa thất bại do tồn tại đơn hàng cần hoàn thành");
            }
        }
        else
        {
            throw new InvalidParameterException("Không tìm thấy sinh viên");
        }
    }

    public PagedResultModel<StudentModel> GetAll
        (List<string> majorIds, List<string> campusIds, List<StudentState> stateIds,
        bool? isVerify, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<StudentModel>>
            (studentRepository.GetAll(majorIds, campusIds, stateIds,
            isVerify, propertySort, isAsc, search, page, limit));
    }

    public StudentExtraModel GetById(string id)
    {
        Student entity = studentRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<StudentExtraModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy sinh viên");
    }

    public PagedResultModel<StudentChallengeModel> GetChallengeListByStudentId
        (List<ChallengeType> typeIds, string id, bool? isCompleted, bool? state,
        bool? isClaimed, string propertySort, bool isAsc, string search, int page, int limit)
    {
        Student entity = studentRepository.GetById(id);
        if (entity != null)
        {
            PagedResultModel<StudentChallengeModel> result = studentChallengeService.GetAll
                (new() { id }, new(), typeIds, state, propertySort, isAsc, search, page, limit);

            result.Result = result.Result
                .Where(c => (isCompleted == null || c.IsCompleted.Equals(isCompleted))
                && (isClaimed == null || c.IsClaimed.Equals(isClaimed))).ToList();

            return result;
        }
        throw new InvalidParameterException("Không tìm thấy sinh viên");
    }

    public PagedResultModel<TransactionModel> GetHistoryTransactionListByStudentId
        (string id, List<TransactionType> typeIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        Student entity = studentRepository.GetById(id);
        if (entity != null)
        {
            var query = (typeIds.Contains(TransactionType.ChallengeTransaction) || typeIds.Count == 0 ?
                challengeTransactionService.GetAll
                (studentRepository.GetById(id).Wallets.Select(w => w.Id).ToList(), new(), search) : new())

                .Concat(typeIds.Contains(TransactionType.OrderTransaction) || typeIds.Count == 0 ?
                orderTransactionService.GetAll
                (studentRepository.GetById(id).Wallets.Select(w => w.Id).ToList(), new(), search) : new())

                .Concat(typeIds.Contains(TransactionType.BonusTransaction) || typeIds.Count == 0 ?
                bonusTransactionService.GetAll
                (studentRepository.GetById(id).Wallets.Select(w => w.Id).ToList(), new(), new(), search) : new())

                .Concat(typeIds.Contains(TransactionType.ActivityTransaction) || typeIds.Count == 0 ?
                activityTransactionService.GetAll
                (studentRepository.GetById(id).Wallets.Select(w => w.Id).ToList(), new(), search) : new())

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
        throw new InvalidParameterException("Không tìm thấy sinh viên");
    }

    public OrderExtraModel GetOrderByOrderId(string id, string orderId)
    {
        Student entity = studentRepository.GetById(id);
        if (entity != null)
        {
            OrderExtraModel order = orderService.GetById(orderId);
            if (order != null && order.StudentId.Equals(id))
            {
                return order;
            }
            throw new InvalidParameterException("Không tìm thấy đơn hàng");
        }
        throw new InvalidParameterException("Không tìm thấy sinh viên");
    }

    public PagedResultModel<OrderModel> GetOrderListByStudentId
        (List<string> stationIds, List<State> stateIds, string id, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        Student entity = studentRepository.GetById(id);
        if (entity != null)
        {
            return orderService.GetAll
                (stationIds, new() { id }, stateIds, state, propertySort, isAsc, search, page, limit);
        }
        throw new InvalidParameterException("Không tìm thấy sinh viên");
    }

    public VoucherItemExtraModel GetVoucherItemByVoucherId(string id, string voucherId)
    {
        Student entity = studentRepository.GetById(id);
        if (entity != null)
        {
            VoucherItemExtraModel voucher = voucherItemService.GetById(voucherId);
            if (voucher != null && !voucher.StudentId.IsNullOrEmpty() && voucher.StudentId.Equals(id))
            {
                return voucher;
            }
            throw new InvalidParameterException("Không tìm thấy khuyến mãi");
        }
        throw new InvalidParameterException("Không tìm thấy sinh viên");
    }

    public PagedResultModel<VoucherItemModel> GetVoucherListByStudentId
        (List<string> campaignIds, List<string> voucherIds, List<string> brandIds, List<string> typeIds,
        string id, bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        Student entity = studentRepository.GetById(id);
        if (entity != null)
        {
            return voucherItemService.GetAll
                (campaignIds, voucherIds, brandIds, typeIds, new() { id },
                state, propertySort, isAsc, search, page, limit);
        }
        throw new InvalidParameterException("Không tìm thấy sinh viên");
    }

    public List<string> GetWishlistsByStudentId(string id)
    {
        return studentRepository.GetById(id).Wishlists
            .Select(w => w.BrandId).Distinct().ToList();
    }

    public async Task<StudentExtraModel> Update(string id, UpdateStudentModel update)
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

            return mapper.Map<StudentExtraModel>(studentRepository.Update(entity));
        }
        throw new InvalidParameterException("Không tìm thấy sinh viên");
    }

    public bool UpdateInviteCode(string id, string code)
    {
        Student entity = studentRepository.GetById(id);
        if (entity != null)
        {
            if (!invitationService.ExistInvitation(entity.Id))
            {
                // Set invitation
                if (!code.IsNullOrEmpty())
                {
                    // Take the challenge
                    studentChallengeService.Update(studentRepository
                        .GetById(entity.Id).StudentChallenges
                        .Where(s => (bool)s.Status
                        && s.IsCompleted.Equals(false)
                        && s.Challenge.Type.Equals(ChallengeType.Welcome)), 1);

                    studentChallengeService.Update(studentRepository
                        .GetById(code).StudentChallenges
                        .Where(s => (bool)s.Status
                        && s.IsCompleted.Equals(false)
                        && s.Challenge.Type.Equals(ChallengeType.Spread)), 1);

                    return invitationService.Add(new CreateInvitationModel
                    {
                        InviterId = code,
                        InviteeId = entity.Id,
                        Description = null,
                        State = true
                    }) != null;
                }
                throw new InvalidParameterException("Mã mời không hợp lệ");
            }
            throw new InvalidParameterException("Sinh viên đã nhập mã mời");
        }
        throw new InvalidParameterException("Không tìm thấy sinh viên");
    }

    public bool UpdateState(string id, StudentState stateId)
    {
        if (!new[] { StudentState.Pending }.Contains(stateId))
        {
            Student entity = studentRepository.GetById(id);
            if (entity != null)
            {
                switch (entity.State)
                {
                    case StudentState.Pending
                        when new[] { StudentState.Inactive }.Contains(stateId):
                        throw new InvalidParameterException("Trạng thái sinh viên không hợp lệ");
                    case StudentState.Active
                        when new[] { StudentState.Rejected, StudentState.Active }.Contains(stateId):
                        throw new InvalidParameterException("Trạng thái sinh viên không hợp lệ");
                    case StudentState.Inactive
                        when new[] { StudentState.Rejected, StudentState.Inactive }.Contains(stateId):
                        throw new InvalidParameterException("Trạng thái sinh viên không hợp lệ");
                    case StudentState.Pending
                        when new[] { StudentState.Rejected, StudentState.Active }.Contains(stateId):

                        if (stateId.Equals(StudentState.Active))
                        {
                            // Take the challenge
                            studentChallengeService.Update(
                                entity.StudentChallenges
                                .Where(s => (bool)s.Status
                                && s.IsCompleted.Equals(false)
                                && s.Challenge.Type.Equals(ChallengeType.Verify)), 1);

                            emailService.SendEmailStudentRegisterApprove(entity.Account.Email);
                        }
                        else if (stateId.Equals(StudentState.Rejected))
                        {
                            emailService.SendEmailStudentRegisterReject(entity.Account.Email);
                        }

                        entity.Account.IsVerify = true;
                        break;
                }

                entity.State = stateId;
                return studentRepository.Update(entity) != null;
            }
            throw new InvalidParameterException("Không tìm thấy sinh viên");
        }
        throw new InvalidParameterException("Trạng thái sinh viên không hợp lệ");
    }

    public async Task<StudentExtraModel> UpdateVerification(string id, UpdateStudentVerifyModel update)
    {
        Student entity = studentRepository.GetById(id);
        if (entity != null)
        {
            if (!entity.Code.Equals(update.Code))
            {
                if (!studentRepository.CheckCodeDuplicate(update.Code))
                {
                    throw new InvalidParameterException("Mã sinh viên đã được sử dụng");
                }
            }
            entity = mapper.Map(update, entity);

            // Upload the student card front image
            if (update.StudentCardFront != null && update.StudentCardFront.Length > 0)
            {
                // Remove image
                await fireBaseService.RemoveFileAsync(entity.FileNameFront, FOLDER_NAME);

                //Upload new image update
                FireBaseFile f = await fireBaseService.UploadFileAsync(update.StudentCardFront, FOLDER_NAME);
                entity.StudentCardFront = f.URL;
                entity.FileNameFront = f.FileName;
            }

            // Upload the student card back image
            if (update.StudentCardBack != null && update.StudentCardBack.Length > 0)
            {
                // Remove image
                await fireBaseService.RemoveFileAsync(entity.FileNameBack, FOLDER_NAME);

                //Upload new image update
                FireBaseFile f = await fireBaseService.UploadFileAsync(update.StudentCardBack, FOLDER_NAME);
                entity.StudentCardBack = f.URL;
                entity.FileNameBack = f.FileName;
            }

            entity.State = StudentState.Pending;
            return mapper.Map<StudentExtraModel>(studentRepository.Update(entity));
        }
        throw new InvalidParameterException("Không tìm thấy sinh viên");
    }
}
