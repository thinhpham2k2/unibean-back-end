using AutoMapper;
using Enable.EnumDisplayName;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
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

    public StationService(
        IStationRepository stationRepository,
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Station, StationModel>()
            .ForMember(s => s.StateId, opt => opt.MapFrom(src => (int)src.State))
            .ForMember(s => s.State, opt => opt.MapFrom(src => src.State))
            .ForMember(s => s.StateName, opt => opt.MapFrom(src => src.State.GetDisplayName()))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Station>, PagedResultModel<StationModel>>()
            .ReverseMap();
            cfg.CreateMap<Station, StationExtraModel>()
            .ForMember(s => s.StateId, opt => opt.MapFrom(src => (int)src.State))
            .ForMember(s => s.State, opt => opt.MapFrom(src => src.State))
            .ForMember(s => s.StateName, opt => opt.MapFrom(src => src.State.GetDisplayName()))
            .ForMember(t => t.NumberOfOrder, opt
                => opt.MapFrom(src => src.Orders.Where(o
                => o.OrderStates.LastOrDefault().State.Equals(State.Order)).Count()))
            .ForMember(t => t.NumberOfAccept, opt
                => opt.MapFrom(src => src.Orders.Where(o
                => o.OrderStates.LastOrDefault().State.Equals(State.Confirmation)).Count()))
            .ForMember(t => t.NumberOfPrepare, opt
                => opt.MapFrom(src => src.Orders.Where(o
                => o.OrderStates.LastOrDefault().State.Equals(State.Preparation)).Count()))
            .ForMember(t => t.NumberOfDelivery, opt
                => opt.MapFrom(src => src.Orders.Where(o
                => o.OrderStates.LastOrDefault().State.Equals(State.Arrival)).Count()))
            .ForMember(t => t.NumberOfDone, opt
                => opt.MapFrom(src => src.Orders.Where(o
                => o.OrderStates.LastOrDefault().State.Equals(State.Receipt)).Count()))
            .ForMember(t => t.NumberOfAbort, opt
                => opt.MapFrom(src => src.Orders.Where(o
                => o.OrderStates.LastOrDefault().State.Equals(State.Abort)).Count()))
            .ForMember(t => t.NumberOfStaffs, opt => opt.MapFrom(src => src.Staffs.Count))
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
            if (entity.Orders.All(
                o => new[] { State.Receipt, State.Abort }.Contains(o.OrderStates.LastOrDefault().State.Value)))
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
                throw new InvalidParameterException
                    ("Xóa thất bại do đang có đơn hàng ở trạm hoặc tồn tại nhân viên thuộc trạm");
            }
        }
        else
        {
            throw new InvalidParameterException("Không tìm thấy trạm");
        }
    }

    public PagedResultModel<StationModel> GetAll
        (List<StationState> stateIds, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<StationModel>>(stationRepository.GetAll
            (stateIds, propertySort, isAsc, search, page, limit));
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

    public bool UpdateState(string id, StationState stateId)
    {
        Station entity = stationRepository.GetById(id);
        if (entity != null)
        {
            if (!entity.State.Equals(stateId))
            {
                if (stateId.Equals(StationState.Closed) && !entity.Orders.All(
                    o => new[] { State.Receipt, State.Abort }.Contains(o.OrderStates.LastOrDefault().State.Value)))
                {
                    throw new InvalidParameterException("Đóng trạm thất bại do đang có đơn hàng ở trạm");
                }

                entity.State = stateId;
                return stationRepository.Update(entity) != null;
            }
            else
            {
                throw new InvalidParameterException("Trạm đang ở trạng thái '" + stateId.GetDisplayName() + "'");
            }
        }
        throw new InvalidParameterException("Không tìm thấy trạm");
    }
}
