using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Authens;
using Unibean.Service.Models.ChallengeTransactions;
using Unibean.Service.Models.Invitations;
using Unibean.Service.Models.Orders;
using Unibean.Service.Models.StudentChallenges;
using Unibean.Service.Models.Students;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Models.VoucherItems;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class StudentServiceTest
{
    private readonly IStudentRepository studentRepository;

    private readonly IFireBaseService fireBaseService;

    private readonly IAccountRepository accountRepository;

    private readonly IInvitationService invitationService;

    private readonly IStudentChallengeService studentChallengeService;

    private readonly IChallengeTransactionService challengeTransactionService;

    private readonly IOrderService orderService;

    private readonly IVoucherItemService voucherItemService;

    private readonly IEmailService emailService;

    private readonly ITransactionService transactionService;

    public StudentServiceTest()
    {
        studentRepository = A.Fake<IStudentRepository>();
        fireBaseService = A.Fake<IFireBaseService>();
        accountRepository = A.Fake<IAccountRepository>();
        invitationService = A.Fake<IInvitationService>();
        studentChallengeService = A.Fake<IStudentChallengeService>();
        challengeTransactionService = A.Fake<IChallengeTransactionService>();
        orderService = A.Fake<IOrderService>();
        voucherItemService = A.Fake<IVoucherItemService>();
        emailService = A.Fake<IEmailService>();
        transactionService = A.Fake<ITransactionService>();
    }

    [Fact]
    public void StudentService_Add()
    {
        // Arrange
        string id = "id";
        CreateStudentModel creation = A.Fake<CreateStudentModel>();
        A.CallTo(() => accountRepository.Add(A<Account>.Ignored)).Returns(new());
        A.CallTo(() => studentRepository.Add(A<Student>.Ignored)).Returns(new()
        {
            Id = id,
            State = StudentState.Active
        });
        var service = new StudentService(studentRepository, fireBaseService, accountRepository,
            invitationService, studentChallengeService, challengeTransactionService, orderService,
            voucherItemService, emailService, transactionService);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<StudentExtraModel>));
        Assert.Equal(id, result.Result.Id);
    }

    [Fact]
    public void StudentService_AddGoogle()
    {
        // Arrange
        string id = "id";
        string email = "email";
        CreateStudentGoogleModel creation = new()
        {
            AccountId = id,
            Email = email,
        };
        A.CallTo(() => accountRepository.GetById(id))
        .Returns(new()
        {
            Id = id,
            Email = email,
            Role = Role.Student
        });
        A.CallTo(() => studentRepository.Add(A<Student>.Ignored))
        .Returns(new()
        {
            Id = id,
            State = StudentState.Active
        });
        var service = new StudentService(studentRepository, fireBaseService, accountRepository,
            invitationService, studentChallengeService, challengeTransactionService, orderService,
            voucherItemService, emailService, transactionService);

        // Act
        var result = service.AddGoogle(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<StudentModel>));
        Assert.Equal(id, result.Result.Id);
    }

    [Fact]
    public void StudentService_ClaimChallenge()
    {
        // Arrange
        string id = "id";
        string challengeId = "challengeId";
        A.CallTo(() => studentRepository.GetById(id))
        .Returns(new()
        {
            Id = id,
            Wallets = new List<Wallet>
            {
                new()
                {
                    Id = id,
                    Type = WalletType.Green
                }
            }
        });
        A.CallTo(() => studentChallengeService.GetById(challengeId))
        .Returns(new()
        {
            Id = id,
            StudentId = id,
            IsClaimed = false,
            IsCompleted = true,
            Current = 0,
            Condition = 0,
            Amount = 0,
        });
        A.CallTo(() => challengeTransactionService.Add(A<CreateChallengeTransactionModel>.Ignored))
        .Returns(new()
        {
            Id = id,
        });
        var service = new StudentService(studentRepository, fireBaseService, accountRepository,
            invitationService, studentChallengeService, challengeTransactionService, orderService,
            voucherItemService, emailService, transactionService);

        // Act
        var result = service.ClaimChallenge(id, challengeId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ChallengeTransactionModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void StudentService_Delete()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => studentRepository.GetById(id)).Returns(new()
        {
            Account = new()
            {
                Id = id,
            },
            Orders = new List<Order>()
            {
                new()
                {
                    OrderStates = new List<OrderState>()
                    {
                        new()
                        {
                            State = State.Abort,
                        }
                    }
                }
            }
        });
        A.CallTo(() => studentRepository.Delete(id));
        A.CallTo(() => accountRepository.Delete(id));
        var service = new StudentService(studentRepository, fireBaseService, accountRepository,
            invitationService, studentChallengeService, challengeTransactionService, orderService,
            voucherItemService, emailService, transactionService);

        // Act & Assert
        service.Delete(id);
    }

    [Fact]
    public void StudentService_GetAll()
    {
        // Arrange
        List<string> majorIds = new();
        List<string> campusIds = new();
        List<string> universityIds = new();
        List<StudentState> stateIds = new();
        bool? isVerify = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        JwtRequestModel request = new()
        {
            Role = "Admin"
        };
        PagedResultModel<Student> pagedResultModel = new()
        {
            Result = new()
            {
                new()
                {
                    State = StudentState.Active
                },
                new()
                {
                    State = StudentState.Active
                },
                new()
                {
                    State = StudentState.Active
                }
            }
        };
        A.CallTo(() => studentRepository.GetAll(majorIds, campusIds, universityIds, stateIds, 
            isVerify, propertySort, isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new StudentService(studentRepository, fireBaseService, accountRepository,
            invitationService, studentChallengeService, challengeTransactionService, orderService,
            voucherItemService, emailService, transactionService);

        // Act
        var result = service.GetAll(majorIds, campusIds, universityIds, stateIds, 
            isVerify, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<StudentModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void StudentService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => studentRepository.GetById(id))
        .Returns(new()
        {
            Id = id,
            State = StudentState.Active
        });
        var service = new StudentService(studentRepository, fireBaseService, accountRepository,
            invitationService, studentChallengeService, challengeTransactionService, orderService,
            voucherItemService, emailService, transactionService);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StudentExtraModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void StudentService_GetChallengeListByStudentId()
    {
        // Arrange
        string id = "id";
        List<ChallengeType> typeIds = new();
        bool? isCompleted = null;
        bool? state = null;
        bool? isClaimed = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        List<StudentChallengeModel> pagedResultModel = new()
        {
            new(),
            new(),
            new()
        };
        A.CallTo(() => studentRepository.CheckStudentId(id)).Returns(true);
        A.CallTo(() => studentChallengeService.GetAll(new() { id }, new(),
            typeIds, state, propertySort, isAsc, search)).Returns(pagedResultModel);
        var service = new StudentService(studentRepository, fireBaseService, accountRepository,
            invitationService, studentChallengeService, challengeTransactionService, orderService,
            voucherItemService, emailService, transactionService);

        // Act
        var result = service.GetChallengeListByStudentId(typeIds, id, isCompleted,
            state, isClaimed, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<StudentChallengeModel>));
        Assert.Equal(pagedResultModel.Count, result.Result.Count);
    }

    [Fact]
    public void StudentService_GetHistoryTransactionListByStudentId()
    {
        // Arrange
        string id = "id";
        List<TransactionType> typeIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        List<string> list = new()
        {
            ""
        };
        PagedResultModel<TransactionModel> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => studentRepository.GetWalletListById(id)).Returns(list);
        A.CallTo(() => transactionService.GetAll(list, typeIds, state, propertySort,
                isAsc, search, page, limit, Role.Student)).Returns(pagedResultModel);
        var service = new StudentService(studentRepository, fireBaseService, accountRepository,
            invitationService, studentChallengeService, challengeTransactionService, orderService,
            voucherItemService, emailService, transactionService);

        // Act
        var result = service.GetHistoryTransactionListByStudentId(id, typeIds,
            state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<TransactionModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void StudentService_GetOrderByOrderId()
    {
        // Arrange
        string id = "id";
        string orderId = "orderId";
        A.CallTo(() => studentRepository.CheckStudentId(id)).Returns(true);
        A.CallTo(() => orderService.GetById(orderId))
        .Returns(new()
        {
            StudentId = id,
        });
        var service = new StudentService(studentRepository, fireBaseService, accountRepository,
            invitationService, studentChallengeService, challengeTransactionService, orderService,
            voucherItemService, emailService, transactionService);

        // Act
        var result = service.GetOrderByOrderId(id, orderId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(OrderExtraModel));
        Assert.Equal(id, result.StudentId);
    }

    [Fact]
    public void StudentService_GetOrderListByStudentId()
    {
        // Arrange
        string id = "id";
        List<string> stationIds = new();
        List<State> stateIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<OrderModel> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => studentRepository.CheckStudentId(id)).Returns(true);
        A.CallTo(() => orderService.GetAll(stationIds, new() { id }, stateIds,
            state, propertySort, isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new StudentService(studentRepository, fireBaseService, accountRepository,
            invitationService, studentChallengeService, challengeTransactionService, orderService,
            voucherItemService, emailService, transactionService);

        // Act
        var result = service.GetOrderListByStudentId(stationIds, stateIds, id,
            state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<OrderModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void StudentService_GetVoucherItemByVoucherId()
    {
        // Arrange
        string id = "id";
        string voucherId = "voucherId";
        A.CallTo(() => studentRepository.CheckStudentId(id)).Returns(true);
        A.CallTo(() => voucherItemService.GetById(voucherId))
        .Returns(new()
        {
            StudentId = id,
        });
        var service = new StudentService(studentRepository, fireBaseService, accountRepository,
            invitationService, studentChallengeService, challengeTransactionService, orderService,
            voucherItemService, emailService, transactionService);

        // Act
        var result = service.GetVoucherItemByVoucherId(id, voucherId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(VoucherItemExtraModel));
        Assert.Equal(id, result.StudentId);
    }

    [Fact]
    public void StudentService_GetVoucherListByStudentId()
    {
        // Arrange
        string id = "id";
        List<string> campaignIds = new();
        List<string> campaignDetailIds = new();
        List<string> voucherIds = new();
        List<string> brandIds = new();
        List<string> typeIds = new();
        bool? state = null;
        bool? isUsed = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<VoucherItemModel> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => studentRepository.CheckStudentId(id)).Returns(true);
        A.CallTo(() => voucherItemService.GetAll(campaignIds, campaignDetailIds,
            voucherIds, brandIds, typeIds, new() { id }, true, true, isUsed, state,
            propertySort, isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new StudentService(studentRepository, fireBaseService, accountRepository,
            invitationService, studentChallengeService, challengeTransactionService, orderService,
            voucherItemService, emailService, transactionService);

        // Act
        var result = service.GetVoucherListByStudentId(campaignIds, campaignDetailIds,
            voucherIds, brandIds, typeIds, id, state, isUsed, propertySort, isAsc,
            search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<VoucherItemModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void StudentService_GetWishlistsByStudentId()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => studentRepository.GetById(id)).Returns(new()
        {
            Wishlists = new List<Wishlist>
            {
                new()
                {
                    BrandId = id
                }
            }
        });
        var service = new StudentService(studentRepository, fireBaseService, accountRepository,
            invitationService, studentChallengeService, challengeTransactionService, orderService,
            voucherItemService, emailService, transactionService);

        // Act
        var result = service.GetWishlistsByStudentId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(List<string>));
        Assert.Equal(id, result.FirstOrDefault());
    }

    [Fact]
    public void StudentService_Update()
    {
        // Arrange
        string id = "id";
        string fullName = "fullName";
        UpdateStudentModel update = A.Fake<UpdateStudentModel>();
        A.CallTo(() => studentRepository.GetById(id));
        A.CallTo(() => studentRepository.Update(A<Student>.Ignored))
            .Returns(new()
            {
                Id = id,
                FullName = fullName,
                State = StudentState.Active
            });
        var service = new StudentService(studentRepository, fireBaseService, accountRepository,
            invitationService, studentChallengeService, challengeTransactionService, orderService,
            voucherItemService, emailService, transactionService);

        // Act
        var result = service.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<StudentExtraModel>));
        Assert.Equal(id, result.Result.Id);
        Assert.Equal(fullName, result.Result.FullName);
    }

    [Fact]
    public void StudentService_UpdateInviteCode()
    {
        // Arrange
        string id = "id";
        string code = "code";
        A.CallTo(() => studentRepository.GetById(id));
        A.CallTo(() => invitationService.ExistInvitation(id)).Returns(false);
        A.CallTo(() => invitationService.Add(A<CreateInvitationModel>.Ignored)).Returns(new());
        var service = new StudentService(studentRepository, fireBaseService, accountRepository,
            invitationService, studentChallengeService, challengeTransactionService, orderService,
            voucherItemService, emailService, transactionService);

        // Act & Assert
        Assert.True(service.UpdateInviteCode(id, code));
    }

    [Fact]
    public void StudentService_UpdateState()
    {
        // Arrange
        string id = "id";
        string note = "note";
        StudentState stateId = StudentState.Active;
        A.CallTo(() => studentRepository.GetById(id)).Returns(new()
        {
            State = StudentState.Pending,
            StudentChallenges = new List<StudentChallenge>
            {
                new()
            },
            Account = new()
            {
                Email = "receiver"
            }
        });
        var service = new StudentService(studentRepository, fireBaseService, accountRepository,
            invitationService, studentChallengeService, challengeTransactionService, orderService,
            voucherItemService, emailService, transactionService);

        // Act & Assert
        Assert.True(service.UpdateState(id, stateId, note));
    }

    [Fact]
    public void StudentService_UpdateVerification()
    {
        // Arrange
        string id = "id";
        string code = "code";
        UpdateStudentVerifyModel update = new()
        {
            Code = code,
        };
        A.CallTo(() => studentRepository.GetById(id)).Returns(new()
        {
            State = StudentState.Pending,
            Code = code,
            StudentChallenges = new List<StudentChallenge>
            {
                new()
            },
            Account = new()
            {
                Email = "receiver"
            }
        });
        A.CallTo(() => studentRepository.Update(A<Student>.Ignored))
            .Returns(new()
            {
                Id = id,
                State = StudentState.Active
            });
        var service = new StudentService(studentRepository, fireBaseService, accountRepository,
            invitationService, studentChallengeService, challengeTransactionService, orderService,
            voucherItemService, emailService, transactionService);

        // Act
        var result = service.UpdateVerification(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<StudentExtraModel>));
        Assert.Equal(id, result.Result.Id);
    }
}
