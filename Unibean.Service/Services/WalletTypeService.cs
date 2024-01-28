using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.WalletTypes;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class WalletTypeService : IWalletTypeService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "walletTypes";

    private readonly IWalletTypeRepository walletTypeRepository;

    private readonly IFireBaseService fireBaseService;

    public WalletTypeService(IWalletTypeRepository walletTypeRepository, 
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<WalletType, WalletTypeModel>().ReverseMap();
            cfg.CreateMap<PagedResultModel<WalletType>, PagedResultModel<WalletTypeModel>>()
            .ReverseMap();
            cfg.CreateMap<WalletType, UpdateWalletTypeModel>()
            .ReverseMap()
            .ForMember(t => t.Image, opt => opt.Ignore())
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
            cfg.CreateMap<WalletType, CreateWalletTypeModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.walletTypeRepository = walletTypeRepository;
        this.fireBaseService = fireBaseService;
    }

    public async Task<WalletTypeModel> Add(CreateWalletTypeModel creation)
    {
        WalletType entity = mapper.Map<WalletType>(creation);

        //Upload image
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            entity.Image = f.URL;
            entity.FileName = f.FileName;
        }
        return mapper.Map<WalletTypeModel>(walletTypeRepository.Add(entity));
    }

    public void Delete(string id)
    {
        WalletType entity = walletTypeRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Image != null && entity.FileName != null)
            {
                //Remove image
                fireBaseService.RemoveFileAsync(entity.FileName, FOLDER_NAME);
            }
            walletTypeRepository.Delete(id);
        }
        else
        {
            throw new InvalidParameterException("Không tìm thấy loại ví");
        }
    }

    public PagedResultModel<WalletTypeModel> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<WalletTypeModel>>
            (walletTypeRepository.GetAll(state, propertySort, isAsc, search, page, limit));
    }

    public WalletTypeModel GetById(string id)
    {
        WalletType entity = walletTypeRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<WalletTypeModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy loại ví");
    }

    public async Task<WalletTypeModel> Update(string id, UpdateWalletTypeModel update)
    {
        WalletType entity = walletTypeRepository.GetById(id);
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
            return mapper.Map<WalletTypeModel>(walletTypeRepository.Update(entity));
        }
        throw new InvalidParameterException("Không tìm thấy loại ví");
    }
}
