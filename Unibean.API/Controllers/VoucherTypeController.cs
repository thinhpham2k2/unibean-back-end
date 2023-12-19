﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.VoucherTypes;
using Unibean.Service.Services.Interfaces;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("Voucher Type API")]
[Route("api/v1/voucher-types")]
public class VoucherTypeController : ControllerBase
{
    private readonly IVoucherTypeService voucherTypeService;

    public VoucherTypeController(IVoucherTypeService voucherTypeService)
    {
        this.voucherTypeService = voucherTypeService;
    }

    /// <summary>
    /// Get voucher's type list
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(PagedResultModel<VoucherTypeModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public ActionResult<PagedResultModel<VoucherTypeModel>> GetList(
        [FromQuery] string sort = "Id,desc",
        [FromQuery] string search = "",
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string propertySort = sort.Split(",")[0];
        var propertyInfo = typeof(VoucherType).GetProperty(propertySort);
        if (propertySort != null && propertyInfo != null)
        {
            PagedResultModel<VoucherTypeModel>
                result = voucherTypeService.GetAll
                (propertySort, sort.Split(",")[1].Equals("asc"), search, page, limit);
            return Ok(result);
        }
        return BadRequest("Invalid property of voucher's type");
    }

    /// <summary>
    /// Get voucher's type by id
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(VoucherTypeModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult GetById(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return Ok(voucherTypeService.GetById(id));
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Create voucher's type
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(VoucherTypeModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Create([FromForm] CreateVoucherTypeModel creation)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var type = await voucherTypeService.Add(creation);
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
    /// Update voucher's type
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(VoucherTypeModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Update(string id, [FromForm] UpdateVoucherTypeModel update)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var type = await voucherTypeService.Update(id, update);
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
    /// Delete voucher's type
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
            voucherTypeService.Delete(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }
}