using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Requests;
using Unibean.Service.Models.RequestTransactions;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class RequestService : IRequestService
{
    private readonly Mapper mapper;

    private readonly IRequestRepository requestRepository;

    public RequestService(IRequestRepository requestRepository)
    {
        var config = new MapperConfiguration(cfg
               =>
        {
            cfg.CreateMap<Request, RequestModel>()
            .ForMember(r => r.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
            .ForMember(r => r.AdminName, opt => opt.MapFrom(src => src.Admin.FullName))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Request>, PagedResultModel<RequestModel>>()
            .ReverseMap();
            cfg.CreateMap<Request, RequestExtraModel>()
            .ForMember(r => r.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
            .ForMember(r => r.BrandAcronym, opt => opt.MapFrom(src => src.Brand.Acronym))
            .ForMember(r => r.BrandLogo, opt => opt.MapFrom(src => src.Brand.Account.Avatar))
            .ForMember(r => r.AdminName, opt => opt.MapFrom(src => src.Admin.FullName))
            .ForMember(r => r.AdminAvatar, opt => opt.MapFrom(src => src.Admin.Account.Avatar))
            .ReverseMap();
            cfg.CreateMap<RequestTransaction, RequestTransactionModel>()
            .ForMember(r => r.WalletType, opt => opt.MapFrom(src => src.Wallet.Type.TypeName))
            .ForMember(r => r.WalletImage, opt => opt.MapFrom(src => src.Wallet.Type.Image))
            .ReverseMap();
            cfg.CreateMap<Request, CreateRequestModel>()
            .ReverseMap()
            .ForMember(r => r.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(r => r.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(r => r.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(r => r.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.requestRepository = requestRepository;
    }

    public RequestExtraModel Add(string id, CreateRequestModel creation)
    {
        Request request = mapper.Map<Request>(creation);
        request.AdminId = id;
        return mapper.Map<RequestExtraModel>(requestRepository.Add(request));
    }

    public PagedResultModel<RequestModel> GetAll
        (List<string> brandIds, List<string> adminIds, bool? state, 
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<RequestModel>>(requestRepository
            .GetAll(brandIds, adminIds, state, propertySort, isAsc, search, page, limit));
    }

    public RequestExtraModel GetById(string id)
    {
        Request entity = requestRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<RequestExtraModel>(entity);
        }
        throw new InvalidParameterException("Not found request");
    }
}
