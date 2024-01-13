using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
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
            .ForMember(s => s.UserName, opt => opt.MapFrom(src => src.Account.UserName))
            .ForMember(s => s.Avatar, opt => opt.MapFrom(src => src.Account.Avatar))
            .ForMember(s => s.Email, opt => opt.MapFrom(src => src.Account.Email))
            .ForMember(s => s.Phone, opt => opt.MapFrom(src => src.Account.Phone))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Store>, PagedResultModel<StoreModel>>()
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.storeRepository = storeRepository;
        this.fireBaseService = fireBaseService;
    }

    public PagedResultModel<StoreModel> GetAll
        (List<string> brandIds, List<string> areaIds, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<StoreModel>>
            (storeRepository.GetAll(brandIds, areaIds, propertySort, isAsc, search, page, limit));
    }
}
