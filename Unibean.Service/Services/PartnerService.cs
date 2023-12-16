using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Partners;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Unibean.Service.Services;

public class PartnerService : IPartnerService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "partners";

    private readonly IPartnerRepository partnerRepository;

    private readonly IFireBaseService fireBaseService;

    public PartnerService(IPartnerRepository partnerRepository, 
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Partner, PartnerModel>().ReverseMap();
            cfg.CreateMap<Partner, PartnerExtraModel>()

            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Partner>, PagedResultModel<PartnerModel>>()
            .ReverseMap();
            cfg.CreateMap<Partner, CreatePartnerModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.Password, opt => opt.MapFrom(src => BCryptNet.HashPassword(src.Password)))
            .ForMember(t => t.Logo, opt => opt.Ignore())
            .ForMember(t => t.CoverPhoto, opt => opt.Ignore())
            .ForMember(t => t.TotalIncome, opt => opt.MapFrom(src => 0))
            .ForMember(t => t.TotalSpending, opt => opt.MapFrom(src => 0))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.partnerRepository = partnerRepository;
        this.fireBaseService = fireBaseService;
    }

    public async Task<PartnerExtraModel> Add(CreatePartnerModel creation)
    {
        Partner entity = mapper.Map<Partner>(creation);
        
        // Upload logo image
        if (creation.Logo != null && creation.Logo.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Logo, FOLDER_NAME);
            entity.Logo = f.URL;
            entity.LogoFileName = f.FileName;
        }

        // Upload cover photo
        if (creation.CoverPhoto != null && creation.CoverPhoto.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.CoverPhoto, FOLDER_NAME);
            entity.CoverPhoto = f.URL;
            entity.CoverFileName = f.FileName;
        }
        return mapper.Map<PartnerExtraModel>(partnerRepository.Add(entity));
    }

    public PartnerModel GetByUserNameAndPassword(string userName, string password)
    {
        return mapper.Map<PartnerModel>(partnerRepository.GetByUserNameAndPassword(userName, password));
    }
}
