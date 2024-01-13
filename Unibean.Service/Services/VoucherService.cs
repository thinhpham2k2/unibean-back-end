using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.Vouchers;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class VoucherService : IVoucherService
{
    private readonly Mapper mapper;

    private readonly IVoucherRepository voucherRepository;

    public VoucherService(IVoucherRepository voucherRepository)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Voucher, VoucherModel>()
            .ForMember(c => c.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
            .ForMember(c => c.TypeName, opt => opt.MapFrom(src => src.Type.TypeName))
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.voucherRepository = voucherRepository;
    }

    public PagedResultModel<VoucherModel> GetAll
        (List<string> brandIds, List<string> typeIds, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<VoucherModel>>
            (voucherRepository.GetAll(brandIds, typeIds, propertySort, isAsc, search, page, limit));
    }
}
