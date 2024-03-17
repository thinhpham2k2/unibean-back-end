using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Wishlists;
using Unibean.Service.Services;

namespace Unibean.Test.Services;

public class WishlistServiceTest
{
    private readonly IWishlistRepository wishlistRepository;

    public WishlistServiceTest()
    {
        wishlistRepository = A.Fake<IWishlistRepository>();
    }

    [Fact]
    public void WishlistService_GetAll()
    {
        // Arrange
        List<string> studentIds = new();
        List<string> brandIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<Wishlist> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => wishlistRepository.GetAll(studentIds, brandIds, state,
            propertySort, isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new WishlistService(wishlistRepository);

        // Act
        var result = service.GetAll(studentIds, brandIds, state, propertySort,
            isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<WishlistModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void WishlistService_Update_New()
    {
        // Arrange
        string id = "id";
        UpdateWishlistModel update = new()
        {
            StudentId = id,
            BrandId = id
        };
        A.CallTo(() => wishlistRepository.GetByStudentAndBrand(id, id))
            .Returns(null);
        var service = new WishlistService(wishlistRepository);

        // Act
        var result = service.UpdateWishlist(update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(WishlistModel));
        Assert.NotEqual(id, result.Id);
    }

    [Fact]
    public void WishlistService_Update_Old()
    {
        // Arrange
        string id = "id";
        UpdateWishlistModel update = new()
        {
            StudentId = id,
            BrandId = id
        };
        A.CallTo(() => wishlistRepository.GetByStudentAndBrand(id, id));
        A.CallTo(() => wishlistRepository.Update(A<Wishlist>.Ignored))
            .Returns(new()
            {
                Id = id,
            });
        var service = new WishlistService(wishlistRepository);

        // Act
        var result = service.UpdateWishlist(update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(WishlistModel));
        Assert.Equal(id, result.Id);
    }
}
