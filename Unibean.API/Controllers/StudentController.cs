using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Orders;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.StudentChallenges;
using Unibean.Service.Models.Students;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Models.VoucherItems;
using Unibean.Service.Services.Interfaces;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("🎓Student API")]
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
    /// <param name="levelIds">Filter by level Id.</param>
    /// <param name="genderIds">Filter by gender Id.</param>
    /// <param name="majorIds">Filter by major Id.</param>
    /// <param name="stationIds">Filter by station Id.</param>
    /// <param name="isVerify">Filter by student verification status.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PagedResultModel<StudentModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public ActionResult<PagedResultModel<StudentModel>> GetList(
        [FromQuery] List<string> levelIds,
        [FromQuery] List<string> genderIds,
        [FromQuery] List<string> majorIds,
        [FromQuery] List<string> stationIds,
        [FromQuery] bool? isVerify,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        string propertySort = paging.Sort.Split(",")[0];
        var propertyInfo = typeof(Student).GetProperty(propertySort);
        if (propertySort != null && propertyInfo != null)
        {
            PagedResultModel<StudentModel>
                result = studentService.GetAll
                (levelIds, genderIds, majorIds, stationIds, isVerify, propertySort, paging.Sort.Split(",")[1].Equals("asc"), paging.Search, paging.Page, paging.Limit);
            return Ok(result);
        }
        return BadRequest("Invalid property of student");
    }

    /// <summary>
    /// Get student by id
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(StudentExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult GetById(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return Ok(studentService.GetById(id));
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Create student
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(StudentModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Create([FromForm] CreateStudentModel creation)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var student = await studentService.Add(creation);
            if (student != null)
            {
                return StatusCode(StatusCodes.Status201Created, student);
            }
            return NotFound("Create fail");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Update student
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(StudentExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Update(string id, [FromForm] UpdateStudentModel update)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var student = await studentService.Update(id, update);
            if (student != null)
            {
                return StatusCode(StatusCodes.Status200OK, student);
            }
            return NotFound("Update fail");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Delete student
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
            studentService.Delete(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Get challenge list by student id
    /// </summary>
    /// <param name="id">Student id.</param>
    /// <param name="isCompleted">Filter by challenge completion status.</param>
    /// <param name="isClaimed">Filter by challenge claim status.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/challenges")]
    [Authorize(Roles = "Admin, Student")]
    [ProducesResponseType(typeof(PagedResultModel<StudentChallengeModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public ActionResult<PagedResultModel<StudentChallengeModel>> GetChallengeByStudentId(string id,
        [FromQuery] bool? isCompleted,
        [FromQuery] bool? isClaimed,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            string propertySort = paging.Sort.Split(",")[0];
            var propertyInfo = typeof(StudentChallenge).GetProperty(propertySort);
            if (propertySort != null && propertyInfo != null)
            {
                PagedResultModel<StudentChallengeModel>
                result = studentService.GetChallengeByStudentId
                    (id, isCompleted, isClaimed, propertySort, paging.Sort.Split(",")[1].Equals("asc"), paging.Search, paging.Page, paging.Limit);
                return Ok(result);
            }
            return BadRequest("Invalid property of challenge");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Get history transaction by student id
    /// </summary>
    /// <param name="id">Student id.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/histories")]
    [Authorize(Roles = "Admin, Student")]
    [ProducesResponseType(typeof(PagedResultModel<TransactionModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public ActionResult<PagedResultModel<TransactionModel>> GetHistoryTransactionByStudentId(string id,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            string propertySort = paging.Sort.Split(",")[0];
            var propertyInfo = typeof(TransactionModel).GetProperty(propertySort);
            if (propertySort != null && propertyInfo != null)
            {
                PagedResultModel<TransactionModel>
                result = studentService.GetHistoryTransactionByStudentId
                    (id, propertySort, paging.Sort.Split(",")[1].Equals("asc"), paging.Search, paging.Page, paging.Limit);
                return Ok(result);
            }
            return BadRequest("Invalid property of transaction");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Get order list by student id
    /// </summary>
    /// <param name="id">Student id.</param>
    /// <param name="stationIds">Filter by station Id.</param>
    /// <param name="stateIds">Filter by state Id.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/orders")]
    [Authorize(Roles = "Admin, Student")]
    [ProducesResponseType(typeof(PagedResultModel<OrderModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public ActionResult<PagedResultModel<OrderModel>> GetOrderListByStudentId(string id,
        [FromQuery] List<string> stationIds,
        [FromQuery] List<string> stateIds,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            string propertySort = paging.Sort.Split(",")[0];
            var propertyInfo = typeof(Order).GetProperty(propertySort);
            if (propertySort != null && propertyInfo != null || propertySort.Equals("StateCurrent"))
            {
                PagedResultModel<OrderModel>
                result = studentService.GetOrderListByStudentId
                    (stationIds, stateIds, id, propertySort.Equals("StateCurrent")
                        ? "OrderStates.Max(s => s.StateId)" : propertySort, paging.Sort.Split(",")[1].Equals("asc"), paging.Search, paging.Page, paging.Limit);
                return Ok(result);
            }
            return BadRequest("Invalid property of order");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Get order details by student id
    /// </summary>
    /// <param name="id">Student id.</param>
    /// <param name="create">Create order.</param>
    [HttpPost("{id}/orders")]
    [Authorize(Roles = "Student")]
    [ProducesResponseType(typeof(OrderExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult CreateOrder(string id, [FromBody] CreateOrderModel create)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return Ok(create);
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Get order details by student id
    /// </summary>
    /// <param name="id">Student id.</param>
    /// <param name="orderId">Order id.</param>
    [HttpGet("{id}/orders/{orderId}")]
    [Authorize(Roles = "Admin, Student")]
    [ProducesResponseType(typeof(OrderExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public IActionResult GetOrderById(string id, string orderId)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return Ok(studentService.GetOrderByOrderId(id, orderId));
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Get voucher list by student id
    /// </summary>
    /// <param name="id">Student id.</param>
    /// <param name="campaignIds">Filter by campaign Id.</param>
    /// <param name="voucherIds">Filter by voucher Id.</param>
    /// <param name="brandIds">Filter by brand Id.</param>
    /// <param name="typeIds">Filter by voucher type Id.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/vouchers")]
    [Authorize(Roles = "Admin, Student")]
    [ProducesResponseType(typeof(PagedResultModel<VoucherItemModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public ActionResult<PagedResultModel<OrderModel>> GetVoucherListByStudentId(string id,
        [FromQuery] List<string> campaignIds,
        [FromQuery] List<string> voucherIds,
        [FromQuery] List<string> brandIds,
        [FromQuery] List<string> typeIds,
        [FromQuery] PagingModel paging)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            string propertySort = paging.Sort.Split(",")[0];
            var propertyInfo = typeof(VoucherItem).GetProperty(propertySort);
            if (propertySort != null && propertyInfo != null)
            {
                PagedResultModel<VoucherItemModel>
                result = studentService.GetVoucherListByStudentId
                    (campaignIds, voucherIds, brandIds, typeIds, id, propertySort, paging.Sort.Split(",")[1].Equals("asc"), paging.Search, paging.Page, paging.Limit);
                return Ok(result);
            }
            return BadRequest("Invalid property of voucher");
        }
        catch (InvalidParameterException e)
        {
            return BadRequest(e.Message);
        }
    }
}
