using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.VoucherTypes;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class VoucherTypeService : IVoucherTypeService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "voucherTypes";

    private readonly IVoucherTypeRepository voucherTypeRepository;

    private readonly IFireBaseService fireBaseService;

    public VoucherTypeService(IVoucherTypeRepository voucherTypeRepository, 
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<VoucherType, VoucherTypeModel>().ReverseMap();
            cfg.CreateMap<PagedResultModel<VoucherType>, PagedResultModel<VoucherTypeModel>>()
            .ReverseMap();
            cfg.CreateMap<VoucherType, UpdateVoucherTypeModel>()
            .ReverseMap()
            .ForMember(t => t.Image, opt => opt.Ignore())
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
            cfg.CreateMap<VoucherType, CreateVoucherTypeModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.Image, opt => opt.Ignore())
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.voucherTypeRepository = voucherTypeRepository;
        this.fireBaseService = fireBaseService;
    }

    public async Task<VoucherTypeModel> Add(CreateVoucherTypeModel creation)
    {
        VoucherType entity = mapper.Map<VoucherType>(creation);

        //Upload image
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            entity.Image = f.URL;
            entity.FileName = f.FileName;
        }
        return mapper.Map<VoucherTypeModel>(voucherTypeRepository.Add(entity));
    }

    public void Delete(string id)
    {
        VoucherType entity = voucherTypeRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Image != null && entity.FileName != null)
            {
                //Remove image
                fireBaseService.RemoveFileAsync(entity.FileName, FOLDER_NAME);
            }
            voucherTypeRepository.Delete(id);
        }
        else
        {
            throw new InvalidParameterException("Not found voucher's type");
        }
    }

    public PagedResultModel<VoucherTypeModel> GetAll(string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<VoucherTypeModel>>(voucherTypeRepository.GetAll(propertySort, isAsc, search, page, limit));
    }

    public VoucherTypeModel GetById(string id)
    {
        VoucherType entity = voucherTypeRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<VoucherTypeModel>(entity);
        }
        throw new InvalidParameterException("Not found voucher's type");
    }

    public async Task<VoucherTypeModel> Update(string id, UpdateVoucherTypeModel update)
    {
        VoucherType entity = voucherTypeRepository.GetById(id);
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
            return mapper.Map<VoucherTypeModel>(voucherTypeRepository.Update(entity));
        }
        throw new InvalidParameterException("Not found voucher's type");
    }
}
