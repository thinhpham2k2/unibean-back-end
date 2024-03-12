using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Admins;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class AdminServiceTest
{
    private readonly IAdminRepository adminRepository;

    private readonly IFireBaseService fireBaseService;

    private readonly IAccountRepository accountRepository;

    public AdminServiceTest()
    {
        adminRepository = A.Fake<IAdminRepository>();
        fireBaseService = A.Fake<IFireBaseService>();
        accountRepository = A.Fake<IAccountRepository>();
    }

    [Fact]
    public void AdminService_Add()
    {
        // Arrange
        string id = "id";
        CreateAdminModel creation = A.Fake<CreateAdminModel>();
        A.CallTo(() => accountRepository.Add(A<Account>.Ignored)).Returns(new());
        A.CallTo(() => adminRepository.Add(A<Admin>.Ignored)).Returns(new()
        {
            Id = id
        });
        var service = new AdminService
            (adminRepository, fireBaseService, accountRepository);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<AdminExtraModel>));
        Assert.Equal(id, result.Result.Id);
    }

    [Fact]
    public void AdminService_Delete()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => adminRepository.GetById(id)).Returns(new()
        {
            Account = new()
            {
                Id = id,
            },
            Requests = new List<Request>(),
        });
        A.CallTo(() => adminRepository.Delete(id));
        A.CallTo(() => accountRepository.Delete(id));
        var service = new AdminService
            (adminRepository, fireBaseService, accountRepository);

        // Act & Assert
        service.Delete(id);
    }

    [Fact]
    public void AdminService_GetAll()
    {
        // Arrange
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<Admin> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => adminRepository.GetAll(state, propertySort, isAsc, search, page, limit))
            .Returns(pagedResultModel);
        var service = new AdminService
            (adminRepository, fireBaseService, accountRepository);

        // Act
        var result = service.GetAll(state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<AdminModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void AdminService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => adminRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        var service = new AdminService
            (adminRepository, fireBaseService, accountRepository);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(AdminExtraModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void AdminService_Update()
    {
        // Arrange
        string id = "id";
        string fullName = "fullName";
        UpdateAdminModel update = new();
        A.CallTo(() => adminRepository.GetById(id));
        A.CallTo(() => adminRepository.Update(A<Admin>.Ignored))
            .Returns(new()
            {
                Id = id,
                FullName = fullName
            });
        var service = new AdminService
            (adminRepository, fireBaseService, accountRepository);

        // Act
        var result = service.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<AdminExtraModel>));
        Assert.Equal(id, result.Result.Id);
        Assert.Equal(fullName, result.Result.FullName);
    }
}
