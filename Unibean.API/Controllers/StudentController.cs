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
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Validations;

namespace Unibean.API.Controllers;

[ApiController]
[Tags("🎓Student API")]
[Route("api/v1/students")]
public class StudentController : ControllerBase
{
    private readonly IStudentService studentService;

    private readonly IOrderService orderService;

    public StudentController(IStudentService studentService,
        IOrderService orderService)
    {
        this.studentService = studentService;
        this.orderService = orderService;
    }

    /// <summary>
    /// Get student list
    /// </summary>
    /// <param name="majorIds">Filter by major id.</param>
    /// <param name="campusIds">Filter by campus id.</param>
    /// <param name="stateIds">Filter by student state --- Pending = 1, Active = 2, Inactive = 3, Rejected = 4</param>
    /// <param name="isVerify">Filter by student verification status.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PagedResultModel<StudentModel>),
        (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<StudentModel>> GetList(
        [FromQuery] List<string> majorIds,
        [FromQuery] List<string> campusIds,
        [FromQuery] List<StudentState> stateIds,
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
                (majorIds, campusIds, stateIds, isVerify, propertySort,
                paging.Sort.Split(",")[1].Equals("asc"), paging.Search, paging.Page, paging.Limit);
            return StatusCode(StatusCodes.Status200OK, result);
        }
        return StatusCode(StatusCodes.Status400BadRequest, "Thuộc tính không hợp lệ của sinh viên");
    }

