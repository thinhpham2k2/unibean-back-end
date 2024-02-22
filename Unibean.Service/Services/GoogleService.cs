using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Unibean.Repository.Entities;
using Unibean.Service.Models.Accounts;
using Unibean.Service.Models.Authens;
using Unibean.Service.Models.Brands;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class GoogleService : IGoogleService
{
    private const string CLIENT_ID = "783137803189-gqg1hfpemgl7cq9qi8jplrlbaemcanld.apps.googleusercontent.com";

    private const string CLIENT_ID_1 = "804634450758-77ftvtrh77qkstdu1dpqmnspdjvvjp45.apps.googleusercontent.com";

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

    public Task<AccountModel> LoginWithGoogle(GoogleTokenModel token, string role)
    {
        try
        {
            var validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new RsaSecurityKey(RSA.Create()),
                ValidAudiences = new[] { CLIENT_ID, CLIENT_ID_1 },
                ValidIssuers = new List<string> { "https://accounts.google.com", "accounts.google.com" },
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = false,
            };

            JwtPayload payload = new();
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token.IdToken);

            SecurityToken validatedToken = null;
            try
            {
                tokenHandler.ValidateToken(token.IdToken, validationParameters, out validatedToken);
                payload = tokenHandler.ReadJwtToken(token.IdToken).Payload;
            }
            catch (SecurityTokenSignatureKeyNotFoundException)
            {
                //Ignore SecurityTokenSignatureKeyNotFoundException
                payload = tokenHandler.ReadJwtToken(token.IdToken).Payload;
            }
            catch (SecurityTokenException e)
            {
                throw new InvalidParameterException(e.Message.ToString());
            }

            bool emailVerify = false;
            string emailVerifyString = payload.Claims.FirstOrDefault(c => c.Type.Equals("email_verified")).Value;

            if (!string.IsNullOrEmpty(emailVerifyString))
            {
                _ = bool.TryParse(emailVerifyString, out emailVerify);
                if (emailVerify)
                {
                    if (payload != null)
                    {
                        var email = payload.Claims.FirstOrDefault(c => c.Type.Equals("email")).Value;
                        var account = accountService.GetByEmail(email);
                        if (account != null)
                        {
                            return Task.FromResult(account);
                        }

                        switch (role)
                        {
                            case "Brand":
                                account = accountService.AddGoogle(new CreateGoogleAccountModel
                                {
                                    Email = email,
                                    IsVerify = true,
                                    Role = (int)Role.Brand,
                                    Description = "Create by logging in with Google",
                                    State = true,
                                });
                                emailService.SendEmailBrandRegister(account.Email);

                                var brand = brandService.AddGoogle(new CreateBrandGoogleModel
                                {
                                    AccountId = account.Id,
                                    BrandName = email,
                                    Email = email,
                                    Description = null,
                                    State = true,
                                });
                                account.UserId = brand.Id;
                                account.Name = brand.BrandName;
                                return Task.FromResult(account);
                            case "Student":
                                account = accountService.AddGoogle(new CreateGoogleAccountModel
                                {
                                    Email = email,
                                    IsVerify = false,
                                    Role = (int)Role.Student,
                                    Description = "Create by logging in with Google",
                                    State = true,
                                });
                                return Task.FromResult(account);
                        }
                    }
                    throw new InvalidParameterException("Mã thông báo không hợp lệ");
                }
            }
            throw new InvalidParameterException("Địa chỉ email chưa được xác thực");
        }
        catch (Exception e)
        {
            throw new InvalidParameterException(e.Message);
        }
    }
}
