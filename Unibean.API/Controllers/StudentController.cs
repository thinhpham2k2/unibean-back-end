﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Students;
using Unibean.Service.Services.Interfaces;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("Student API")]
[Route("api/v1/students")]
public class StudentController : ControllerBase
{
    private readonly IStudentService studentService;

    public StudentController(IStudentService studentService)
    {
        this.studentService = studentService;
    }

    /// <summary>
    /// Get student list
    /// </summary>
    /// <param name="levelIds">Filter by city Id.</param>
    /// <param name="genderIds">Filter by gender Id.</param>
    /// <param name="majorIds">Filter by major Id.</param>
    /// <param name="campusIds">Filter by campus Id.</param>
    /// <param name="isVerify">Filter by student verification status.</param>
    /// <param name="sort">Sorting criteria for the results.</param>
    /// <param name="search">Search query.</param>
    /// <param name="page">Current page in the paginated results.</param>
    /// <param name="limit">Number of results per page.</param>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PagedResultModel<StudentModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public ActionResult<PagedResultModel<StudentModel>> GetList(
        [FromQuery] List<string> levelIds,
        [FromQuery] List<string> genderIds,
        [FromQuery] List<string> majorIds,
        [FromQuery] List<string> campusIds,
        [FromQuery] bool? isVerify,
        [FromQuery] string sort = "Id,desc",
        [FromQuery] string search = "",
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string propertySort = sort.Split(",")[0];
        var propertyInfo = typeof(Student).GetProperty(propertySort);
        if (propertySort != null && propertyInfo != null)
        {
            PagedResultModel<StudentModel>
                result = studentService.GetAll
                (levelIds, genderIds, majorIds, campusIds, isVerify, propertySort, sort.Split(",")[1].Equals("asc"), search, page, limit);
            return Ok(result);
        }
        return BadRequest("Invalid property of student");
    }
}