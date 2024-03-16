using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Staffs;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class StaffServiceTest
{
    private readonly IStaffRepository staffRepository;

    private readonly IFireBaseService fireBaseService;

    private readonly IAccountRepository accountRepository;

    public StaffServiceTest()
    {
        staffRepository = A.Fake<IStaffRepository>();
        fireBaseService = A.Fake<IFireBaseService>();
        accountRepository = A.Fake<IAccountRepository>();
    }

    [Fact]
    public void StaffService_Add()
    {
        // Arrange
        string id = "id";
        CreateStaffModel creation = A.Fake<CreateStaffModel>();
        A.CallTo(() => accountRepository.Add(A<Account>.Ignored)).Returns(new());
        A.CallTo(() => staffRepository.Add(A<Staff>.Ignored)).Returns(new()
        {
            Id = id
        });
        var service = new StaffService
            (staffRepository, fireBaseService, accountRepository);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<StaffExtraModel>));
        Assert.Equal(id, result.Result.Id);
    }

    [Fact]
    public void StaffService_Delete()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => staffRepository.GetById(id)).Returns(new()
        {
            Account = new()
            {
                Id = id,
            },
            Station = new()
            {
                Staffs = new List<Staff>()
                {
                    new(),
                    new()
                }
            }
        });
        A.CallTo(() => staffRepository.Delete(id));
        A.CallTo(() => accountRepository.Delete(id));
        var service = new StaffService
            (staffRepository, fireBaseService, accountRepository);

        // Act & Assert
        service.Delete(id);
    }

    [Fact]
    public void StaffService_GetAll()
    {
        // Arrange
        List<string> stationIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<Staff> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => staffRepository.GetAll(stationIds, state, propertySort, 
            isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new StaffService
            (staffRepository, fireBaseService, accountRepository);

        // Act
        var result = service.GetAll
            (stationIds, state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<StaffModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void StaffService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => staffRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        var service = new StaffService
            (staffRepository, fireBaseService, accountRepository);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StaffExtraModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void StaffService_Update()
    {
        // Arrange
        string id = "id";
        string fullName = "fullName";
        UpdateStaffModel update = A.Fake<UpdateStaffModel>();
        A.CallTo(() => staffRepository.GetById(id));
        A.CallTo(() => staffRepository.Update(A<Staff>.Ignored))
        .Returns(new()
        {
                Id = id,
                FullName = fullName
            });
        var service = new StaffService
            (staffRepository, fireBaseService, accountRepository);

        // Act
        var result = service.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<StaffExtraModel>));
        Assert.Equal(id, result.Result.Id);
        Assert.Equal(fullName, result.Result.FullName);
    }
}
