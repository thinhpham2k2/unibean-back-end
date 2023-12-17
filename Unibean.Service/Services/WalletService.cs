using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Partners;
using Unibean.Service.Models.Wallets;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class WalletService : IWalletService
{
    private readonly Mapper mapper;

    private readonly IWalletRepository walletRepository;

    public WalletService(IWalletRepository walletRepository)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Wallet, WalletModel>().ReverseMap();
            cfg.CreateMap<Wallet, CreateWalletModel>()
            .ReverseMap()
            .ForMember(p => p.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(p => p.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(p => p.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(p => p.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.walletRepository = walletRepository;
    }

    public WalletModel Add(CreateWalletModel creation)
    {
        return mapper.Map<WalletModel>(walletRepository.Add(mapper.Map<Wallet>(creation)));
    }
}
