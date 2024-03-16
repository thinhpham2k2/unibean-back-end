using FakeItEasy;
using Unibean.Service.Models.Authens;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class GoogleServiceTest
{
    private readonly IAccountService accountService;

    private readonly IBrandService brandService;

    private readonly IEmailService emailService;

    public GoogleServiceTest()
    {
        accountService = A.Fake<IAccountService>();
        brandService = A.Fake<IBrandService>();
        emailService = A.Fake<IEmailService>();
    }

    [Fact]
    public void GoogleService_LoginWithGoogle()
    {
        // Arrange
        GoogleTokenModel token = new();
        string role = "Brand";
        var service = new GoogleService
            (accountService, brandService, emailService);

        // Act & Assert
        Assert.ThrowsAsync<InvalidParameterException>(
            () => service.LoginWithGoogle(token, role));
    }
}
