using AutoMapper;
using Enable.EnumDisplayName;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.OrderDetails;
using Unibean.Service.Models.Orders;
using Unibean.Service.Models.OrderStates;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class OrderService : IOrderService
{
    private readonly Mapper mapper;

    private readonly IEmailService emailService;

    private readonly IOrderRepository orderRepository;

    private readonly IStudentRepository studentRepository;

    private readonly IOrderTransactionRepository orderTransactionRepository;

    public OrderService(
        IEmailService emailService,
        IOrderRepository orderRepository,
        IStudentRepository studentRepository,
        IOrderTransactionRepository orderTransactionRepository)
    {
        var config = new MapperConfiguration(cfg
               =>
        {
            cfg.CreateMap<Order, OrderModel>()
            .ForMember(o => o.OrderImage, opt => opt.MapFrom(src
                => src.OrderDetails.FirstOrDefault().Product.Images.Where(i
                => (bool)i.IsCover).FirstOrDefault().Url))
            .ForMember(o => o.StudentName, opt => opt.MapFrom(src => src.Student.FullName))
            .ForMember(o => o.StudentCode, opt => opt.MapFrom(src => src.Student.Code))
            .ForMember(o => o.StationName, opt => opt.MapFrom(src => src.Station.StationName))
            .ForMember(o => o.CurrentStateId, opt => opt.MapFrom(
                src => (int)src.OrderStates.LastOrDefault().State))
            .ForMember(o => o.CurrentState, opt => opt.MapFrom(
                src => src.OrderStates.LastOrDefault().State))
            .ForMember(o => o.CurrentStateName, opt => opt.MapFrom(
                src => src.OrderStates.LastOrDefault().State.GetDisplayName()))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Order>, PagedResultModel<OrderModel>>()
            .ReverseMap();
            cfg.CreateMap<Order, OrderExtraModel>()
            .ForMember(o => o.StudentName, opt => opt.MapFrom(src => src.Student.FullName))
            .ForMember(o => o.StudentCode, opt => opt.MapFrom(src => src.Student.Code))
            .ForMember(o => o.StudentImage, opt => opt.MapFrom(src => src.Student.Account.Avatar))
            .ForMember(o => o.StationName, opt => opt.MapFrom(src => src.Station.StationName))
            .ForMember(o => o.StationImage, opt => opt.MapFrom(src => src.Station.Image))
            .ForMember(o => o.StationAdress, opt => opt.MapFrom(src => src.Station.Address))
            .ForMember(o => o.StationPhone, opt => opt.MapFrom(src => src.Station.Phone))
            .ForMember(o => o.StationOpeningHours, opt => opt.MapFrom(src => src.Station.OpeningHours))
            .ForMember(o => o.StationClosingHours, opt => opt.MapFrom(src => src.Station.ClosingHours))
            .ForMember(o => o.CurrentStateId, opt => opt.MapFrom(
                src => (int)src.OrderStates.LastOrDefault().State))
            .ForMember(o => o.CurrentState, opt => opt.MapFrom(
                src => src.OrderStates.LastOrDefault().State))
            .ForMember(o => o.CurrentStateName, opt => opt.MapFrom(
                src => src.OrderStates.LastOrDefault().State.GetDisplayName()))
            .ForMember(o => o.StateDetails, opt => opt.MapFrom(src => src.OrderStates))
            .ReverseMap();
            cfg.CreateMap<OrderDetail, OrderDetailModel>()
            .ForMember(d => d.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
            .ForMember(d => d.ProductImage, opt => opt.MapFrom(src => src.Product.Images.Where(i
                => i.IsCover.Equals(true)).FirstOrDefault().Url))
            .ReverseMap();
            cfg.CreateMap<OrderState, OrderStateModel>()
            .ForMember(s => s.StateId, opt => opt.MapFrom(src => (int)src.State))
            .ForMember(s => s.State, opt => opt.MapFrom(src => src.State))
            .ForMember(s => s.StateName, opt => opt.MapFrom(src => src.State.GetDisplayName()))
            .ReverseMap();
            // Map Create Order Model
            cfg.CreateMap<Order, CreateOrderModel>()
            .ReverseMap()
            .ForMember(o => o.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(o => o.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(o => o.Status, opt => opt.MapFrom(src => true));
            // Map Create Order Details Model
            cfg.CreateMap<OrderDetail, CreateDetailModel>()
            .ReverseMap()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(o => o.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.emailService = emailService;
        this.orderRepository = orderRepository;
        this.studentRepository = studentRepository;
        this.orderTransactionRepository = orderTransactionRepository;
    }

    public OrderModel Add(string id, CreateOrderModel creation)
    {
        Student student = studentRepository.GetById(id);
        if (student != null)
        {
            Wallet wallet = student.Wallets.Where(w => w.Type.Equals(WalletType.Red)).FirstOrDefault();
            if (wallet != null)
            {
                if (wallet.Balance >= creation.Amount)
                {
                    Order order = mapper.Map<Order>(creation);
                    order.StudentId = student.Id;
                    order = orderRepository.Add(order);
                    if (order != null)
                    {
                        orderTransactionRepository.Add(new OrderTransaction
                        {
                            Id = Ulid.NewUlid().ToString(),
                            OrderId = order.Id,
                            WalletId = wallet.Id,
                            Amount = -creation.Amount,
                            Rate = 1,
                            Description = creation.Description,
                            State = true,
                            Status = true
                        });

                        // Send mail
                        emailService.SendEmailCreateOrder(student.Account.Email, student.Code,
                            student.FullName, orderRepository.GetById(order.Id));

                        return mapper.Map<OrderModel>(order);
                    }
                    throw new InvalidParameterException("Tạo thất bại");
                }
                throw new InvalidParameterException("Số dư ví đậu đỏ là không đủ");
            }
            throw new InvalidParameterException("Ví đậu đỏ không hợp lệ");
        }
        throw new InvalidParameterException("Sinh viên không hợp lệ");
    }

    public PagedResultModel<OrderModel> GetAll
        (List<string> stationIds, List<string> studentIds, List<State> stateIds,
        bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<OrderModel>>(orderRepository.GetAll
            (stationIds, studentIds, stateIds, state, propertySort, isAsc, search, page, limit));
    }

    public OrderExtraModel GetById(string id)
    {
        Order entity = orderRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<OrderExtraModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy đơn hàng");
    }
}
