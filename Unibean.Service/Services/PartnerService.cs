using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Partners;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class PartnerService : IPartnerService
{
    private readonly Mapper mapper;

    private readonly IPartnerRepository partnerRepository;

    private readonly IFireBaseService fireBaseService;

    public PartnerService(IPartnerRepository partnerRepository, 
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Partner, PartnerModel>().ReverseMap();
        });
        mapper = new Mapper(config);
        this.partnerRepository = partnerRepository;
        this.fireBaseService = fireBaseService;
    }

    public PartnerModel GetByUserNameAndPassword(string userName, string password)
    {
        return mapper.Map<PartnerModel>(partnerRepository.GetByUserNameAndPassword(userName, password));
    }
}
