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
    /// <param name="studentIds">Filter by student Id.</param>
    /// <param name="brandIds">Filter by brand Id.</param>
    /// <param name="state">Filter by wishlists state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(PagedResultModel<WishlistModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
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
            return Ok(result);
        }
        return BadRequest("Invalid property of wishlist");
    }

    /// <summary>
    /// Create wishlist
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin, Student")]
    [ProducesResponseType(typeof(WishlistModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
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
            return NotFound("Update fail");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }
}
