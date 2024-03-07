using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Controllers;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Admins;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Controllers;

public class AdminControllerTest
{
    private readonly IAdminService adminService;

    private readonly IChartService chartService;

    private readonly IRequestService requestService;

    private readonly IFireBaseService fireBaseService;

    public AdminControllerTest()
    {
        adminService = A.Fake<IAdminService>();
        chartService = A.Fake<IChartService>();
        requestService = A.Fake<IRequestService>();
        fireBaseService = A.Fake<IFireBaseService>();
    }

    [Fact]
    public void AdminController_GetList_ReturnOK()
    {
        // Arrange
        var controller = new AdminController
            (adminService, chartService, requestService, fireBaseService);

        // Act
        var result = controller.GetList(true, new()
        {
            Sort = "Id,desc",
            Search = "",
            Page = 1,
            Limit = 10,
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(ActionResult<PagedResultModel<AdminModel>>));
    }
}
