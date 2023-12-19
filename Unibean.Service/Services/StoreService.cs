using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Stores;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class StoreService : IStoreService
{
    private readonly Mapper mapper;

    private readonly IStoreRepository storeRepository;

    private readonly IFireBaseService fireBaseService;

    public StoreService(IStoreRepository storeRepository, 
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Store, StoreModel>()
            .ForMember(s => s.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
            .ForMember(s => s.AreaName, opt => opt.MapFrom(src => src.Area.AreaName))
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.storeRepository = storeRepository;
        this.fireBaseService = fireBaseService;
    }
}
