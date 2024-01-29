using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.ChallengeTypes;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Services.Interfaces;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("🎯Challenge Type API")]
[Route("api/v1/challenge-types")]
public class ChallengeTypeController : ControllerBase
{
    private readonly IChallengeTypeService challengeTypeService;

    public ChallengeTypeController(IChallengeTypeService challengeTypeService)
    {
        this.challengeTypeService = challengeTypeService;
    }

    /// <summary>
    /// Get challenge type list
    /// </summary>
    /// <param name="state">Filter by challenge type state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(PagedResultModel<ChallengeTypeModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<ChallengeTypeModel>> GetList(
        [FromQuery] bool? state,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string propertySort = paging.Sort.Split(",")[0];
        var propertyInfo = typeof(ChallengeType).GetProperty(propertySort);
        if (propertySort != null && propertyInfo != null)
        {
            PagedResultModel<ChallengeTypeModel>
                result = challengeTypeService.GetAll
                (state, propertySort, paging.Sort.Split(",")[1].Equals("asc"), 
                paging.Search, paging.Page, paging.Limit);
            return StatusCode(StatusCodes.Status200OK, result);
        }
        return StatusCode(StatusCodes.Status400BadRequest, "Thuộc tính không hợp lệ của loại thử thách");
    }

    /// <summary>
    /// Get challenge type by id
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(ChallengeTypeModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GetById(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return StatusCode(StatusCodes.Status200OK, challengeTypeService.GetById(id));
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Create challenge type
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ChallengeTypeModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> Create([FromForm] CreateChallengeTypeModel creation)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var type = await challengeTypeService.Add(creation);
            if (type != null)
            {
                return StatusCode(StatusCodes.Status201Created, type);
            }
            return StatusCode(StatusCodes.Status404NotFound, "Tạo thất bại");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Update challenge type
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ChallengeTypeModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> Update(string id, [FromForm] UpdateChallengeTypeModel update)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var type = await challengeTypeService.Update(id, update);
            if (type != null)
            {
                return StatusCode(StatusCodes.Status200OK, type);
            }
            return StatusCode(StatusCodes.Status404NotFound, "Cập nhật thất bại");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Delete challenge type
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult Delete(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            challengeTypeService.Delete(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }
}
