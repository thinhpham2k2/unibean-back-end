using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Authens;
using Unibean.Service.Models.Files;
using Unibean.Service.Models.VoucherItems;
using Unibean.Service.Services;

namespace Unibean.Test.Services;

public class VoucherItemServiceTest
{
    private readonly IVoucherRepository voucherRepository;

    private readonly IVoucherItemRepository voucherItemRepository;

    public VoucherItemServiceTest()
    {
        voucherRepository = A.Fake<IVoucherRepository>();
        voucherItemRepository = A.Fake<IVoucherItemRepository>();
    }

    [Fact]
    public void VoucherItemService_Add()
    {
        // Arrange
        string id = "id";
        CreateVoucherItemModel creation = new()
        {
            VoucherId = id,
        };
        A.CallTo(() => voucherItemRepository.GetMaxIndex(id)).Returns(1);
        A.CallTo(() => voucherItemRepository.Add(A<VoucherItem>.Ignored)).Returns(new()
        {
            Id = id
        });
        var service = new VoucherItemService(voucherRepository, voucherItemRepository);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(MemoryStream));
    }

    [Fact]
    public void VoucherItemService_Delete()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => voucherItemRepository.GetById(id)).Returns(new()
        {
            Id = id,
            IsLocked = false,
            IsBought = false,
            IsUsed = false,
        });
        A.CallTo(() => voucherItemRepository.Delete(id));
        var service = new VoucherItemService(voucherRepository, voucherItemRepository);

        // Act & Assert
        service.Delete(id);
    }

    [Fact]
    public void VoucherItemService_GetAll()
    {
        // Arrange
        List<string> campaignIds = new();
        List<string> campaignDetailIds = new();
        List<string> voucherIds = new();
        List<string> brandIds = new();
        List<string> typeIds = new();
        List<string> studentIds = new();
        bool? isLocked = null;
        bool? isBought = null;
        bool? isUsed = null;
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<VoucherItem> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => voucherItemRepository.GetAll(campaignIds, campaignDetailIds,
            voucherIds, brandIds, typeIds, studentIds, isLocked, isBought, isUsed,
            state, propertySort, isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new VoucherItemService(voucherRepository, voucherItemRepository);

        // Act
        var result = service.GetAll(campaignIds, campaignDetailIds, voucherIds,
            brandIds, typeIds, studentIds, isLocked, isBought, isUsed, state,
            propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<VoucherItemModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void VoucherItemService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => voucherItemRepository.GetById(id))
        .Returns(new()
        {
            Id = id
        });
        var service = new VoucherItemService(voucherRepository, voucherItemRepository);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(VoucherItemExtraModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void VoucherItemService_GetTemplateVoucherItem()
    {
        // Arrange
        var service = new VoucherItemService(voucherRepository, voucherItemRepository);

        // Act
        var result = service.GetTemplateVoucherItem();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(MemoryStream));
    }

    [Fact]
    public void VoucherItemService_AddTemplate()
    {
        // Arrange
        JwtRequestModel request = new()
        {
            Role = "Brand"
        };
        InsertVoucherItemModel insert = A.Fake<InsertVoucherItemModel>();
        var service = new VoucherItemService(voucherRepository, voucherItemRepository);

        // Act
        var result = service.AddTemplate(insert, request);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<MemoryStreamModel>));
    }

    [Fact]
    public void VoucherItemService_GetByCode()
    {
        // Arrange
        string code = "code";
        string brandId = "brandId";
        A.CallTo(() => voucherItemRepository.GetByVoucherCode(code, brandId))
        .Returns(new()
        {
            VoucherCode = code
        });
        var service = new VoucherItemService(voucherRepository, voucherItemRepository);

        // Act
        var result = service.GetByCode(code, brandId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(VoucherItemExtraModel));
        Assert.Equal(code, result.VoucherCode);
    }

    [Fact]
    public void VoucherItemService_EntityToExtra()
    {
        // Arrange
        VoucherItem item = A.Fake<VoucherItem>();
        var service = new VoucherItemService(voucherRepository, voucherItemRepository);

        // Act
        var result = service.EntityToExtra(item);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(VoucherItemExtraModel));
    }
}
