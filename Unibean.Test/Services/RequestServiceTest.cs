using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Requests;
using Unibean.Service.Services;

namespace Unibean.Test.Services;

public class RequestServiceTest
{
    private readonly IRequestRepository requestRepository;

    public RequestServiceTest()
    {
        requestRepository = A.Fake<IRequestRepository>();
    }

    [Fact]
    public void RequestService_Add()
    {
        // Arrange
        string id = "id";
        CreateRequestModel creation = A.Fake<CreateRequestModel>();
        A.CallTo(() => requestRepository.Add(A<Request>.Ignored)).Returns(new()
        {
            Id = id
        });
        var service = new RequestService(requestRepository);

        // Act
        var result = service.Add(id, creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(RequestExtraModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void RequestService_GetAll()
    {
        // Arrange
        List<string> brandIds = new();
        List<string> adminIds = new();
        bool? state = null;
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<Request> pagedResultModel = new()
        {
            Result = new()
            {
                new(),
                new(),
                new()
            }
        };
        A.CallTo(() => requestRepository.GetAll(brandIds, adminIds, state, 
            propertySort, isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new RequestService(requestRepository);

        // Act
        var result = service.GetAll(brandIds, adminIds, state, propertySort,
            isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<RequestModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void RequestService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => requestRepository.GetById(id))
            .Returns(new()
            {
                Id = id
            });
        var service = new RequestService(requestRepository);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(RequestExtraModel));
        Assert.Equal(id, result.Id);
    }
}
