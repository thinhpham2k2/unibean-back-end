﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Types;
using Unibean.Service.Services.Interfaces;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("Activity Type API")]
[Route("api/v1/types")]
public class TypeController : ControllerBase
{
    private readonly ITypeService typeService;

    public TypeController(ITypeService typeService)
    {
        this.typeService = typeService;
    }

    /// <summary>
    /// Get activity's type list
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(PagedResultModel<TypeModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public ActionResult<PagedResultModel<TypeModel>> GetList(
        [FromQuery] string sort = "Id,desc",
        [FromQuery] string search = "",
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string propertySort = sort.Split(",")[0];
        var propertyInfo = typeof(Type).GetProperty(propertySort);
        if (propertySort != null && propertyInfo != null)
        {
            PagedResultModel<TypeModel>
                result = typeService.GetAll
                (propertySort, sort.Split(",")[1].Equals("asc"), search, page, limit);
            return Ok(result);
        }
        return BadRequest("Invalid property of activity's type");
    }

    /// <summary>
    /// Get activity's type by id
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(TypeModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult GetById(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return Ok(typeService.GetById(id));
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Create activity's type
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(TypeModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Create([FromForm] CreateTypeModel creation)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var type = await typeService.Add(creation);
            if (type != null)
            {
                return StatusCode(StatusCodes.Status201Created, type);
            }
            return NotFound("Create fail");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Update activity's type
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(TypeModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Update(string id, [FromForm] UpdateTypeModel update)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var type = await typeService.Update(id, update);
            if (type != null)
            {
                return StatusCode(StatusCodes.Status200OK, type);
            }
            return NotFound("Update fail");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Delete activity's type
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult Delete(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            typeService.Delete(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }
}