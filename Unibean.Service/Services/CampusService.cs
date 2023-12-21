using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Campuses;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class CampusService : ICampusService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "campuses";

    private readonly ICampusRepository campusRepository;

    private readonly IFireBaseService fireBaseService;

    public CampusService(ICampusRepository campusRepository, 
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Campus, CampusModel>()
            .ForMember(c => c.UniversityName, opt => opt.MapFrom(src => src.University.UniversityName))
            .ForMember(c => c.AreaName, opt => opt.MapFrom(src => src.Area.AreaName))
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.campusRepository = campusRepository;
        this.fireBaseService = fireBaseService;
    }

    public CampusModel GetById(string id)
    {
        Campus entity = campusRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<CampusModel>(entity);
        }
        throw new InvalidParameterException("Not found campus");
    }
}
