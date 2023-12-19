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
            .ForMember(a => a.UserId, opt => opt.MapFrom((src, dest) =>
            {
                dest.UserId = "UserId nè";
                switch (src.Role.RoleName)
                {
                    case "Admin":
                        return src.Admins.FirstOrDefault().Id;
                    case "Brand":
                        return src.Brands.FirstOrDefault().Id;
                    case "Store":
                        return src.Stores.FirstOrDefault().Id;
                    case "Student":
                        return src.Students.FirstOrDefault().Id;
                    default:
                        return null;
                }
            }))
            .ForMember(a => a.Name, opt => opt.MapFrom((src, dest) =>
            {
                dest.Name = "Name nè";
                switch (src.Role.RoleName)
                {
                    case "Admin":
                        return src.Admins.FirstOrDefault().FullName;
                    case "Brand":
                        return src.Brands.FirstOrDefault().BrandName;
                    case "Store":
                        return src.Stores.FirstOrDefault().StoreName;
                    case "Student":
                        return src.Students.FirstOrDefault().FullName;
                    default:
                        return null;
                }
            }))
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.accountRepository = accountRepository;
        this.fireBaseService = fireBaseService;
    }
    public AccountModel GetByUserNameAndPassword(string userName, string password)
    {
        return mapper.Map<AccountModel>(accountRepository.GetByUserNameAndPassword(userName, password));
    }
}
