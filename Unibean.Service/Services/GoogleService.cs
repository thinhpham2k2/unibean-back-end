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

    private readonly IRoleService roleService;

    private readonly IBrandService brandService;

    public GoogleService(IAccountService accountService,
        IRoleService roleService, IBrandService brandService)
    {
        this.accountService = accountService;
        this.roleService = roleService;
        this.brandService = brandService;
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
                    var roleModel = roleService.GetRoleByName(role);
                    switch (role)
                    {
                        case "Brand":
                            account = accountService.AddGoogle(new CreateGoogleAccountModel
                            {
                                Email = payload.Email,
                                IsVerify = true,
                                RoleId = roleModel?.Id,
                                Description = "Create by logging in with Google",
                                State = true,
                            });
                            account.RoleName = roleModel?.RoleName;

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
                                RoleId = roleModel?.Id,
                                Description = "Create by logging in with Google",
                                State = false,
                            });
                            account.RoleName = roleModel?.RoleName;
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
