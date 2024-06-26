﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Files;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.VoucherItems;
using Unibean.Service.Services.Interfaces;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("🎟️Voucher Item API")]
[Route("api/v1/voucher-items")]
public class VoucherItemController : ControllerBase
{
    private readonly IVoucherItemService voucherItemService;

    private readonly IJwtService jwtService;

    public VoucherItemController
        (IVoucherItemService voucherItemService, IJwtService jwtService)
    {
        this.voucherItemService = voucherItemService;
        this.jwtService = jwtService;
    }

    /// <summary>
    /// Get voucher item list
    /// </summary>
    /// <param name="brandIds">Filter by brand id.</param>
    /// <param name="campaignIds">Filter by campaign id.</param>
    /// <param name="campaignDetailIds">Filter by campaign detail id.</param>
    /// <param name="voucherIds">Filter by voucher id.</param>
    /// <param name="typeIds">Filter by type id.</param>
    /// <param name="studentIds">Filter by student id.</param>
    /// <param name="isLocked">Filter by lock state.</param>
    /// <param name="isBought">Filter by bought state.</param>
    /// <param name="isUsed">Filter by used state.</param>
    /// <param name="state">Filter by voucher item state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet]
    [Authorize(Roles = "Admin, Brand, Student")]
    [ProducesResponseType(typeof(PagedResultModel<VoucherItemModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<VoucherItemModel>> GetList(
        [FromQuery] List<string> brandIds,
        [FromQuery] List<string> campaignIds,
        [FromQuery] List<string> campaignDetailIds,
        [FromQuery] List<string> voucherIds,
        [FromQuery] List<string> typeIds,
        [FromQuery] List<string> studentIds,
        [FromQuery] bool? isLocked,
        [FromQuery] bool? isBought,
        [FromQuery] bool? isUsed,
        [FromQuery] bool? state,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string propertySort = paging.Sort.Split(",")[0];
        var propertyInfo = typeof(VoucherItem).GetProperty(propertySort);
        if (propertySort != null && propertyInfo != null)
        {
            PagedResultModel<VoucherItemModel>
                result = voucherItemService.GetAll
                (campaignIds, campaignDetailIds, voucherIds, brandIds, typeIds, studentIds, isLocked, isBought, isUsed,
                state, propertySort, paging.Sort.Split(",")[1].Equals("asc"), paging.Search, paging.Page, paging.Limit);
            return StatusCode(StatusCodes.Status200OK, result);
        }
        return StatusCode(StatusCodes.Status400BadRequest, "Thuộc tính không hợp lệ của mục khuyến mãi");
    }

    /// <summary>
    /// Get voucher item by id
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Brand, Student")]
    [ProducesResponseType(typeof(VoucherItemExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GetById(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return StatusCode(StatusCodes.Status200OK, voucherItemService.GetById(id));
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Create voucher item (auto-generated)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin, Brand")]
    [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult Create([FromBody] CreateVoucherItemModel creation)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return File(voucherItemService.Add(creation).ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Result(" + DateTime.UtcNow.ToString("R") + ").xlsx");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Delete voucher item
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin, Brand")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult Delete(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            voucherItemService.Delete(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Get insert voucher item template
    /// </summary>
    [HttpGet("template")]
    [Authorize(Roles = "Admin, Brand")]
    [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GetTemplate()
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return File(voucherItemService.GetTemplateVoucherItem().ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Template_Unibean.xlsx");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Import voucher item template
    /// </summary>
    [HttpPost("template")]
    [Authorize(Roles = "Brand")]
    [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> ImportTemplate([FromForm] InsertVoucherItemModel insert)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string jwtToken = HttpContext.Request.Headers["Authorization"];

        try
        {
            MemoryStreamModel result = await voucherItemService.AddTemplate
                (insert, jwtService.GetJwtRequest(jwtToken.Split(" ")[1]));
            return File(result.Ms.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                result.IsValid ?
                "Result(" + DateTime.UtcNow.ToString("R") + ").xlsx" : "Exception");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }
}
