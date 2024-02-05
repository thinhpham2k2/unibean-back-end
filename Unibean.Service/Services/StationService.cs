using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Orders;
using Unibean.Service.Models.Stations;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class StationService : IStationService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "stations";

    private readonly IStationRepository stationRepository;

    private readonly IFireBaseService fireBaseService;

    private readonly IOrderService orderService;

    public StationService(
        IStationRepository stationRepository, 
        IFireBaseService fireBaseService,
        IOrderService orderService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Station, StationModel>()
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Station>, PagedResultModel<StationModel>>()
            .ReverseMap();
            cfg.CreateMap<Station, StationExtraModel>()
            .ForMember(t => t.NumberOfOrder, opt 
                => opt.MapFrom(src => src.Orders.Where(o 
                => o.OrderStates.DistinctBy(s => s.State).Count().Equals(1)).Count()))
            .ForMember(t => t.NumberOfAccept, opt 
                => opt.MapFrom(src => src.Orders.Where(o 
                => o.OrderStates.DistinctBy(s => s.State).Count().Equals(2)).Count()))
            .ForMember(t => t.NumberOfPrepare, opt 
                => opt.MapFrom(src => src.Orders.Where(o 
                => o.OrderStates.DistinctBy(s => s.State).Count().Equals(3)).Count()))
            .ForMember(t => t.NumberOfDelivery, opt 
                => opt.MapFrom(src => src.Orders.Where(o 
                => o.OrderStates.DistinctBy(s => s.State).Count().Equals(4)).Count()))
            .ForMember(t => t.NumberOfDone, opt 
                => opt.MapFrom(src => src.Orders.Where(o 
                => o.OrderStates.DistinctBy(s => s.State).Count().Equals(5)).Count()))
            .ReverseMap();
            cfg.CreateMap<Station, UpdateStationModel>()
            .ReverseMap()
            .ForMember(t => t.Image, opt => opt.Ignore())
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
            cfg.CreateMap<Station, CreateStationModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.stationRepository = stationRepository;
        this.fireBaseService = fireBaseService;
        this.orderService = orderService;
    }

    public async Task<StationExtraModel> Add(CreateStationModel creation)
    {
        Station entity = mapper.Map<Station>(creation);

        //Upload image
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            entity.Image = f.URL;
            entity.FileName = f.FileName;
        }
        return mapper.Map<StationExtraModel>(stationRepository.Add(entity));
    }

    public void Delete(string id)
    {
        Station entity = stationRepository.GetById(id);
        if (entity != null)
        {
            entity.Orders = entity.Orders.Where(
                o => o.OrderStates.DistinctBy(s => s.State).Count() < 5).ToList();
            if (entity.Orders.Count.Equals(0))
            {
                if (entity.Image != null && entity.FileName != null)
                {
                    //Remove image
                    fireBaseService.RemoveFileAsync(entity.FileName, FOLDER_NAME);
                }
                stationRepository.Delete(id);
            }
            else
            {
                throw new InvalidParameterException("Đang có đơn hàng ở trạm");
            }
        }
        else
        {
            throw new InvalidParameterException("Không tìm thấy trạm");
        }
    }

    public PagedResultModel<StationModel> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<StationModel>>(stationRepository.GetAll
            (state, propertySort, isAsc, search, page, limit));
    }

    public StationExtraModel GetById(string id)
    {
        Station entity = stationRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<StationExtraModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy trạm");
    }

    public PagedResultModel<OrderModel> GetOrderListByStudentId
        (string id, List<string> studentIds, List<State> stateIds, bool? state, 
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        Station entity = stationRepository.GetById(id);
        if (entity != null)
        {
            return orderService.GetAll
                (new() { id }, studentIds, stateIds, state, 
                propertySort, isAsc, search, page, limit);
        }
        throw new InvalidParameterException("Không tìm thấy trạm");
    }

    public async Task<StationExtraModel> Update(string id, UpdateStationModel update)
    {
        Station entity = stationRepository.GetById(id);
        if (entity != null)
        {
            entity = mapper.Map(update, entity);
            if (update.Image != null && update.Image.Length > 0)
            {
                // Remove image
                await fireBaseService.RemoveFileAsync(entity.FileName, FOLDER_NAME);

                //Upload new image update
                FireBaseFile f = await fireBaseService.UploadFileAsync(update.Image, FOLDER_NAME);
                entity.Image = f.URL;
                entity.FileName = f.FileName;
            }
            return mapper.Map<StationExtraModel>(stationRepository.Update(entity));
        }
        throw new InvalidParameterException("Không tìm thấy trạm");
    }
}
