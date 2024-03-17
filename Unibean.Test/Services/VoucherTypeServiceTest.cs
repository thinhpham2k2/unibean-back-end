using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.VoucherTypes;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class VoucherTypeServiceTest
{
    private readonly IVoucherTypeRepository voucherTypeRepository;

    private readonly IFireBaseService fireBaseService;

    public VoucherTypeServiceTest()
    {
        voucherTypeRepository = A.Fake<IVoucherTypeRepository>();
        fireBaseService = A.Fake<IFireBaseService>();
    }

    [Fact]
    public void VoucherTypeService_Add()
    {
        // Arrange
        string id = "id";
        CreateVoucherTypeModel creation = A.Fake<CreateVoucherTypeModel>();
        A.CallTo(() => voucherTypeRepository.Add(A<VoucherType>.Ignored)).Returns(new()
        {
            Id = id
        });
        var service = new VoucherTypeService(voucherTypeRepository, fireBaseService);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<VoucherTypeExtraModel>));
        Assert.Equal(id, result.Result.Id);
    }

    [Fact]
    public void VoucherTypeService_Delete()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => voucherTypeRepository.GetById(id)).Returns(new()
        {
            Id = id,
            Vouchers = new List<Voucher>(),
        });
        A.CallTo(() => voucherTypeRepository.Delete(id));
        var service = new VoucherTypeService(voucherTypeRepository, fireBaseService);

        // Act & Assert
        service.Delete(id);
    }

    [Fact]
    public void VoucherTypeService_GetAll()
    {
        // Arrange
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<VoucherType> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => voucherTypeRepository.GetAll(state, propertySort, isAsc, search, page, limit))
            .Returns(pagedResultModel);
        var service = new VoucherTypeService(voucherTypeRepository, fireBaseService);

        // Act
        var result = service.GetAll(state, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<VoucherTypeModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void VoucherTypeService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => voucherTypeRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        var service = new VoucherTypeService(voucherTypeRepository, fireBaseService);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(VoucherTypeExtraModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void VoucherTypeService_Update()
    {
        // Arrange
        string id = "id";
        string typeName = "typeName";
        UpdateVoucherTypeModel update = A.Fake<UpdateVoucherTypeModel>();
        A.CallTo(() => voucherTypeRepository.GetById(id));
        A.CallTo(() => voucherTypeRepository.Update(A<VoucherType>.Ignored))
        .Returns(new()
        {
            Id = id,
            TypeName = typeName
        });
        var service = new VoucherTypeService(voucherTypeRepository, fireBaseService);

        // Act
        var result = service.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<VoucherTypeExtraModel>));
        Assert.Equal(id, result.Result.Id);
        Assert.Equal(typeName, result.Result.TypeName);
    }
}
