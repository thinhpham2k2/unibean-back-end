using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.Wishlists;
using Unibean.Service.Services.Interfaces;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("🛎️Wishlist API")]
[Route("api/v1/wishlists")]
public class WishlistController : ControllerBase
{
    private readonly IWishlistService wishlistService;

    public WishlistController(IWishlistService wishlistService)
    {
        this.wishlistService = wishlistService;
    }

    /// <summary>
    /// Get wishlists
    /// </summary>
    /// <param name="studentIds">Filter by student id.</param>
    /// <param name="brandIds">Filter by brand id.</param>
    /// <param name="state">Filter by wishlists state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(PagedResultModel<WishlistModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<WishlistModel>> GetList(
        [FromQuery] List<string> studentIds,
        [FromQuery] List<string> brandIds,
        [FromQuery] bool? state,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string propertySort = paging.Sort.Split(",")[0];
        var propertyInfo = typeof(Wishlist).GetProperty(propertySort);
        if (propertySort != null && propertyInfo != null)
        {
            PagedResultModel<WishlistModel>
                result = wishlistService.GetAll
                (studentIds, brandIds, state, propertySort, paging.Sort.Split(",")[1].Equals("asc"), 
                paging.Search, paging.Page, paging.Limit);
            return StatusCode(StatusCodes.Status200OK, result);
        }
        return StatusCode(StatusCodes.Status400BadRequest, "Thuộc tính không hợp lệ của danh sách mong muốn");
    }

    /// <summary>
    /// Create wishlist
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin, Student")]
    [ProducesResponseType(typeof(WishlistModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult Update([FromBody] UpdateWishlistModel update)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var wishlist = wishlistService.UpdateWishlist(update);
            if (wishlist != null)
            {
                return StatusCode(StatusCodes.Status201Created, wishlist);
            }
            return StatusCode(StatusCodes.Status404NotFound, "Cập nhật thất bại");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }
}
