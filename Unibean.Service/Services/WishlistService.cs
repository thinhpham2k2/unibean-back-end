using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Campuses;
using Unibean.Service.Models.Wishlists;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class WishlistService : IWishlistService
{

    private readonly Mapper mapper;

    private readonly IWishlistRepository wishlistRepository;

    public WishlistService(IWishlistRepository wishlistRepository)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Wishlist, WishlistModel>()
            .ForMember(c => c.StudentName, opt => opt.MapFrom(src => src.Student.FullName))
            .ForMember(c => c.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Wishlist>, PagedResultModel<WishlistModel>>()
            .ReverseMap();
            cfg.CreateMap<Wishlist, UpdateWishlistModel>()
            .ReverseMap()
            .AfterMap((src, dest) => dest.Status = !dest.Status);
        });
        mapper = new Mapper(config);
        this.wishlistRepository = wishlistRepository;
    }

    public PagedResultModel<WishlistModel> GetAll
        (List<string> studentIds, List<string> brandIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<WishlistModel>>
            (wishlistRepository.GetAll(studentIds, brandIds,
            state, propertySort, isAsc, search, page, limit));
    }

    public WishlistModel UpdateWishlist(UpdateWishlistModel update)
    {
        Wishlist wishlist = wishlistRepository.GetByStudentAndBrand(update.StudentId, update.BrandId);
        if (wishlist != null)
        {
            wishlist = mapper.Map(update, wishlist);
            return mapper.Map<WishlistModel>(wishlistRepository.Update(wishlist));
        }
        else
        {
            return mapper.Map<WishlistModel>(wishlistRepository.Add(new Wishlist
            {
                Id = Ulid.NewUlid().ToString(),
                StudentId = update.StudentId,
                BrandId = update.BrandId,
                Description = update.Description,
                State = update.State,
                Status = true
            }));
        }
    }
}
