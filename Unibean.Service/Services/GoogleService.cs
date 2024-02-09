using Unibean.Repository.Entities;
using Unibean.Service.Models.Accounts;
using Unibean.Service.Models.Authens;
using Unibean.Service.Models.Brands;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Services.Interfaces;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Unibean.Service.Services;

public class GoogleService : IGoogleService
{
    private const string CLIENT_ID = "783137803189-gqg1hfpemgl7cq9qi8jplrlbaemcanld.apps.googleusercontent.com";

    private readonly IAccountService accountService;

    private readonly IBrandService brandService;

    private readonly IEmailService emailService;

    public GoogleService(
        IAccountService accountService,
        IBrandService brandService,
        IEmailService emailService)
    {
        this.accountService = accountService;
        this.brandService = brandService;
        this.emailService = emailService;
    }

    public async Task<AccountModel> LoginWithGoogle(GoogleTokenModel token, string role)
    {
        try
        {
            Payload payload = await ValidateAsync(token.IdToken);

            if (payload != null)
            {
                if (payload.AudienceAsList.Contains(CLIENT_ID) && payload.EmailVerified)
                {
                    var account = accountService.GetByEmail(payload.Email);
                    if(account != null)
                    {
                        return account;
                    }

                    switch (role)
                    {
                        case "Brand":
                            account = accountService.AddGoogle(new CreateGoogleAccountModel
                            {
                                Email = payload.Email,
                                IsVerify = true,
                                Role = (int)Role.Brand,
                                Description = "Create by logging in with Google",
                                State = true,
                            });
                            emailService.SendEmailBrandRegister(account.Email);

                            var brand = brandService.AddGoogle(new CreateBrandGoogleModel
                            {
                                AccountId = account.Id,
                                BrandName = payload.Email,
                                Email = payload.Email,
                                Description = null,
                                State = true,
                            });
                            account.UserId = brand.Id;
                            account.Name = brand.BrandName;
                            return account;
                        case "Student":
                            account = accountService.AddGoogle(new CreateGoogleAccountModel
                            {
                                Email = payload.Email,
                                IsVerify = false,
                                Role = (int)Role.Student,
                                Description = "Create by logging in with Google",
                                State = false,
                            });
                            return account;
                    }
                }
            }
            throw new InvalidParameterException("Mã thông báo không hợp lệ");
        }
        catch (Exception e)
        {
            throw new InvalidParameterException(e.Message);
        }
    }
}
