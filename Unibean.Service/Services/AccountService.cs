using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Accounts;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class AccountService : IAccountService
{
    private readonly Mapper mapper;

    private readonly IAccountRepository accountRepository;

    private readonly IFireBaseService fireBaseService;

    public AccountService(IAccountRepository accountRepository,
        IFireBaseService fireBaseService)
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
        });
        mapper = new Mapper(config);
        this.accountRepository = accountRepository;
        this.fireBaseService = fireBaseService;
    }

    public AccountModel AddGoogle(CreateGoogleAccountModel creation)
    {
        return mapper.Map<AccountModel>(accountRepository.Add(mapper.Map<Account>(creation)));
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
