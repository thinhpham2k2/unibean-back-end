using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Vouchers;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class VoucherServiceTest
{
    private readonly IVoucherRepository voucherRepository;

    private readonly IFireBaseService fireBaseService;

    public VoucherServiceTest()
    {
        voucherRepository = A.Fake<IVoucherRepository>();
        fireBaseService = A.Fake<IFireBaseService>();
    }

    [Fact]
    public void VoucherService_Add()
    {
        // Arrange
        string id = "id";
        CreateVoucherModel creation = A.Fake<CreateVoucherModel>();
        A.CallTo(() => voucherRepository.Add(A<Voucher>.Ignored)).Returns(new()
        {
            Id = id
        });
        var service = new VoucherService(voucherRepository, fireBaseService);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<VoucherExtraModel>));
        Assert.Equal(id, result.Result.Id);
    }

    [Fact]
    public void VoucherService_Delete()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => voucherRepository.GetById(id)).Returns(new()
        {
            Id = id,
            VoucherItems = new List<VoucherItem>(),
        });
        A.CallTo(() => voucherRepository.Delete(id));
        var service = new VoucherService(voucherRepository, fireBaseService);

        // Act & Assert
        service.Delete(id);
    }

    [Fact]
    public void VoucherService_GetAll()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> typeIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<Voucher> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => voucherRepository.GetAll(brandIds, typeIds, state,
            propertySort, isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new VoucherService(voucherRepository, fireBaseService);

        // Act
        var result = service.GetAll(brandIds, typeIds, state, propertySort, isAsc,
            search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<VoucherModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void VoucherService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => voucherRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        var service = new VoucherService(voucherRepository, fireBaseService);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(VoucherExtraModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void VoucherService_Update()
    {
        // Arrange
        string id = "id";
        string voucherName = "voucherName";
        UpdateVoucherModel update = A.Fake<UpdateVoucherModel>();
        A.CallTo(() => voucherRepository.GetById(id));
        A.CallTo(() => voucherRepository.Update(A<Voucher>.Ignored))
        .Returns(new()
        {
            Id = id,
            VoucherName = voucherName
        });
        var service = new VoucherService(voucherRepository, fireBaseService);

        // Act
        var result = service.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<VoucherExtraModel>));
        Assert.Equal(id, result.Result.Id);
        Assert.Equal(voucherName, result.Result.VoucherName);
    }
}
