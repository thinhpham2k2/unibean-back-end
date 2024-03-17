using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Stations;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Test.Services;

public class StationServiceTest
{
    private readonly IStationRepository stationRepository;

    private readonly IFireBaseService fireBaseService;

    public StationServiceTest()
    {
        stationRepository = A.Fake<IStationRepository>();
        fireBaseService = A.Fake<IFireBaseService>();
    }

    [Fact]
    public void StationService_Add()
    {
        // Arrange
        string id = "id";
        CreateStationModel creation = A.Fake<CreateStationModel>();
        A.CallTo(() => stationRepository.Add(A<Station>.Ignored)).Returns(new()
        {
            Id = id,
            State = StationState.Active,
        });
        var service = new StationService(stationRepository, fireBaseService);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<StationExtraModel>));
        Assert.Equal(id, result.Result.Id);
    }

    [Fact]
    public void StationService_Delete()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => stationRepository.GetById(id)).Returns(new()
        {
            Id = id,
            Orders = new List<Order>()
            {
                new()
                {
                    OrderStates = new List<OrderState>()
                    {
                        new()
                        {
                            State = State.Receipt,
                        }
                    }
                },
                new()
                {
                    OrderStates = new List<OrderState>()
                    {
                        new()
                        {
                            State = State.Abort,
                        }
                    }
                }
            }
        });
        A.CallTo(() => stationRepository.Delete(id));
        var service = new StationService(stationRepository, fireBaseService);

        // Act & Assert
        service.Delete(id);
    }

    [Fact]
    public void StationService_GetAll()
    {
        // Arrange
        List<StationState> stateIds = new();
        string propertySort = "";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        PagedResultModel<Station> pagedResultModel = new()
        {
            Result = new()
            {
                new()
                {
                    State = StationState.Active,
                },
                new()
                {
                    State = StationState.Inactive,
                },
                new()
                {
                    State = StationState.Closed,
                }
            }
        };
        A.CallTo(() => stationRepository.GetAll(stateIds, propertySort,
            isAsc, search, page, limit)).Returns(pagedResultModel);
        var service = new StationService(stationRepository, fireBaseService);

        // Act
        var result = service.GetAll(stateIds, propertySort, isAsc,
            search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<StationModel>));
        Assert.Equal(pagedResultModel.Result.Count, result.Result.Count);
    }

    [Fact]
    public void StationService_GetById()
    {
        // Arrange
        string id = "id";
        A.CallTo(() => stationRepository.GetById(id))
            .Returns(new()
            {
                Id = id,
                State = StationState.Active,
            });
        var service = new StationService(stationRepository, fireBaseService);

        // Act
        var result = service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(StationExtraModel));
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void StationService_Update()
    {
        // Arrange
        string id = "id";
        string stationName = "stationName";
        UpdateStationModel update = A.Fake<UpdateStationModel>();
        A.CallTo(() => stationRepository.GetById(id));
        A.CallTo(() => stationRepository.Update(A<Station>.Ignored))
            .Returns(new()
            {
                Id = id,
                StationName = stationName,
                State = StationState.Active,
            });
        var service = new StationService(stationRepository, fireBaseService);

        // Act
        var result = service.Update(id, update);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<StationExtraModel>));
        Assert.Equal(id, result.Result.Id);
        Assert.Equal(stationName, result.Result.StationName);
    }

    [Fact]
    public void StationService_UpdateState()
    {
        // Arrange
        string id = "id";
        StationState stateId = StationState.Inactive;
        A.CallTo(() => stationRepository.GetById(id));
        A.CallTo(() => stationRepository.Update(A<Station>.Ignored))
            .Returns(new()
            {
                Id = id,
                State = StationState.Active,
            });
        var service = new StationService(stationRepository, fireBaseService);

        // Act & Assert
        Assert.True(service.UpdateState(id, stateId));
    }
}
