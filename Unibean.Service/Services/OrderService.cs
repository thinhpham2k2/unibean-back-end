﻿using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.OrderDetails;
using Unibean.Service.Models.Orders;
using Unibean.Service.Models.OrderStates;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class OrderService : IOrderService
{
    private readonly Mapper mapper;

    private readonly IOrderRepository orderRepository;

    public OrderService(
        IOrderRepository orderRepository)
    {
        var config = new MapperConfiguration(cfg
               =>
        {
            cfg.CreateMap<Order, OrderModel>()
            .ForMember(o => o.StudentName, opt => opt.MapFrom(src => src.Student.FullName))
            .ForMember(o => o.StationName, opt => opt.MapFrom(src => src.Station.StationName))
            .ForMember(o => o.StateCurrent, opt => opt.MapFrom(src => src.OrderStates.Select(s 
                => s.State).OrderByDescending(s => s.Id).FirstOrDefault().StateName))
            .ForMember(o => o.StateDetails, opt => opt.MapFrom(src => src.OrderStates))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Order>, PagedResultModel<OrderModel>>()
            .ReverseMap();
            cfg.CreateMap<OrderDetail, OrderDetailModel>()
            .ForMember(d => d.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
            .ForMember(d => d.ProductImage, opt => opt.MapFrom(src => src.Product.Images.Where(i 
                => i.IsCover.Equals(true)).FirstOrDefault().Url))
            .ReverseMap();
            cfg.CreateMap<OrderState, OrderStateModel>()
            .ForMember(s => s.StateName, opt => opt.MapFrom(src => src.State.StateName))
            .ForMember(s => s.StateImage, opt => opt.MapFrom(src => src.State.Image))
            .ReverseMap();
        });
       mapper = new Mapper(config);
        this.orderRepository = orderRepository;
    }

    public PagedResultModel<OrderModel> GetAll
        (List<string> stationIds, List<string> studentIds, List<string> stateIds, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<OrderModel>>(orderRepository.GetAll
            (stationIds, studentIds, stateIds, propertySort, isAsc, search, page, limit));
    }
}
