using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Levels;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class LevelService : ILevelService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "levels";

    private readonly ILevelRepository levelRepository;

    private readonly IFireBaseService fireBaseService;

    public LevelService(ILevelRepository levelRepository,
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Level, LevelModel>().ReverseMap();
        });
        mapper = new Mapper(config);
        this.levelRepository = levelRepository;
        this.fireBaseService = fireBaseService;
    }

    public LevelModel GetLevelByName(string levelName)
    {
        return mapper.Map<LevelModel>(levelRepository.GetLevelByName(levelName));
    }
}
