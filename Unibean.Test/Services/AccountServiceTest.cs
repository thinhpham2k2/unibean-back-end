using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Accounts;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class AccountServiceTest
{
    private readonly IAccountRepository accountRepository;

    private readonly IFireBaseService fireBaseService;

    private readonly IBrandRepository brandRepository;

    private readonly IStudentRepository studentRepository;

    private readonly IInvitationService invitationService;

    private readonly IStudentChallengeService studentChallengeService;

    private readonly IEmailService emailService;

    public AccountServiceTest()
    {
        accountRepository = A.Fake<IAccountRepository>();
        fireBaseService = A.Fake<IFireBaseService>();
        brandRepository = A.Fake<IBrandRepository>();
        studentRepository = A.Fake<IStudentRepository>();
        invitationService = A.Fake<IInvitationService>();
        studentChallengeService = A.Fake<IStudentChallengeService>();
        emailService = A.Fake<IEmailService>();
    }

    [Fact]
    public void AccountService_AddBrand()
    {
        //Arrange
        Role role = Role.Brand;
        CreateBrandAccountModel creation = A.Fake<CreateBrandAccountModel>();
        A.CallTo(() => accountRepository.Add(A<Account>.Ignored))
            .Returns(new()
            {
                Role = role,
            });
        var service = new AccountService
            (accountRepository, fireBaseService, brandRepository, studentRepository,
            invitationService, studentChallengeService, emailService);

        //Act
        var result = service.AddBrand(creation);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<AccountModel>));
        Assert.Equal(role.ToString(), result.Result.Role);
    }

    [Fact]
    public void AccountService_AddGoogle()
    {
        //Arrange
        Role role = Role.Student;
        CreateGoogleAccountModel creation = new()
        {
            Role = (int)role
        };
        A.CallTo(() => accountRepository.Add(A<Account>.Ignored))
            .Returns(new()
            {
                Role = role,
            });
        var service = new AccountService
            (accountRepository, fireBaseService, brandRepository, studentRepository,
            invitationService, studentChallengeService, emailService);

        //Act
        var result = service.AddGoogle(creation);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(AccountModel));
        Assert.Equal(role.ToString(), result.Role);
    }

    [Fact]
    public void AccountService_AddStudent()
    {
        //Arrange
        Role role = Role.Student;
        CreateStudentAccountModel creation = A.Fake<CreateStudentAccountModel>();
        A.CallTo(() => accountRepository.Add(A<Account>.Ignored))
            .Returns(new()
            {
                Role = role,
            });
        var service = new AccountService
            (accountRepository, fireBaseService, brandRepository, studentRepository,
            invitationService, studentChallengeService, emailService);

        //Act
        var result = service.AddStudent(creation);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<AccountModel>));
        Assert.Equal(role.ToString(), result.Result.Role);
    }

    [Fact]
    public void AccountService_GetByEmail()
    {
        //Arrange
        string email = "";
        Role role = Role.Student;
        A.CallTo(() => accountRepository.GetByEmail(email))
            .Returns(new()
            {
                Role = role,
            });
        var service = new AccountService
            (accountRepository, fireBaseService, brandRepository, studentRepository,
            invitationService, studentChallengeService, emailService);

        //Act
        var result = service.GetByEmail(email);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(AccountModel));
        Assert.Equal(role.ToString(), result.Role);
    }

    [Fact]
    public void AccountService_GetByUserNameAndPassword()
    {
        //Arrange
        string userName = "";
        string password = "";
        Role role = Role.Student;
        A.CallTo(() => accountRepository.GetByUserNameAndPassword(userName, password))
            .Returns(new()
            {
                Role = role,
            });
        var service = new AccountService
            (accountRepository, fireBaseService, brandRepository, studentRepository,
            invitationService, studentChallengeService, emailService);

        //Act
        var result = service.GetByUserNameAndPassword(userName, password);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(AccountModel));
        Assert.Equal(role.ToString(), result.Role);
    }
}
