using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Types;
using Unibean.Service.Services.Interfaces;

namespace Unibean.API.Controllers;

[Route("api/v1/types")]
[ApiController]
public class TypeController : ControllerBase
{
    private readonly ITypeService typeService;

    public TypeController(ITypeService typeService)
    {
        this.typeService = typeService;
    }

    /// <summary>
    /// Get activity's type by id
    /// </summary>
    [HttpGet("{id}")]
    //[Authorize(Roles = "Admin")]
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
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(TypeModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult Create([FromBody] CreateTypeModel creator)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        var type = typeService.Add(creator);
        if (type != null)
        {
            return StatusCode(StatusCodes.Status201Created, type);
        }
        return NotFound("Create fail");
    }
}
