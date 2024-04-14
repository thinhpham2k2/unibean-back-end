using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Orders;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Models.StudentChallenges;
using Unibean.Service.Models.Students;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Models.VoucherItems;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class StudentControllerTest
{
    private readonly IStudentService studentService;

    private readonly IOrderService orderService;

    public StudentControllerTest()
    {
        studentService = A.Fake<IStudentService>();
        orderService = A.Fake<IOrderService>();
    }

    [Fact]
    public void StudentController_GetList_ReturnOK()
    {
        // Arrange
        List<string> majorIds = new();
        List<string> campusIds = new();
        List<StudentState> stateIds = new();
        bool? isVerify = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.GetList
            (majorIds, campusIds, stateIds, isVerify, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<StudentModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StudentController_GetList_ReturnBadRequest1()
    {
        // Arrange
        List<string> majorIds = new();
        List<string> campusIds = new();
        List<StudentState> stateIds = new();
        bool? isVerify = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StudentController(studentService, orderService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetList
            (majorIds, campusIds, stateIds, isVerify, paging));
    }

    [Fact]
    public void StudentController_GetList_ReturnBadRequest2()
    {
        // Arrange
        List<string> majorIds = new();
        List<string> campusIds = new();
        List<StudentState> stateIds = new();
        bool? isVerify = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.GetList
            (majorIds, campusIds, stateIds, isVerify, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<StudentModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StudentController_GetById_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => studentService.GetById(id)).Returns(new());
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StudentController_GetById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => studentService.GetById(id)).Throws(new InvalidParameterException());
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StudentController_Create_ReturnCreated()
    {
        // Arrange
        CreateStudentModel create = new();
        A.CallTo(() => studentService.Add(create)).Returns<StudentExtraModel>(new());
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status201Created,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
    [Fact]
    public void StudentController_Create_ReturnBadRequest1()
    {
        // Arrange
        CreateStudentModel create = new();
        var controller = new StudentController(studentService, orderService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(typeof(InvalidParameterException).ToString(),
            result.Exception?.InnerException?.GetType().ToString());
    }

    [Fact]
    public void StudentController_Create_ReturnBadRequest2()
    {
        // Arrange
        CreateStudentModel create = new();
        A.CallTo(() => studentService.Add(create))
        .Throws(new InvalidParameterException());
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StudentController_Create_ReturnNotFound()
    {
        // Arrange
        CreateStudentModel create = new();
        A.CallTo(() => studentService.Add(create)).Returns<StudentExtraModel>(null);
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.Create(create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StudentController_Update_ReturnOK()
    {
        // Arrange
        string id = "";
        UpdateStudentModel update = new();
        A.CallTo(() => studentService.Update(id, update)).Returns<StudentExtraModel>(new());
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StudentController_Update_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        UpdateStudentModel update = new();
        var controller = new StudentController(studentService, orderService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(typeof(InvalidParameterException).ToString(),
            result.Exception?.InnerException?.GetType().ToString());
    }

    [Fact]
    public void StudentController_Update_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        UpdateStudentModel update = new();
        A.CallTo(() => studentService.Update(id, update))
        .Throws(new InvalidParameterException());
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StudentController_Update_ReturnNotFound()
    {
        // Arrange
        string id = "";
        UpdateStudentModel update = new();
        A.CallTo(() => studentService.Update(id, update)).Returns<StudentExtraModel>(null);
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StudentController_UpdateInviteCode_ReturnOK()
    {
        // Arrange
        string id = "";
        string code = "";
        A.CallTo(() => studentService.UpdateInviteCode(id, code))
            .Returns(true);
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.UpdateInviteCode(id, code);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StudentController_UpdateInviteCode_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        string code = "";
        var controller = new StudentController(studentService, orderService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.UpdateInviteCode(id, code));
    }

    [Fact]
    public void StudentController_UpdateInviteCode_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        string code = "";
        A.CallTo(() => studentService.UpdateInviteCode(id, code))
            .Throws(new InvalidParameterException());
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.UpdateInviteCode(id, code);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StudentController_UpdateInviteCode_ReturnNotFound()
    {
        // Arrange
        string id = "";
        string code = "";
        A.CallTo(() => studentService.UpdateInviteCode(id, code))
            .Returns(false);
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.UpdateInviteCode(id, code);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StudentController_UpdateVerification_ReturnOK()
    {
        // Arrange
        string id = "";
        UpdateStudentVerifyModel update = new();
        A.CallTo(() => studentService.UpdateVerification(id, update))
            .Returns<StudentExtraModel>(new());
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.UpdateVerification(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StudentController_UpdateVerification_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        UpdateStudentVerifyModel update = new();
        var controller = new StudentController(studentService, orderService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act
        var result = controller.UpdateVerification(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(typeof(InvalidParameterException).ToString(),
            result.Exception?.InnerException?.GetType().ToString());
    }

    [Fact]
    public void StudentController_UpdateVerification_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        UpdateStudentVerifyModel update = new();
        A.CallTo(() => studentService.UpdateVerification(id, update))
            .Throws(new InvalidParameterException());
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.UpdateVerification(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StudentController_UpdateVerification_ReturnNotFound()
    {
        // Arrange
        string id = "";
        UpdateStudentVerifyModel update = new();
        A.CallTo(() => studentService.UpdateVerification(id, update))
            .Returns<StudentExtraModel>(null);
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.UpdateVerification(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<ActionResult>));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StudentController_UpdateState_ReturnOK()
    {
        // Arrange
        string id = "";
        string note = "note";
        StudentState stateId = new();
        A.CallTo(() => studentService.UpdateState(id, stateId, note))
            .Returns(true);
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.UpdateState(id, stateId, note);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StudentController_UpdateState_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        string note = "note";
        StudentState stateId = new();
        var controller = new StudentController(studentService, orderService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.UpdateState(id, stateId, note));
    }

    [Fact]
    public void StudentController_UpdateState_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        string note = "note";
        StudentState stateId = new();
        A.CallTo(() => studentService.UpdateState(id, stateId, note))
            .Throws(new InvalidParameterException());
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.UpdateState(id, stateId, note);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StudentController_UpdateState_ReturnNotFound()
    {
        // Arrange
        string id = "";
        string note = "note";
        StudentState stateId = new();
        A.CallTo(() => studentService.UpdateState(id, stateId, note))
            .Returns(false);
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.UpdateState(id, stateId, note);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(NotFoundObjectResult));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StudentController_Delete_ReturnNoContent()
    {
        // Arrange
        string id = "";
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StatusCodeResult));
        Assert.Equal(StatusCodes.Status204NoContent,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StudentController_Delete_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => studentService.Delete(id)).Throws(new InvalidParameterException());
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StudentController_GetChallengeListByStudentId_ReturnOK()
    {
        // Arrange
        string id = "";
        List<ChallengeType> typeIds = new();
        bool? state = null;
        bool? isCompleted = null;
        bool? isClaimed = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.GetChallengeListByStudentId
            (id, typeIds, state, isCompleted, isClaimed, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<StudentChallengeModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StudentController_GetChallengeListByStudentId_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        List<ChallengeType> typeIds = new();
        bool? state = null;
        bool? isCompleted = null;
        bool? isClaimed = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StudentController(studentService, orderService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetChallengeListByStudentId
            (id, typeIds, state, isCompleted, isClaimed, paging));
    }

    [Fact]
    public void StudentController_GetChallengeListByStudentId_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        List<ChallengeType> typeIds = new();
        bool? state = null;
        bool? isCompleted = null;
        bool? isClaimed = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.GetChallengeListByStudentId
            (id, typeIds, state, isCompleted, isClaimed, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<StudentChallengeModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StudentController_ClaimChallenge_ReturnCreated()
    {
        // Arrange
        string id = "";
        string challengeId = "";
        A.CallTo(() => studentService.ClaimChallenge(id, challengeId))
            .Returns(new());
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.ClaimChallenge(id, challengeId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status201Created,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StudentController_ClaimChallenge_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        string challengeId = "";
        var controller = new StudentController(studentService, orderService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.ClaimChallenge(id, challengeId));
    }

    [Fact]
    public void StudentController_ClaimChallenge_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        string challengeId = "";
        A.CallTo(() => studentService.ClaimChallenge(id, challengeId))
            .Throws(new InvalidParameterException());
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.ClaimChallenge(id, challengeId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StudentController_ClaimChallenge_ReturnNotFound()
    {
        // Arrange
        string id = "";
        string challengeId = "";
        A.CallTo(() => studentService.ClaimChallenge(id, challengeId))
            .Returns(null);
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.ClaimChallenge(id, challengeId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status404NotFound,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StudentController_GetHistoryTransactionListByStudentId_ReturnOK()
    {
        // Arrange
        string id = "";
        List<TransactionType> typeIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.GetHistoryTransactionListByStudentId
            (id, typeIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<TransactionModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StudentController_GetHistoryTransactionListByStudentId_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        List<TransactionType> typeIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StudentController(studentService, orderService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetHistoryTransactionListByStudentId(id, typeIds, state, paging));
    }

    [Fact]
    public void StudentController_GetHistoryTransactionListByStudentId_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        List<TransactionType> typeIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.GetHistoryTransactionListByStudentId
            (id, typeIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<TransactionModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StudentController_GetOrderListByStudentId_ReturnOK()
    {
        // Arrange
        string id = "";
        List<string> stationIds = new();
        List<State> stateIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.GetOrderListByStudentId
            (id, stationIds, stateIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<OrderModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StudentController_GetOrderListByStudentId_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        List<string> stationIds = new();
        List<State> stateIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StudentController(studentService, orderService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetOrderListByStudentId
            (id, stationIds, stateIds, state, paging));
    }

    [Fact]
    public void StudentController_GetOrderListByStudentId_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        List<string> stationIds = new();
        List<State> stateIds = new();
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.GetOrderListByStudentId
            (id, stationIds, stateIds, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<OrderModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StudentController_CreateOrder_ReturnCreated()
    {
        // Arrange
        string id = "";
        CreateOrderModel create = new();
        A.CallTo(() => orderService.Add(id, create)).Returns(new());
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.CreateOrder(id, create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status201Created,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StudentController_CreateOrder_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        CreateOrderModel create = new();
        var controller = new StudentController(studentService, orderService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.CreateOrder(id, create));
    }

    [Fact]
    public void StudentController_CreateOrder_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        CreateOrderModel create = new();
        A.CallTo(() => orderService.Add(id, create))
            .Throws(new InvalidParameterException());
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.CreateOrder(id, create);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StudentController_GetOrderById_ReturnOK()
    {
        // Arrange
        string id = "";
        string orderId = "";
        A.CallTo(() => studentService.GetOrderByOrderId(id, orderId))
            .Returns(new());
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.GetOrderById(id, orderId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StudentController_GetOrderById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        string orderId = "";
        A.CallTo(() => studentService.GetOrderByOrderId(id, orderId))
            .Throws(new InvalidParameterException());
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.GetOrderById(id, orderId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StudentController_GetVoucherListByStudentId_ReturnOK()
    {
        // Arrange
        string id = "";
        List<string> campaignIds = new();
        List<string> campaignDetailIds = new();
        List<string> voucherIds = new();
        List<string> brandIds = new();
        List<string> typeIds = new();
        bool? isUsed = null;
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.GetVoucherListByStudentId
            (id, campaignIds, campaignDetailIds, voucherIds, brandIds, typeIds, isUsed, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<VoucherItemModel>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StudentController_GetVoucherListByStudentId_ReturnBadRequest1()
    {
        // Arrange
        string id = "";
        List<string> campaignIds = new();
        List<string> campaignDetailIds = new();
        List<string> voucherIds = new();
        List<string> brandIds = new();
        List<string> typeIds = new();
        bool? isUsed = null;
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StudentController(studentService, orderService);
        controller.ModelState.AddModelError("SessionName", "Required");

        // Act & Assert
        Assert.Throws<InvalidParameterException>(
            () => controller.GetVoucherListByStudentId
            (id, campaignIds, campaignDetailIds, voucherIds, brandIds, typeIds, isUsed, state, paging));
    }

    [Fact]
    public void StudentController_GetVoucherListByStudentId_ReturnBadRequest2()
    {
        // Arrange
        string id = "";
        List<string> campaignIds = new();
        List<string> campaignDetailIds = new();
        List<string> voucherIds = new();
        List<string> brandIds = new();
        List<string> typeIds = new();
        bool? isUsed = null;
        bool? state = null;
        PagingModel paging = new()
        {
            Sort = "Ids,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        };
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.GetVoucherListByStudentId
            (id, campaignIds, campaignDetailIds, voucherIds, brandIds, typeIds, isUsed, state, paging);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<VoucherItemModel>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result?.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StudentController_GetVoucherById_ReturnOK()
    {
        // Arrange
        string id = "";
        string voucherId = "";
        A.CallTo(() => studentService.GetVoucherItemByVoucherId(id, voucherId))
            .Returns(new());
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.GetVoucherById(id, voucherId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status200OK,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StudentController_GetVoucherById_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        string voucherId = "";
        A.CallTo(() => studentService.GetVoucherItemByVoucherId(id, voucherId))
            .Throws(new InvalidParameterException());
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.GetVoucherById(id, voucherId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ObjectResult));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.GetType().GetProperty("StatusCode")?.GetValue(result));
    }

    [Fact]
    public void StudentController_GetWishlistsByStudentId_ReturnOK()
    {
        // Arrange
        string id = "";
        A.CallTo(() => studentService.GetWishlistsByStudentId(id))
            .Returns(new());
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.GetWishlistsByStudentId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<List<string>>));
        Assert.Equal(StatusCodes.Status200OK,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }

    [Fact]
    public void StudentController_GetWishlistsByStudentId_ReturnBadRequest()
    {
        // Arrange
        string id = "";
        A.CallTo(() => studentService.GetWishlistsByStudentId(id))
            .Throws(new InvalidParameterException());
        var controller = new StudentController(studentService, orderService);

        // Act
        var result = controller.GetWishlistsByStudentId(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<List<string>>));
        Assert.Equal(StatusCodes.Status400BadRequest,
            result.Result.GetType().GetProperty("StatusCode")?.GetValue(result.Result));
    }
}