    /// <summary>
    /// Get student by id
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Brand, Store, Student")]
    [ProducesResponseType(typeof(StudentExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GetById(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return StatusCode(StatusCodes.Status200OK, studentService.GetById(id));
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Create student
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(StudentModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
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
            return StatusCode(StatusCodes.Status404NotFound, "Tạo thất bại");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Update student
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, Student")]
    [ProducesResponseType(typeof(StudentExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
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
            return StatusCode(StatusCodes.Status404NotFound, "Cập nhật thất bại");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Update student
    /// </summary>
    [HttpPut("{id}/verification")]
    [Authorize(Roles = "Admin, Student")]
    [ProducesResponseType(typeof(StudentExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> UpdateVerification(
        [ValidStudent(new[] { StudentState.Rejected })] string id,
        [FromForm] UpdateStudentVerifyModel update)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            var student = await studentService.UpdateVerification(id, update);
            if (student != null)
            {
                return StatusCode(StatusCodes.Status200OK, student);
            }
            return StatusCode(StatusCodes.Status404NotFound, "Cập nhật thất bại");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Update student state
    /// </summary>
    /// <param name="id">Student id.</param>
    /// <param name="stateId">Student state id --- Active = 2, Inactive = 3, Rejected = 4</param>
    [HttpPut("{id}/states/{stateId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult UpdateState(
        [ValidStudent(new[] {
            StudentState.Pending,
            StudentState.Active,
            StudentState.Inactive })] string id,
        StudentState stateId)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            if (studentService.UpdateState(id, stateId))
            {
                return StatusCode(StatusCodes.Status200OK, "Cập nhật trạng thái sinh viên thành công");
            }
            return NotFound("Cập nhật trạng thái sinh viên thất bại");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Delete student
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
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
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Get challenge list by student id
    /// </summary>
    /// <param name="id">Student id.</param>
    /// <param name="typeIds">Filter by challenge type id --- Verify = 1, Welcome = 2, Spread = 3, Consume = 4</param>
    /// <param name="state">Filter by challenge state.</param>
    /// <param name="isCompleted">Filter by challenge completion status.</param>
    /// <param name="isClaimed">Filter by challenge claim status.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/challenges")]
    [Authorize(Roles = "Admin, Student")]
    [ProducesResponseType(typeof(PagedResultModel<StudentChallengeModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<StudentChallengeModel>> GetChallengeListByStudentId(string id,
        [FromQuery] List<ChallengeType> typeIds,
        [FromQuery] bool? state,
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
                result = studentService.GetChallengeListByStudentId
                    (typeIds, id, state, isCompleted, isClaimed, propertySort,
                    paging.Sort.Split(",")[1].Equals("asc"), paging.Search, paging.Page, paging.Limit);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Thuộc tính của thách thức không hợp lệ");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Get history transaction by student id
    /// </summary>
    /// <param name="id">Student id.</param>
    /// <param name="typeIds">Filter by transaction type id --- ActivityTransaction = 1, OrderTransaction = 2, ChallengeTransaction = 3, BonusTransaction = 4</param>
    /// <param name="state">Filter by history transaction state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/histories")]
    [Authorize(Roles = "Admin, Student")]
    [ProducesResponseType(typeof(PagedResultModel<TransactionModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<TransactionModel>> GetHistoryTransactionListByStudentId(string id,
        [FromQuery] List<TransactionType> typeIds,
        [FromQuery] bool? state,
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
                result = studentService.GetHistoryTransactionListByStudentId
                    (id, typeIds, state, propertySort, paging.Sort.Split(",")[1].Equals("asc"),
                    paging.Search, paging.Page, paging.Limit);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Thuộc tính của giao dịch không hợp lệ");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Get order list by student id
    /// </summary>
    /// <param name="id">Student id.</param>
    /// <param name="stationIds">Filter by station id.</param>
    /// <param name="stateIds">Filter by state id --- Order = 1, Confirmation = 2, Preparation = 3, Arrival = 4, Receipt = 5, Abort = 6</param>
    /// <param name="state">Filter by order state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/orders")]
    [Authorize(Roles = "Admin, Student")]
    [ProducesResponseType(typeof(PagedResultModel<OrderModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<OrderModel>> GetOrderListByStudentId(string id,
        [FromQuery] List<string> stationIds,
        [FromQuery] List<State> stateIds,
        [FromQuery] bool? state,
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
                    (stationIds, stateIds, id, state, propertySort.Equals("StateCurrent")
                        ? "OrderStates.Max(s => s.StateId)" : propertySort, paging.Sort.Split(",")[1].Equals("asc"), paging.Search, paging.Page, paging.Limit);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Thuộc tính của đơn đặt hàng không hợp lệ");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Create order
    /// </summary>
    /// <param name="id">Student id.</param>
    /// <param name="create">Create order.</param>
    [HttpPost("{id}/orders")]
    [Authorize(Roles = "Admin, Student")]
    [ProducesResponseType(typeof(OrderExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult CreateOrder([ValidStudent(new[] { StudentState.Active })] string id, [FromBody] CreateOrderModel create)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return StatusCode(StatusCodes.Status201Created,
                orderService.Add(id, create));
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
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
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GetOrderById(string id, string orderId)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return StatusCode(StatusCodes.Status200OK, studentService.GetOrderByOrderId(id, orderId));
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Get voucher list by student id
    /// </summary>
    /// <param name="id">Student id.</param>
    /// <param name="campaignIds">Filter by campaign id.</param>
    /// <param name="voucherIds">Filter by voucher id.</param>
    /// <param name="brandIds">Filter by brand id.</param>
    /// <param name="typeIds">Filter by voucher type id.</param>
    /// <param name="state">Filter by voucher state.</param>
    /// <param name="paging">Paging parameter.</param>
    [HttpGet("{id}/vouchers")]
    [Authorize(Roles = "Admin, Student")]
    [ProducesResponseType(typeof(PagedResultModel<VoucherItemModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<PagedResultModel<OrderModel>> GetVoucherListByStudentId(string id,
        [FromQuery] List<string> campaignIds,
        [FromQuery] List<string> voucherIds,
        [FromQuery] List<string> brandIds,
        [FromQuery] List<string> typeIds,
        [FromQuery] bool? state,
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
                    (campaignIds, voucherIds, brandIds, typeIds, id, state,
                    propertySort, paging.Sort.Split(",")[1].Equals("asc"),
                    paging.Search, paging.Page, paging.Limit);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Thuộc tính không hợp lệ của khuyến mãi");
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Get voucher item by student id
    /// </summary>
    /// <param name="id">Student id.</param>
    /// <param name="voucherId">Voucher id.</param>
    [HttpGet("{id}/vouchers/{voucherId}")]
    [Authorize(Roles = "Admin, Student")]
    [ProducesResponseType(typeof(VoucherItemExtraModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GetVoucherById(string id, string voucherId)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return StatusCode(StatusCodes.Status200OK, studentService.GetVoucherItemByVoucherId(id, voucherId));
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <summary>
    /// Get wishlists by student id
    /// </summary>
    /// <param name="id">Student id.</param>
    [HttpGet("{id}/wishlists")]
    [Authorize(Roles = "Admin, Student")]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
    public ActionResult<List<string>> GetWishlistsByStudentId(string id)
    {
        if (!ModelState.IsValid) throw new InvalidParameterException(ModelState);

        try
        {
            return StatusCode(StatusCodes.Status200OK, studentService.GetWishlistsByStudentId(id));
        }
        catch (InvalidParameterException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
    }
}
